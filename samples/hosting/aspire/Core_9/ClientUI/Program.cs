using ClientUI;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();

builder.AddServiceDefaults();

var endpointConfiguration = new EndpointConfiguration("ClientUI");
endpointConfiguration.EnableOpenTelemetry();

var connectionString = builder.Configuration.GetConnectionString("transport");
var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(PlaceOrder), "Sales");

endpointConfiguration.EnableInstallers();

endpointConfiguration.UseSerialization<SystemJsonSerializer>();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<MessageSenderService>();

await builder.Build().RunAsync();
