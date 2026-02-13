using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var endpointName = "ClientUI";

Console.Title = endpointName;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration(endpointName);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<InputLoopService>();

await builder.Build().RunAsync();