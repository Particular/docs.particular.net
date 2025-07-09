using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

var builder = Host.CreateApplicationBuilder(args);

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

builder.UseNServiceBus(endpointConfiguration);

Console.Title = "Server";

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine();
Console.WriteLine("Storage locations:");
Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

Console.WriteLine();
Console.WriteLine("Press 'Enter' to send a StartOrder message");
Console.WriteLine();


while (true)
{
    if (Console.ReadKey().Key != ConsoleKey.Enter)
    {
        break;
    }
    var orderId = Guid.NewGuid();
    var startOrder = new StartOrder
    {
        OrderId = orderId
    };
    await messageSession.SendLocal(startOrder);

    Console.WriteLine($"Sent StartOrder with OrderId {orderId}.");
}

await host.StopAsync();