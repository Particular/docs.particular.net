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
            var endpointConfiguration = new EndpointConfiguration("CustomTelemetry");
            endpointConfiguration.UseTransport<LearningTransport>();

            endpointConfiguration.Pipeline.Register(
                new AddOrderIdToTrace(),
                "Adds order id to the ambient trace for incoming ShipOrder messages"
            );

            endpointConfiguration.Pipeline.Register(
                new TraceBillingActivities(),
                "Traces incoming billing messages"
            );

            endpointConfiguration.Pipeline.Register(
                new TraceOutgoingOrderIdsBehavior(),
                "Captures outgoing order ids as trace tags"
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