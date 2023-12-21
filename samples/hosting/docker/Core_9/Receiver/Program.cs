using Microsoft.Extensions.Hosting;

using Shared;

await ProceedIfRabbitMqIsAlive.WaitForRabbitMq("rabbitmq");

var builder = Host.CreateApplicationBuilder(args);

var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");

var rabbitMqConnectionString = "host=rabbitmq";
var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), rabbitMqConnectionString);

_ = endpointConfiguration.UseTransport(transport);

endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);

var app = builder.Build();
app.Run();
