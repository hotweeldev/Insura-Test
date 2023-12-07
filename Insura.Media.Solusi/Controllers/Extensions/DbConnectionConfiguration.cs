using Autofac;
using Insura.Media.Solusi.Common.Config;

namespace Insura.Media.Solusi.Controllers.Extensions
{
    public static class DbConnectionConfiguration
    {
        public static IServiceCollection RegisterDBConnection(this IServiceCollection services, DbConnection dbConnection)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (dbConnection != null)
            {
                services.AddSingleton(dbConnection);
            }
            return services;
        }

        public static ContainerBuilder RegisterDBConnection(this ContainerBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException("builder");
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            return builder;
        }
    }
}
