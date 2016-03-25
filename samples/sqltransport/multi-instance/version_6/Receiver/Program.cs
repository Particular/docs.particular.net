using System;
using System.Threading.Tasks;
using EndpointConnectionStringLookup;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

namespace Receiver
{
    class Program
    {
        private static IEndpointInstance endpoint;

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Samples.SqlServer.MultiInstance Receiver";
            #region EndpointConfiguration
            var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceReceiver");
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .EnableLagacyMultiInstanceMode(EndpointConnectionLookup.GetLookupFunc());
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            endpoint = await Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Receiver running. Press Enter key to quit");
            Console.WriteLine("Waiting for Order messages from the Sender");

            while (true)
            {
                var key = Console.ReadKey();
                Console.WriteLine();

                if (key.Key != ConsoleKey.Enter) continue;
                await endpoint.Stop(); ;
            }
        }

        public class OrderHandler : IHandleMessages<ClientOrder>
        {
            public Task Handle(ClientOrder message, IMessageHandlerContext context)
            {
                Console.WriteLine("Handled ClientOrder with ID {0}", message.OrderId);
                return Task.FromResult(0);
            }
        }

    }
}
