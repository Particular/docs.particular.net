using Microsoft.Extensions.Hosting;
using NServiceBus;


var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("ClientUI");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

var routing = endpointConfiguration.UseTransport(new LearningTransport());

builder.UseNServiceBus(endpointConfiguration);

var host = builder.Build();

host.Run();
