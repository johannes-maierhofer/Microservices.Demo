using BuildingBlocks.Mediatr.Behaviors;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Mediatr
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomMediatr(this IServiceCollection services, params Assembly[] assemblies)
        {
            if (assemblies.Length == 0)
            {
                assemblies = new[]{ Assembly.GetCallingAssembly() };
            }

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(assemblies);
                cfg.AddOpenBehavior(typeof(TracingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(LoggingBehaviour<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });
            
            return services;
        }
    }
}
