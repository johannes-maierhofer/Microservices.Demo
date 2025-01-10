using System.Reflection;
using Argo.MD.BuildingBlocks.Logging;
using Argo.MD.BuildingBlocks.Messaging.MassTransit;
using Argo.MD.BuildingBlocks.Tracing.OpenTelemetry;
using Argo.MD.EmailSender.Messages.Commands;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services
    .AddCustomMassTransit(
        hostBuilder.Configuration,
        hostBuilder.Environment,
        Assembly.GetExecutingAssembly(),
        [typeof(SendEmail).Assembly])
    .AddCustomOpenTelemetry()
    .AddCustomSerilog(hostBuilder.Configuration);

var host = hostBuilder.Build();

host.Run();
