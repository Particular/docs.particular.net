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

        #region Logical-Routing

        busConfiguration.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(DoSomething), "Server");

        #endregion

        #region File-Based-Routing

        busConfiguration.Routing().UseFileBasedEndpointInstanceMapping("routes.xml");

        #endregion

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