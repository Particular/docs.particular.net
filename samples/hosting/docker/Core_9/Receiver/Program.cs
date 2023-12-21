using Microsoft.Extensions.Hosting;
using Shared;

await ProceedIfBrokerIsAlive.WaitForBroker("rabbitmq");

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");
endpointConfiguration.CustomDiagnosticsWriter((d, ct) => Task.CompletedTask);

var connectionString = "host=rabbitmq";

#region TransportConfiguration

var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);

#endregion

_ = endpointConfiguration.UseTransport(transport);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
app.Run();
