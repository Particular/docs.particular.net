using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "Endpoint2";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Encryption.Endpoint2");
endpointConfiguration.Conventions().DefiningMessagesAs(type => type.Name.Contains("Message"));
endpointConfiguration.ConfigurationEncryption();
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();