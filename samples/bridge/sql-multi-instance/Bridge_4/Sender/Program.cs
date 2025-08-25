using System;
using System.Threading.Tasks;

using Messages;

using NServiceBus;

public class Program
{
    // for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100;Encrypt=false
    const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

    static async Task Main()
    {
        Console.Title = "MultiInstanceSender";

        #region SenderConfiguration

        var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
        var routing = endpointConfiguration.UseTransport(new SqlServerTransport(ConnectionString));

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.EnableInstallers();

        routing.RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");

        #endregion

        SqlHelper.EnsureDatabaseExists(ConnectionString);

        var endpointInstance = await Endpoint.Start(endpointConfiguration);

        Console.WriteLine("Press <enter> to send a message");
        Console.WriteLine("Press any other key to exit");
        while (true)
        {
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
            await PlaceOrder(endpointInstance);
        }
        await endpointInstance.Stop();
    }

    static async Task PlaceOrder(IEndpointInstance endpoint)
    {
        #region SendMessage

        var order = new ClientOrder
        {
            OrderId = Guid.NewGuid()
        };
        await endpoint.Send(order);

        #endregion

        Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
    }
}
