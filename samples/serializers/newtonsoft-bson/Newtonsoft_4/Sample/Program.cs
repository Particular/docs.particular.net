using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Bson;
using NServiceBus;
using NServiceBus.MessageMutator;
using Sample;

Console.Title = "ExternalBson";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.ExternalBson");

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

Console.WriteLine("Press any key, the application is starting");
Console.ReadKey();
Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
