using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Transport.Bridge.MsmqEndpoint";

        var endpointConfiguration = new EndpointConfiguration("Samples.Transport.Bridge.MsmqEndpoint");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.UsePersistence<NonDurablePersistence>();    

        var routingConfig = endpointConfiguration.UseTransport(new MsmqTransport());
        routingConfig.RegisterPublisher(typeof(OtherEvent), "Samples.Transport.Bridge.AsbEndpoint");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
