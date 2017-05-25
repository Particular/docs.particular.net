using System;
using System.Threading.Tasks;
using NServiceBus;
using Subscriber2.Contracts;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.ConsumerDrivenContracts.Consumer2";
        var endpointConfiguration = new EndpointConfiguration("Samples.ConsumerDrivenContracts.Consumer2");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(Consumer2Contract), "Samples.ConsumerDrivenContracts.Producer");

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