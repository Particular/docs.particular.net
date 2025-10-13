using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "SimpleSender";

var endpointConfiguration = new EndpointConfiguration("PostgreSql.SimpleSender");
endpointConfiguration.EnableInstallers();

#region TransportConfiguration
var connectionString = "User ID=user;Password=admin;Host=localhost;Port=54320;Database=nservicebus;Pooling=true;Connection Lifetime=0;Include Error Detail=true";
var routing = endpointConfiguration.UseTransport(new PostgreSqlTransport(connectionString)
{
    TransportTransactionMode = TransportTransactionMode.SendsAtomicWithReceive
});

routing.RouteToEndpoint(typeof(MyCommand), "PostgreSql.SimpleReceiver");

#endregion

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var builder = Host.CreateApplicationBuilder(args);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press [c] to send a command, or [e] to publish an event");
Console.WriteLine("Press any other key to exit");

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.C && key.Key != ConsoleKey.E)
    {
        break;
    }

    switch (key.Key)
    {
        case ConsoleKey.C:
            await messageSession.Send(new MyCommand());
            break;
        case ConsoleKey.E:
            await messageSession.Publish(new MyEvent());
            break;
        default:
            continue;
    }
}

await host.StopAsync();