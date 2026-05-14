using Microsoft.Extensions.Hosting;

var endpointName = "V1.Subscriber";
Console.Title = endpointName;
var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration(endpointName);
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.UseTransport(new LearningTransport());

builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

await builder.Build().RunAsync();