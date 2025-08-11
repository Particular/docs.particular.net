using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

Console.Title = "SystemJson";

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.SystemJson");

#region config

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

#endregion

endpointConfiguration.UseTransport(new LearningTransport());
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();

#region message

var message = new CreateOrder
{
    OrderId = 9,
    Date = DateTime.Now,
    CustomerId = 12,
    OrderItems =
    [
        new OrderItem()
        {
            ItemId = 6,
            Quantity = 2
        },

        new OrderItem()
        {
            ItemId = 5,
            Quantity = 4
        }

    ]
};
await messageSession.SendLocal(message);

#endregion

Console.WriteLine("Order Sent");

await host.StopAsync();