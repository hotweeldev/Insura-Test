using AutoMapper;
using FluentValidation.AspNetCore;
using Insura.Media.Solusi.Common.Config;
using Insura.Media.Solusi.Consumer;
using Insura.Media.Solusi.Controllers.Extensions;
using Insura.Media.Solusi.Exceptions;
using Insura.Media.Solusi.Repository;
using Insura.Media.Solusi.Service;
using Insura.Media.Solusi.Service.Impl;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Yaml;
using RabbitQueue.Configuration;
using RabbitQueue.Connection;
using RabbitQueue.Service;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Insura.Media.Solusi
{
    public class Startup
    {
        public Startup() 
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddYamlFile("configuration.yml", optional: false, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            #region Common Config
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();
            #endregion

            #region DB Configuration
            var dbConnection = new DbConnection();
            Configuration.Bind("dbConnection", dbConnection);
            services.RegisterDBConnection(dbConnection);

            services.AddDbContext<DataContext>((provider, options) =>
            {
                var connectionString = provider.GetRequiredService<DbConnection>().ConnectionString;
                options.UseSqlServer(connectionString);
            });
            #endregion

            #region Rabbit MQ
            var rabbitConnection = new RabbitConnection();
            Configuration.GetSection("rabbitConnection").Bind(rabbitConnection);
            services.AddSingleton(rabbitConnection);

            services.AddSingleton<IBus>(sp => RabbitHutch.CreateBus
            (
                rabbitConnection.hostName, 
                rabbitConnection.hostPort, 
                rabbitConnection.virtualHost, 
                rabbitConnection.username, 
                rabbitConnection.password
            ));

            services.AddHostedService<UserLogActivityConsumer>();
            #endregion

            #region Service
            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<ICalculatorService, CalculatorServiceImpl>();
            services.AddScoped<ITaskService, TaskServiceImpl>();
            #endregion

            #region AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
            #endregion

            #region Fluent Validation
            services.AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
            #endregion

            #region CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            #endregion

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // ensure database is created
                var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
                var db = serviceScope?.ServiceProvider.GetRequiredService<DataContext>();
                db?.Database.EnsureCreated();

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature == null) return;

                    context.Response.StatusCode = contextFeature.Error switch
                    {
                        OperationCanceledException => (int)HttpStatusCode.ServiceUnavailable,
                        BadRequestException => (int)HttpStatusCode.BadRequest,
                        NotFoundException => (int)HttpStatusCode.NotFound,
                        _ => (int)HttpStatusCode.InternalServerError
                    };

                    string errorMessage = contextFeature.Error switch
                    {
                        BadRequestException badRequestEx => badRequestEx.Message,
                        _ => contextFeature.Error.Message
                    };

                    var errorResponse = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = errorMessage
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                });
            });


            app.UseCors();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
