using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.Transport.IBMMQ;

Console.Title = "Orders";
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

var endpointConfiguration = new EndpointConfiguration("Orders");
endpointConfiguration.SendFailedMessagesTo("error");
endpointConfiguration.UseTransport(ibmmq);
endpointConfiguration.Recoverability().Delayed(settings => settings.NumberOfRetries(0));

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var session = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [O] for a standard order, [E] for an express order, [Q] to quit.");

while (true)
{
    var key = Console.ReadKey(true);

    if (key.Key == ConsoleKey.Q)
        break;

    var orderId = Guid.CreateVersion7();

    #region PublishOrders
    if (key.Key == ConsoleKey.E)
    {
        await session.Publish(new ExpressOrderPlaced(orderId, "Widget"));
        Console.WriteLine($"Published ExpressOrderPlaced {orderId}");
    }
    else if (key.Key == ConsoleKey.O)
    {
        await session.Publish(new OrderPlaced(orderId, "Widget"));
        Console.WriteLine($"Published OrderPlaced {orderId}");
    }
    #endregion
}

await host.StopAsync();
