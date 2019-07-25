using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ImmutableMessages.UsingInterfaces.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Sender");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var transportConfiguration = endpointConfiguration.UseTransport<LearningTransport>();

        transportConfiguration.Routing()
            .RouteToEndpoint(typeof(MyMessage), "Samples.ImmutableMessages.UsingInterfaces.receiver");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await MessageSender.Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

