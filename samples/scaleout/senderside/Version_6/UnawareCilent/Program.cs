using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfig = new BusConfiguration();
        busConfig.UsePersistence<InMemoryPersistence>();
        busConfig.SendFailedMessagesTo("error");

        busConfig.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(DoSomething), "Server");

        Run(busConfig).GetAwaiter().GetResult();
    }

    static async Task Run(BusConfiguration busConfig)
    {
        IEndpointInstance endpoint = await Endpoint.Start(busConfig);
        while (true)
        {
            Console.WriteLine("Press <enter> to send a message.");
            Console.ReadLine();
            await endpoint.Send(new DoSomething());
        }
        await endpoint.Stop();
    }
}