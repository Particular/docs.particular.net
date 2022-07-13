using Azure.Monitor.OpenTelemetry.Exporter;
using NServiceBus;
using System;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;

internal class Program
{
    static async Task Main(string[] args)
    {
        Console.Title = "Samples.OpenTelemetry.AppInsights";

        var attributes = new Dictionary<string, object>
        {
            ["service.name"] = "Samples.OpenTelemetry.AppInsights",
            ["service.instance.id"] = Guid.NewGuid().ToString(),
        };

        var appInsightsConnectionString = "<YOUR KEY HERE>";

        var resourceBuilder = ResourceBuilder.CreateDefault().AddAttributes(attributes);
        using var traceProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("NServiceBus.*")
            .AddAzureMonitorTraceExporter(o => o.ConnectionString = appInsightsConnectionString)
            .AddConsoleExporter(o => o.Targets = OpenTelemetry.Exporter.ConsoleExporterOutputTargets.Console)
            .Build();

        var config = new EndpointConfiguration("Samples.OpenTelemetry.AppInsights");
        config.UseTransport<LearningTransport>();
        config.UsePersistence<LearningPersistence>();

        var endpointInstance = await Endpoint.Start(config);

        Console.WriteLine("Endpoint started. Press ESC to stop");

        while(Console.ReadKey(true).Key != ConsoleKey.Escape)
        {
            await endpointInstance.SendLocal(new SomeMessage());
        }

        await endpointInstance.Stop();
    }
}

class SomeMessage : IMessage
{

}

class SomeMessageHandler : IHandleMessages<SomeMessage>
{
    public Task Handle(SomeMessage message, IMessageHandlerContext context)
    {
        Console.WriteLine("Message handled");
        return Task.CompletedTask;
    }
}