using NServiceBus;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System;
using System.Threading.Tasks;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = "CustomTelemetry";

        #region open-telemetry-config
        var resourceBuilder = ResourceBuilder.CreateDefault()
            .AddService("CustomTelemetry");

        var tracerProviderBuilder = Sdk.CreateTracerProviderBuilder()
            .SetResourceBuilder(resourceBuilder)
            .AddSource("NServiceBus.Core")
            .AddSource(CustomActivitySources.Name)
            .AddProcessor(new NetHostProcessor())
            .AddConsoleExporter();
        #endregion

        using (var tracerProvider = tracerProviderBuilder.Build())
        {
            #region enable-opentelemetry
            var endpointConfiguration = new EndpointConfiguration("CustomTelemetry");
            endpointConfiguration.EnableOpenTelemetry();
            #endregion
            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.Pipeline.Register(
                new TraceOutgoingMessageSizeBehavior(),
                "Captures body size of outgoing messages as OpenTelemetry tags"
            );

            var endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Endpoint started.");

            var done = false;
            while (!done)
            {
                Console.WriteLine("Press ESC to stop.\nO. to create an order");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Escape:
                        done = true;
                        break;
                    case ConsoleKey.O:
                        await endpoint.SendLocal(new CreateOrder { OrderId = Guid.NewGuid() });
                        break;
                    default:
                        // Do nothing
                        break;
                }
            }

            await endpoint.Stop();
            Console.WriteLine("Endpoint stopped");
        }
    }
}
