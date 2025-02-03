using System.Reflection;
using Argo.MD.BuildingBlocks.Logging;
using Argo.MD.BuildingBlocks.Messaging.MassTransit;
using Argo.MD.BuildingBlocks.Tracing.OpenTelemetry;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services
    .AddCustomMassTransit(
        hostBuilder.Configuration,
        hostBuilder.Environment,
        Assembly.GetExecutingAssembly())
    .AddCustomOpenTelemetry()
    .AddCustomSerilog(hostBuilder.Configuration);

var host = hostBuilder.Build();

host.Run();
