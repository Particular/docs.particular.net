using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;
using Sample;


Console.Title = "Xml";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

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
await builder.Build().RunAsync();
