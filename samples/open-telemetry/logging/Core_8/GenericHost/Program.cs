using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var endpointName = "Samples.OpenTelemetry.MyEndpoint";

var builder = Host.CreateDefaultBuilder(args);

#region otel-config

builder.ConfigureServices(services =>
{
    services.AddOpenTelemetry()
        .ConfigureResource(resourceBuilder => resourceBuilder.AddService(endpointName))
        .WithTracing(traceBuilder =>
        {
            traceBuilder.AddSource("NServiceBus.*");
            traceBuilder.AddConsoleExporter();
        });
});

#endregion

#region otel-logging

builder.ConfigureLogging((_, logging) =>
{
    logging.AddOpenTelemetry(loggingOptions =>
    {
        loggingOptions.IncludeFormattedMessage = true;
        loggingOptions.IncludeScopes = true;
        loggingOptions.ParseStateValues = true;
        loggingOptions.AddConsoleExporter();
    });

    logging.AddConsole();
});

#endregion

#region otel-nsb-config

builder.UseNServiceBus(_ =>
{
    var endpointConfiguration = new EndpointConfiguration(endpointName);
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.UseTransport(new LearningTransport());

    endpointConfiguration.EnableOpenTelemetry();

    return endpointConfiguration;
});

#endregion

builder.ConfigureServices(services => services.AddHostedService<MessageGenerator>());

var host = builder.Build();

await host.RunAsync();