using System;
using System.Threading.Tasks;
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

// register the mutator so the the message on the wire is written
builder.Services.AddSingleton<IncomingMessageBodyWriter>();

var serviceProvider = builder.Services.BuildServiceProvider();
var incomingMessageBodyWriter = serviceProvider.GetRequiredService<IncomingMessageBodyWriter>();
endpointConfiguration.RegisterMessageMutator(incomingMessageBodyWriter);
#endregion

endpointConfiguration.UseTransport(new LearningTransport());

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
