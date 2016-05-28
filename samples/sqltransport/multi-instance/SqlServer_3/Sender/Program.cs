using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Transport.SQLServer;
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

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.EnableLegacyMultiInstanceMode(ConnectionProvider.GetConnection);
        endpointConfiguration.UseSerialization<JsonSerializer>();
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");

        #endregion

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to send a message");
        Console.WriteLine("Press any other key to exit");
        try
        {
            while (true)
            {
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    return;
                }
                PlaceOrder(endpointInstance);
            }
        }
        finally
        {
            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }

    static void PlaceOrder(IEndpointInstance endpoint)
    {
        #region SendMessage

        var order = new ClientOrder
        {
            OrderId = Guid.NewGuid()
        };

        endpoint.Send("Samples.SqlServer.MultiInstanceReceiver", order);

        #endregion

        Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
    }

}
