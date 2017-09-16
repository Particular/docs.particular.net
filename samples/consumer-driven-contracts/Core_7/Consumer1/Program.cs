using System;
using System.Threading.Tasks;
using NServiceBus;
using Subscriber1.Contracts;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.ConsumerDrivenContracts.Consumer1";
        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Consumer1");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(Consumer1Contract), "Samples.ConsumerDrivenContracts.Producer");

        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");

        Console.ReadKey();

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

}