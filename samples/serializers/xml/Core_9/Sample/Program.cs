using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NServiceBus;
using NServiceBus.MessageMutator;


Console.Title = "Xml";

var builder = Host.CreateApplicationBuilder(args);

#region config
var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Xml");

endpointConfiguration.UseSerialization<XmlSerializer>();

// The mutator needs a logger. The endpoint's service provider is only available once the
// host is built, but the mutator has to be registered before that, so the logger comes
// from a factory here instead of from a separately built service provider.
using var loggerFactory = LoggerFactory.Create(logging => logging.AddConsole());

// Register the mutator so the serialized message body on the wire gets logged
endpointConfiguration.RegisterMessageMutator(new MessageBodyWriter(loggerFactory.CreateLogger<MessageBodyWriter>()));

#endregion

endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

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
    OrderItems = new List<OrderItem>
    {
        new OrderItem
        {
            ItemId = 6,
            Quantity = 2
        },
        new OrderItem
        {
            ItemId = 5,
            Quantity = 4
        },
    }
};

await messageSession.SendLocal(message);

#endregion

Console.WriteLine("Order Sent");

await host.StopAsync();