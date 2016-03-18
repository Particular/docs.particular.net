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
        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SenderSideScaleOut.UnawareCilent");
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        endpointConfiguration.Routing()
            .UnicastRoutingTable
            .RouteToEndpoint(typeof(DoSomething), "Samples.SenderSideScaleOut.Server");

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);
        Console.WriteLine("Press enter to send a message");
        Console.WriteLine("Press any key to exit");
        while (true)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key != ConsoleKey.Enter)
            {
                break;
            }
            await endpoint.Send(new DoSomething());
            Console.WriteLine("Message Sent");
        }
        await endpoint.Stop();
    }
}