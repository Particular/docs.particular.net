using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Server";
var endpointConfiguration = new EndpointConfiguration("Samples.SimpleSaga");

#region config

endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

var builder = Host.CreateApplicationBuilder(args);

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine();
Console.WriteLine("Storage locations:");
Console.WriteLine($"Learning Persister: {LearningLocationHelper.SagaDirectory}");
Console.WriteLine($"Learning Transport: {LearningLocationHelper.TransportDirectory}");

Console.WriteLine();
Console.WriteLine("Press 'Enter' to send a StartOrder message");
Console.WriteLine("Press any other key to exit");

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