using System;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

// for SqlExpress use Data Source=.\SqlExpress;Initial Catalog=NsbSamplesSqlMultiInstanceSender;Integrated Security=True;Max Pool Size=100;Encrypt=false
const string connectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

Console.Title = "MultiInstanceSender";

#region SenderConfiguration

var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
var routing = endpointConfiguration.UseTransport(new SqlServerTransport(connectionString));

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();

routing.RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");

#endregion

SqlHelper.EnsureDatabaseExists(connectionString);

var builder = Host.CreateApplicationBuilder();
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press <enter> to send a message");
Console.WriteLine("Press any other key to exit");
while (true)
{
    if (Console.ReadKey().Key != ConsoleKey.Enter)
    {
        break;
    }
    #region SendMessage

    var order = new ClientOrder
    {
        OrderId = Guid.NewGuid()
    };
    await messageSession.Send(order);

    #endregion

    Console.WriteLine($"ClientOrder message sent with ID {order.OrderId}");
}
await host.StopAsync();