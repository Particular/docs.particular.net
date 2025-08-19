using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Client";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.RavenDB.Client");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press 'enter' to send a StartOrder messages");
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.Enter)
    {
        break;
    }

    var orderId = Guid.NewGuid();
    var startOrder = new StartOrder
    {
        OrderId = orderId
    };
    await messageSession.Send("Samples.RavenDB.Server", startOrder);
    Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");
}

await host.StopAsync();
