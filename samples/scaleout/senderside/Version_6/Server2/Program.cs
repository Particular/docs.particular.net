using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

namespace Server2
{
    class Program
    {
        static void Main(string[] args)
        {
            BusConfiguration busConfig = new BusConfiguration();
            busConfig.EndpointName("Server");
            busConfig.EndpointInstanceId(() => ConfigurationManager.AppSettings["InstanceId"]);
            busConfig.UsePersistence<InMemoryPersistence>();
            busConfig.SendFailedMessagesTo("error");
            Run(busConfig).GetAwaiter().GetResult();
        }

        private static async Task Run(BusConfiguration busConfig)
        {
            IEndpointInstance endpoint = await Endpoint.Start(busConfig);
            Console.WriteLine("Press <enter> to exit.");
            Console.ReadLine();
            await endpoint.Stop();
        }
    }

    public class DoSomethingHandler : IHandleMessages<DoSomething>
    {
        public Task Handle(DoSomething message, IMessageHandlerContext context)
        {
            Console.WriteLine("Message received.");
            return Task.FromResult(0);
        }
    }
}
