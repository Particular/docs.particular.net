using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using NServiceBus.Routing;


Console.Title = "Subscriber";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.PubSub.Subscriber");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();