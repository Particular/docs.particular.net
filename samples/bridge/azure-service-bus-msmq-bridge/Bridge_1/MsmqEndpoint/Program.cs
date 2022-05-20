using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Azure.ServiceBus.MsmqEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Azure.ServiceBus.MsmqEndpoint");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();    

        var routingConfig = endpointConfiguration.UseTransport(new MsmqTransport());
        routingConfig.RegisterPublisher(typeof(OtherEvent), "Samples.Azure.ServiceBus.AsbEndpoint");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
