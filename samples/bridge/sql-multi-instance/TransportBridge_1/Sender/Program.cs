using System;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

//for local instance or SqlExpress
const string ConnectionString = @"Data Source=(localdb)\mssqllocaldb;Database=NsbSamplesSqlMultiInstanceSender;Trusted_Connection=True;MultipleActiveResultSets=true;Max Pool Size=100;Encrypt=false";
//const string ConnectionString = @"Server=localhost,1433;Initial Catalog=NsbSamplesSqlMultiInstanceSender;User Id=SA;Password=yourStrong(!)Password;Max Pool Size=100;Encrypt=false";

Console.Title = "MultiInstanceSender";

Console.WriteLine("Starting...");

var builder = Host.CreateDefaultBuilder(args);

builder.UseNServiceBus(x =>
{
    #region SenderConfiguration
    var endpointConfiguration = new EndpointConfiguration("Samples.SqlServer.MultiInstanceSender");
    var transport = endpointConfiguration.UseTransport<SqlServerTransport>();
    transport.ConnectionString(ConnectionString);
    endpointConfiguration.UseSerialization<SystemJsonSerializer>();
    endpointConfiguration.EnableInstallers();

    transport.Routing().RouteToEndpoint(typeof(ClientOrder), "Samples.SqlServer.MultiInstanceReceiver");
    #endregion

    SqlHelper.EnsureDatabaseExists(ConnectionString);
    return endpointConfiguration;
});

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press <enter> to send a message");
Console.WriteLine("Press any other key to continue");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();
    if (key.Key != ConsoleKey.Enter)
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
