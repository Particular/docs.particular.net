using System;
using System.Threading.Tasks;
using Messages;
using NServiceBus;

public class Program
{
    const string connectionString = @"Data Source=.\SqlExpress;Database=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100";

    static async Task Main()
    {
        Console.Title = "Samples.SqlServer.MultiInstanceSender";

        #region SenderConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
        transport.ConnectionString(connectionString);
        endpointConfiguration.EnableInstallers();
        
        transport.Routing().RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");

        #endregion

        SqlHelper.EnsureDatabaseExists(connectionString);

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
        await endpoint.Send(order)
            .ConfigureAwait(false);

        #endregion

        Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
    }
}