using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Publisher";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Publisher");
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

Console.WriteLine("Press '1' to publish the OrderReceived event");
Console.WriteLine("Press any other key to exit");

#region PublishLoop

while (true)
{
    var key = Console.ReadKey();
    Console.WriteLine();

    var orderReceivedId = Guid.NewGuid();
    if (key.Key == ConsoleKey.D1)
    {
        var orderReceived = new OrderReceived
        {
            OrderId = orderReceivedId
        };
        await messageSession.Publish(orderReceived);
        Console.WriteLine($"Published OrderReceived Event with Id {orderReceivedId}.");
    }
    else
    {
        break;
    }
}

#endregion

await host.StopAsync();