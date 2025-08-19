using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;


Console.Title = "Xml";

var builder = Host.CreateApplicationBuilder(args);

#region config
var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Xml");

endpointConfiguration.UseSerialization<XmlSerializer>();


// Then later get it from the service provider when needed
builder.Services.AddSingleton<MessageBodyWriter>();
var serviceProvider = builder.Services.BuildServiceProvider();
var messageBodyWriter = serviceProvider.GetRequiredService<MessageBodyWriter>();

// register the mutator so the the message on the wire is written
endpointConfiguration.RegisterMessageMutator(messageBodyWriter);

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