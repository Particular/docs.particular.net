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

        #region Logical-Routing

        busConfig.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(DoSomething), "Server");

        #endregion

        #region File-Based-Routing

        busConfig.Routing().UseFileBasedEndpointInstanceMapping("routes.xml");

        #endregion

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