using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
using UserImplementation;

namespace Client
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
            Console.Title = "Samples.SqlServer.MultiInstance Client";
            #region EndpointConfiguration
            var configuration = new EndpointConfiguration();
            configuration.UseTransport<SqlServerTransport>()
                                .EnableLagacyMultiInstanceMode(EndpointConnectionLookup.GetLookupFunc());
            configuration.EndpointName("Samples.SqlServer.MultiInstanceClient");
            configuration.UseSerialization<JsonSerializer>();
            #endregion

            configuration.UsePersistence<InMemoryPersistence>();
            configuration.SendFailedMessagesTo("error");

            endpoint = await Endpoint.Start(configuration);
            try
            {
                Console.WriteLine("Client running, Press Enter key to quit");
                Console.WriteLine("Press space bar to send a message");

                while (true)
                {
                    var key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key == ConsoleKey.Enter)
                    {
                        await endpoint.Stop(); ;
                    }
                    if (key.Key == ConsoleKey.Spacebar)
                    {
                        PlaceOrder();
                    }
                }
            }
            finally
            {
                await endpoint.Stop();
            }
        }

        private static void PlaceOrder()
        {
            #region MessagePayload

            var order = new ClientOrder
            {
                OrderId = Guid.NewGuid()
            };

            #endregion

            endpoint.Send(order);
            Console.WriteLine("ClientOrder message sent with ID {0}", order.OrderId);
        }
    }
}
