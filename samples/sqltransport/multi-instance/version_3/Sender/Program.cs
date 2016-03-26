using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transports.SQLServer;
#pragma warning disable 618


public class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender";

        #region SenderConfiguration

        EndpointConfiguration endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        endpointConfiguration.UseTransport<SqlServerTransport>()
            .EnableLagacyMultiInstanceMode(ConnectionProvider.GetConnecton);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #endregion

        IEndpointInstance endpoint = await Endpoint.Start(endpointConfiguration);

        try
        {
            Console.WriteLine("Press <enter> to send a message");

            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Enter)
                {
                    PlaceOrder(endpoint);
                }
            }
        }
        finally
        {
            await endpoint.Stop();
        }
    }

    static void PlaceOrder(IEndpointInstance endpoint)
    {
        ClientOrder order = new ClientOrder
        {
            OrderId = Guid.NewGuid()
        };

        endpoint.Send(order);

        Console.WriteLine("ClientOrder message sent with ID {0}", order.OrderId);
    }

}
