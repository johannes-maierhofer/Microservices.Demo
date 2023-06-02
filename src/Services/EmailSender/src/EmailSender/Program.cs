﻿using Microsoft.AspNetCore.Builder;
using System.Reflection;
using BuildingBlocks.Logging;
using BuildingBlocks.Messaging.MassTransit;
using BuildingBlocks.Tracing.OpenTelemetry;

var hostBuilder = WebApplication.CreateBuilder(args);

hostBuilder.Services
    .AddCustomMassTransit(hostBuilder, Assembly.GetExecutingAssembly())
    .AddCustomOpenTelemetry();

hostBuilder.AddCustomSerilog();

var host = hostBuilder.Build();

await host.StartAsync();

Console.WriteLine("Application started. Press Esc to quit");

ConsoleKeyInfo cki;
do
{
    cki = Console.ReadKey();
} while (cki.Key != ConsoleKey.Escape);

await host.StopAsync();