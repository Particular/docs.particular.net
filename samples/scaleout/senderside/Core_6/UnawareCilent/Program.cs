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
        Console.Title = "Samples.SenderSideScaleOut.UnawareCilent";
        var endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.UnawareCilent");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        var routing = endpointConfiguration.UnicastRouting();
        routing.RouteToEndpoint(typeof(DoSomething), "Samples.SenderSideScaleOut.Server");

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