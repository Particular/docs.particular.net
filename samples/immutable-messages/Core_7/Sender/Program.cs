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
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var transportConfiguration = endpointConfiguration.UseTransport<LearningTransport>();

#endregion

        var routingConfiguration = transportConfiguration.Routing();
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

