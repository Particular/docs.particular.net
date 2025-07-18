using Microsoft.Extensions.Hosting;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
endpointConfiguration.CustomDiagnosticsWriter((_, __) => Task.CompletedTask);

var connectionString = Environment.GetEnvironmentVariable("CONNECTIONSTRING");

#region TransportConfiguration

var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);

#endregion

endpointConfiguration.UseTransport(transport);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

var metrics = endpointConfiguration.EnableMetrics();
metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

endpointConfiguration.EnableInstallers();
builder.UseNServiceBus(endpointConfiguration);

await builder.Build()
    .RunAsync();