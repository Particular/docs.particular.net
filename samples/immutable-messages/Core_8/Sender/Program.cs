using Messages;
using NServiceBus;
using System;
using System.Threading.Tasks;
using UsingClasses.Messages;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ImmutableMessages.UsingInterfaces.Sender";

#region immutable-messages-endpoint-config

        var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Sender");
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var routingConfiguration = endpointConfiguration.UseTransport(new LearningTransport());

#endregion

        routingConfiguration.RouteToEndpoint(typeof(MyMessageImpl), "Samples.ImmutableMessages.UsingInterfaces.Receiver");
        routingConfiguration.RouteToEndpoint(typeof(MyMessage), "Samples.ImmutableMessages.UsingInterfaces.Receiver");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        await MessageSender.Start(endpointInstance)
            .ConfigureAwait(false);
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}

