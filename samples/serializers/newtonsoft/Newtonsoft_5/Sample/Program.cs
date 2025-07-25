using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "ExternalJson";

var builder = Host.CreateApplicationBuilder(args);

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ExternalJson");

var settings = new JsonSerializerSettings
{
    Formatting = Formatting.Indented
};
var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.Settings(settings);

#endregion

endpointConfiguration.UseTransport(new LearningTransport());

#region registermutator
builder.Services.AddSingleton<MessageBodyWriter>();

// Then later get it from the service provider when needed
var serviceProvider = builder.Services.BuildServiceProvider();
var messageBodyWriter = serviceProvider.GetRequiredService<MessageBodyWriter>();
endpointConfiguration.RegisterMessageMutator(messageBodyWriter);

#endregion

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

Console.WriteLine("Order Sent");

#endregion

await host.StopAsync();