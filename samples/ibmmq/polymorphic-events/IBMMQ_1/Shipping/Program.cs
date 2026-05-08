using Microsoft.Extensions.Hosting;
using NServiceBus.Transport.IBMMQ;

Console.Title = "Shipping";
var builder = Host.CreateApplicationBuilder(args);

var ibmmq = new IBMMQTransport()
{
    QueueManagerName = "QM1",
    Host = "localhost",
    Port = 1414,
    Channel = "DEV.ADMIN.SVRCONN",
    User = "admin",
    Password = "passw0rd"
};

#region SubscribeRoutes
// Explicitly subscribe to both concrete types' topics when handling OrderPlaced
ibmmq.Topology.SubscribeTo<OrderPlaced, OrderPlaced>();
ibmmq.Topology.SubscribeTo<OrderPlaced, ExpressOrderPlaced>();
#endregion

var endpointConfiguration = new EndpointConfiguration("Shipping");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.UseTransport(ibmmq);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));

endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.RunAsync();
