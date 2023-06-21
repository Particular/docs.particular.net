using System;
using System.Threading.Tasks;
using NServiceBus;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

class Program
{
    const string EndpointName = "Samples.OpenTelemetry.Subscriber";

    public static async Task Main()
    {
        Console.Title = EndpointName;

        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(EndpointName))
            .AddSource("NServiceBus.Core")
            .AddJaegerExporter()
            .Build();

        var endpointConfiguration = new EndpointConfiguration(EndpointName);

        endpointConfiguration.EnableOpenTelemetry();

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UseTransport(new LearningTransport());

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press any key to exit");
        _ = Console.ReadKey();

        await endpointInstance.Stop();
        tracerProvider.ForceFlush();
    }
}
