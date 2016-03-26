using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
#pragma warning disable 618


public class Program
{

    static IEndpointInstance endpoint;

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender";

        #region SenderConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .EnableLagacyMultiInstanceMode(ConnectionProvider.GetConnecton);
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
        var order = new ClientOrder
        {
            OrderId = Guid.NewGuid()
        };

        endpoint.Send(order);

        Console.WriteLine("ClientOrder message sent with ID {0}", order.OrderId);
    }

}
