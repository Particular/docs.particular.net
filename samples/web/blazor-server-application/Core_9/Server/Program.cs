using Microsoft.Extensions.Hosting;
using NServiceBus;
using System;

Console.Title = "BlazorServer";
var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration(Console.Title = "BlazorServer");
endpointConfiguration.EnableCallbacks(makesRequests: false);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

var endpointInstance = await Endpoint.Start(endpointConfiguration);
Console.WriteLine("Press any key to exit");
Console.ReadKey();
await endpointInstance.Stop();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build().RunAsync();