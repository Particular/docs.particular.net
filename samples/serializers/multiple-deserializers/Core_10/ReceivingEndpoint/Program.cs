using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "ReceivingEndpoint";

var builder = Host.CreateApplicationBuilder(args);

#region configAll

var endpointConfiguration = new EndpointConfiguration("Samples.MultipleDeserializers.ReceivingEndpoint");

// Xml
endpointConfiguration.UseSerialization<XmlSerializer>();

// Json
endpointConfiguration.AddDeserializer<SystemJsonSerializer>();

// External Newtonsoft Json
var externalNewtonsoftJson = endpointConfiguration.AddDeserializer<NewtonsoftJsonSerializer>();
externalNewtonsoftJson.ContentTypeKey("NewtonsoftJson");

// External Newtonsoft Bson
var externalNewtonsoftBson = endpointConfiguration.AddDeserializer<NewtonsoftJsonSerializer>();
externalNewtonsoftBson.ReaderCreator(stream => new BsonDataReader(stream));
externalNewtonsoftBson.WriterCreator(stream => new BsonDataWriter(stream));
externalNewtonsoftBson.ContentTypeKey("NewtonsoftBson");

// Register the mutator so the message on the wire is written
builder.Services.AddSingleton<IncomingMessageBodyWriter>();

var serviceProvider = builder.Services.BuildServiceProvider();
var incomingMessageBodyWriter = serviceProvider.GetRequiredService<IncomingMessageBodyWriter>();
endpointConfiguration.RegisterMessageMutator(incomingMessageBodyWriter);
#endregion

endpointConfiguration.UseTransport(new LearningTransport());
builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();
await host.StartAsync();

Console.WriteLine("Press any key to exit");
Console.ReadKey();

await host.StopAsync();
