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

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
endpointConfiguration.AuditProcessedMessagesTo("audit");

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

builder.Services.AddHostedService<MessageSenderService>();

await builder.Build().RunAsync();
