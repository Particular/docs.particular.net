using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Logging;
using Subscriber1.Contracts;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.MultipleInheritance.Consumer1";
        LogManager.Use<DefaultFactory>().Level(LogLevel.Info);
        var endpointConfiguration = new EndpointConfiguration("Samples.MultipleInheritance.Consumer1");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        var transport = endpointConfiguration.UseTransport<MsmqTransport>();

        var routing = transport.Routing();
        routing.RegisterPublisher(typeof(Consumer1Contract), "Samples.MultipleInheritance.Producer");

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