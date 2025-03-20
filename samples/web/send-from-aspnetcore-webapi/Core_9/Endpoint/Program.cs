using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;


Console.Title = "ASPNETCoreEndpoint";
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.ASPNETCore.Endpoint");
endpointConfiguration.UsePersistence<LearningPersistence>();
endpointConfiguration.UseTransport(new LearningTransport());
endpointConfiguration.UseSerialization<SystemJsonSerializer>();

Console.WriteLine("Press any key");
Console.ReadKey();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();