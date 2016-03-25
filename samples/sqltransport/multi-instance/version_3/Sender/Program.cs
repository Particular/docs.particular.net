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
                Console.WriteLine("Press <enter> to send a message");

                while (true)
                {
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
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

        public class OrderAcceptedHandler : IHandleMessages<ClientOrderAccepted>
        {
            public Task Handle(ClientOrderAccepted message, IMessageHandlerContext context)
            {
                Console.WriteLine("Received ClientOrderAccepted for ID {0}", message.OrderId);

                return Task.FromResult(0);
            }
        }
    }
}
