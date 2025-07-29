using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Bson;
using NServiceBus;

Console.Title = "NewtonsoftBsonEndpoint";

#region configExternalNewtonsoftBson
var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ExternalNewtonsoftBsonEndpoint");

var serialization = endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();

serialization.ReaderCreator(stream => new BsonDataReader(stream));
serialization.WriterCreator(stream => new BsonDataWriter(stream));
serialization.ContentTypeKey("NewtonsoftBson");

endpointConfiguration.RegisterOutgoingMessageLogger();

#endregion
endpointConfiguration.UseTransport(new LearningTransport());

var builder = Host.CreateApplicationBuilder(args);
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

var messageSession = host.Services.GetRequiredService<IMessageSession>();
var message = MessageBuilder.BuildMessage();
await messageSession.Send("Samples.MultipleDeserializers.ReceivingEndpoint", message);

Console.WriteLine("Order Sent");
Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();