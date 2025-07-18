using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sender;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");
endpointConfiguration.CustomDiagnosticsWriter((d, ct) => Task.CompletedTask);

var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");
var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);

var routing = endpointConfiguration.UseTransport(transport);

routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<MessageSender>();

await builder.Build()
    .RunAsync();