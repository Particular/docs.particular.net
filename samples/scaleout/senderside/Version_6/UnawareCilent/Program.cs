using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace UnawareCilent
{
    class Program
    {
        static void Main(string[] args)
        {
            BusConfiguration busConfig = new BusConfiguration();
            busConfig.UsePersistence<InMemoryPersistence>();
            busConfig.SendFailedMessagesTo("error");

            busConfig.Routing().UnicastRoutingTable.RouteToEndpoint(typeof(DoSomething), "Server");

            Run(busConfig).GetAwaiter().GetResult();
        }

        private static async Task Run(BusConfiguration busConfig)
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
}
