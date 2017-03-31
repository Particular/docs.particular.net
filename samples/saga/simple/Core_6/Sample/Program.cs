using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SimpleSaga";
        var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        var startOrder1 = new StartOrder
        {
            OrderId = "1"
        };
        await endpointInstance.SendLocal(startOrder1)
            .ConfigureAwait(false);

        var startOrder2 = new StartOrder
        {
            OrderId = "2"
        };
        await endpointInstance.SendLocal(startOrder2)
            .ConfigureAwait(false);

        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}
