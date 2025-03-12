using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NServiceBus;
using NServiceBus.MessageMutator;
using Sample;


Console.Title = "ExternalJson";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

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
await builder.Build().RunAsync();
