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
        Console.Title = "Samples.SenderSideScaleOut.AwareClient";
        var endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.AwareClient");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #region Logical-Routing

        var routing = endpointConfiguration.UnicastRouting();
        routing.RouteToEndpoint(typeof(DoSomething), "Samples.SenderSideScaleOut.Server");

        #endregion

        #region File-Based-Routing

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.DistributeMessagesUsingFileBasedEndpointInstanceMapping("routes.xml");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            var key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            await endpointInstance.Send(new DoSomething())
                .ConfigureAwait(false);
            Console.WriteLine("Message Sent");
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}