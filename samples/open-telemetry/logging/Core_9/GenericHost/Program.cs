using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var endpointName = "Samples.OpenTelemetry.MyEndpoint";

var builder = Host.CreateApplicationBuilder(args);

#region otel-config

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resourceBuilder => resourceBuilder.AddService(endpointName))
    .WithTracing(traceBuilder =>
    {
        traceBuilder.AddSource("NServiceBus.*");
        traceBuilder.AddConsoleExporter();
    });

#endregion

#region otel-logging

builder.Logging.AddOpenTelemetry(loggingOptions =>
{
    loggingOptions.IncludeFormattedMessage = true;
    loggingOptions.IncludeScopes = true;
    loggingOptions.ParseStateValues = true;
    loggingOptions.AddConsoleExporter();
});

#endregion

#region otel-nsb-config

var endpointConfiguration = new EndpointConfiguration(endpointName);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.EnableOpenTelemetry();

builder.UseNServiceBus(endpointConfiguration);

#endregion

builder.Services.AddHostedService<MessageGenerator>();

var host = builder.Build();

await host.RunAsync();