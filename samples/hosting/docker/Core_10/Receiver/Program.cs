using Microsoft.Extensions.Hosting;
using Shared;

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
endpointConfiguration.CustomDiagnosticsWriter((_, __) => Task.CompletedTask);

var connectionString = "host=rabbitmq";

#region TransportConfiguration

var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);

#endregion

endpointConfiguration.UseTransport(transport);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

await builder.Build()
    .RunAsync();