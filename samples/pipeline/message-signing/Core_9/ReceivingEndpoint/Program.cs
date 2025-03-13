using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

Console.Title = "ReceivingEndpoint";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Pipeline.SigningAndEncryption.ReceivingEndpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

endpointConfiguration.RegisterSigningBehaviors();

Console.WriteLine("Waiting to receive messages. Press Enter to exit.");
builder.UseNServiceBus(endpointConfiguration);
await builder.Build().RunAsync();
