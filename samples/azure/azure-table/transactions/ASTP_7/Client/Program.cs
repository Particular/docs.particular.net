using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "Client";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.AzureTable.Transactions.Client");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();



Console.WriteLine("Press any key, the application is starting");
Console.TreatControlCAsInput = true;
var input = Console.ReadKey();
if (input.Key == ConsoleKey.C && (input.Modifiers & ConsoleModifiers.Control) != 0)
{
    Environment.Exit(0);
}
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press 'S' to send a message");
while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    if (key.Key != ConsoleKey.S)
    {
        break;
    }

    var orderId = Guid.NewGuid();
    var startOrder = new StartOrder
    {
        OrderId = orderId
    };

    await messageSession.Send("Samples.AzureTable.Transactions.Server", startOrder);
    Console.WriteLine($"StartOrder Message sent to Server with OrderId {orderId}");
}

await host.StopAsync();
