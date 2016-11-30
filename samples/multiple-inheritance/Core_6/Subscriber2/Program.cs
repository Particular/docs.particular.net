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
        Console.Title = "Samples.MultipleInheritance.Subscriber2";
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleInheritance.Subscriber2");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(Subscriber2Event), "Samples.MultipleInheritance.Publisher");

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