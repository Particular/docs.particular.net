﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


Console.Title = "Client";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.MongoDB.Client");
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
Console.WriteLine("Press 'enter' to send a StartOrder messages");

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

    await messageSession.Send("Samples.MongoDB.Server", startOrder);

    Console.WriteLine($"StartOrder Message sent with OrderId {orderId}");

}

await builder.Build().StopAsync();