using AutoMapper;
using FluentValidation.AspNetCore;
using Insura.Media.Solusi.Exceptions;
using Insura.Media.Solusi.Repository;
using Insura.Media.Solusi.Service;
using Insura.Media.Solusi.Service.Impl;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Insura.Media.Solusi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddMemoryCache();

            var connectionString = _configuration.GetConnectionString("DataContext");
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IUserService, UserServiceImpl>();
            services.AddScoped<ICalculatorService, CalculatorServiceImpl>();
            services.AddScoped<ITaskService, TaskServiceImpl>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddFluentValidation(config => config.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()));
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

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
