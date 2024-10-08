using ClientUI;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
