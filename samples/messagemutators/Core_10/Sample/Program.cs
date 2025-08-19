using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus.MessageMutator;

Console.Title = "MessageMutators";
var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Runner>();
var endpointConfiguration = new EndpointConfiguration("Samples.MessageMutators");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

#region ComponentRegistration
builder.Services.AddSingleton<IMutateIncomingMessages, ValidationMessageMutator>();
builder.Services.AddSingleton<IMutateOutgoingMessages, ValidationMessageMutator>();

// Add the compression mutator
builder.Services.AddSingleton<IMutateIncomingTransportMessages, TransportMessageCompressionMutator>();
builder.Services.AddSingleton<IMutateOutgoingTransportMessages, TransportMessageCompressionMutator>();

#endregion

Console.WriteLine("Starting...");

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();