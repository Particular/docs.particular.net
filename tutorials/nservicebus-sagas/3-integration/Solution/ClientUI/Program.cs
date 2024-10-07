using ClientUI;
using Messages;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddWindowsService();

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<Worker>();

var host = builder.Build();

host.Run();
