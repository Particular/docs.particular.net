using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "ExternalBson";

var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ExternalBson");

#region config

var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
serialization.ContentTypeKey("application/bson");
serialization.ReaderCreator(stream => new BsonDataReader(stream));
serialization.WriterCreator(stream => new BsonDataWriter(stream));

#endregion

#region registermutator
builder.Services.AddSingleton<MessageBodyWriter>();

// Then later get it from the service provider when needed
var serviceProvider = builder.Services.BuildServiceProvider();
var messageBodyWriter = serviceProvider.GetRequiredService<MessageBodyWriter>();
endpointConfiguration.RegisterMessageMutator(messageBodyWriter);

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
        new OrderItem
        {
            ItemId = 6,
            Quantity = 2
        },
        new OrderItem
        {
            ItemId = 5,
            Quantity = 4
        }

    ]
};
await messageSession.SendLocal(message);
#endregion

Console.WriteLine("Order Sent, waiting 5s for receipt");
await Task.Delay(5_000);

Console.WriteLine("Shutting down");
await host.StopAsync();
