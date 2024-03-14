using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using UsingClasses.Messages;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ImmutableMessages.UsingInterfaces.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.ImmutableMessages.UsingInterfaces.Sender");
        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.UsePersistence<LearningPersistence>();
        var routingConfiguration = endpointConfiguration.UseTransport(new LearningTransport());

        routingConfiguration.RouteToEndpoint(typeof(MyMessageImpl), "Samples.ImmutableMessages.UsingInterfaces.Receiver");
        routingConfiguration.RouteToEndpoint(typeof(MyMessage), "Samples.ImmutableMessages.UsingInterfaces.Receiver");

        endpointConfiguration.ApplyCustomConventions();

        var endpointInstance = await Endpoint.Start(endpointConfiguration);
        await MessageSender.Start(endpointInstance);
        await endpointInstance.Stop();
    }
}

