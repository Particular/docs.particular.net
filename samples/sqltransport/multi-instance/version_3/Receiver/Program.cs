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
                .EnableLagacyMultiInstanceMode(ConnectionProvider.GetConnecton);
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            endpoint = await Endpoint.Start(endpointConfiguration);

            Console.WriteLine("Receiver running. Press <enter> key to quit");
            Console.WriteLine("Waiting for Order messages from the Sender");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    await endpoint.Stop();
                    break;
                }
            }
        }

        public class OrderHandler : IHandleMessages<ClientOrder>
        {
            public async Task Handle(ClientOrder message, IMessageHandlerContext context)
            {
                Console.WriteLine("Handling ClientOrder with ID {0}", message.OrderId);

                await context.Reply(new ClientOrderAccepted {OrderId = message.OrderId});
            }
        }
    }
}
