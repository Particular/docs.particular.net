using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "EndpointsMonitor";
        var endpointConfiguration = new EndpointConfiguration("EndpointsMonitor");
        endpointConfiguration.UseTransport<MsmqTransport>();
        endpointConfiguration.UseSerialization<NewtonsoftSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        var routing = transport.Routing();
        routing.RegisterPublisher(
            typeof(ServiceControl.Contracts.MessageFailed).Assembly,
            "Particular.ServiceControl"
        );

        var conventions = endpointConfiguration.Conventions();
        conventions.DefiningEventsAs(
            type =>
            {
                return typeof(IEvent).IsAssignableFrom(type) ||
                       // include ServiceControl events
                       type.Namespace != null &&
                       type.Namespace.StartsWith("ServiceControl.Contracts");
            });


        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press any key to finish.");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}