using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
#pragma warning disable 618

public class Program
{
    const string ConnectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender4;Integrated Security=True;Max Pool Size=100";

    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender.V4";

        #region SenderConfigurationV4

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(ConnectionString);
        endpointConfiguration.UsePersistence<InMemoryPersistence>();
        endpointConfiguration.SendFailedMessagesTo("error");
        endpointConfiguration.EnableInstallers();

        var bridgeConfig = transport.Routing().ConnectToBridge("Bridge-Sender");
        bridgeConfig.RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");
        #endregion

        endpointConfiguration.Conventions().DefiningMessagesAs(t => t.Assembly == typeof(ClientOrder).Assembly && t.Namespace == "Messages");

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to send a message");
        Console.WriteLine("Press any other key to exit");
        while (true)
        {
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
            await PlaceOrder(endpointInstance)
                .ConfigureAwait(false);
        }
        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }

    static async Task PlaceOrder(IEndpointInstance endpoint)
    {
        #region SendMessage

        var order = new ClientOrder
        {
            OrderId = Guid.NewGuid()
        };
        await endpoint.Send(order).ConfigureAwait(false);

        #endregion

        Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
    }
}