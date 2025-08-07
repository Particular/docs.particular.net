using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.MessageMutator;

Console.Title = "Avro";

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<InputLoopService>();

#region config

var endpointConfiguration = new EndpointConfiguration("Samples.Serialization.Avro");

endpointConfiguration.UseSerialization<AvroSerializer>();

#endregion

endpointConfiguration.UseTransport(new LearningTransport());

#region register-body-writer

endpointConfiguration.Pipeline.Register(typeof(MessageBodyWriter), "Logs the message body received");

#endregion

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();