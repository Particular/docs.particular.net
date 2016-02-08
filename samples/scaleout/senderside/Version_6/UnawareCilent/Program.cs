using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    static void Main()
    {
        BusConfiguration busConfiguration = new BusConfiguration();
        busConfiguration.UsePersistence<InMemoryPersistence>();
        busConfiguration.SendFailedMessagesTo("error");

        busConfiguration.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(DoSomething), "Server");

        Run(busConfiguration).GetAwaiter().GetResult();
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