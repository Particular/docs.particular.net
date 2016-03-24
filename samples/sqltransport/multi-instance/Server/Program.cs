using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using UserImplementation;

namespace Server
{
    class Program
    {
        private static IEndpointInstance endpoint;

        static void Main(string[] args)
        {
            AsyncMain().GetAwaiter().GetResult();
        }

        static async Task AsyncMain()
        {
            Console.Title = "Samples.SqlServer.MultiInstance Server";
            #region EndpointConfiguration
            var configuration = new EndpointConfiguration();
            configuration.EndpointName("Samples.SqlServer.MultiInstanceServer");
            configuration.UseTransport<SqlServerTransport>()
                .EnableLagacyMultiInstanceMode(EndpointConnectionLookup.GetLookupFunc());

            configuration.UseSerialization<JsonSerializer>();
            #endregion

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.SendFailedMessagesTo("error");

            endpoint = await Endpoint.Start(configuration);
            Console.WriteLine("Server running. Press Enter key to quit");
            Console.WriteLine("Waiting for Order messages from the client");


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
