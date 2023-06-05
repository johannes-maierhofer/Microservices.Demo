using System.Reflection;
using BuildingBlocks.Logging;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Tracing.OpenTelemetry;
using Microsoft.Extensions.Hosting;

var hostBuilder = Host.CreateApplicationBuilder(args);

hostBuilder.Services
    .AddCustomMassTransit(
        hostBuilder.Configuration,
        hostBuilder.Environment,
        Assembly.GetExecutingAssembly())
    .AddCustomOpenTelemetry()
    .AddCustomSerilog(hostBuilder.Configuration);

var host = hostBuilder.Build();

await host.StartAsync();

Console.WriteLine("Application started. Press Esc to quit");

ConsoleKeyInfo cki;
do
{
    cki = Console.ReadKey();
} while (cki.Key != ConsoleKey.Escape);

await host.StopAsync();