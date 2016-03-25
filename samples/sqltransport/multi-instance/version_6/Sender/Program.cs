using System;
using System.Threading.Tasks;
using EndpointConnectionStringLookup;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;

namespace Sender
{

class Program
{

    static IEndpointInstance endpoint;

        static void Main()
        {
            AsyncMain().GetAwaiter().GetResult();
        }
        static async Task AsyncMain()
        {
            Console.Title = "Samples.SqlServer.MultiInstance Sender";
            #region EndpointConfiguration
            var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
            endpointConfiguration.UseTransport<SqlServerTransport>()
                                .EnableLagacyMultiInstanceMode(EndpointConnectionLookup.GetLookupFunc());
            endpointConfiguration.UseSerialization<JsonSerializer>();
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.SendFailedMessagesTo("error");
            #endregion

            endpoint = await Endpoint.Start(endpointConfiguration);
            try
            {
                Console.WriteLine("Sender running, Press Enter key to quit");
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

        static void PlaceOrder()
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
