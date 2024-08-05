using System.Reflection;
using Argo.MD.BuildingBlocks.Mediatr.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace Argo.MD.BuildingBlocks.Mediatr
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
                cfg.AddOpenBehavior(typeof(CommandTransactionBehavior<,>));
            });
            
            return services;
        }
    }
}
