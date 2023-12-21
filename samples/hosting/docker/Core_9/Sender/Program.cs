using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Shared;

using System.Reflection.Metadata.Ecma335;

await ProceedIfRabbitMqIsAlive.WaitForRabbitMq("rabbitmq");

var builder = Host.CreateApplicationBuilder(args);
var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");
endpointConfiguration.CustomDiagnosticsWriter((d, ct) => Task.CompletedTask);

var rabbitMqConnectionString = "host=rabbitmq";
var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), rabbitMqConnectionString);

var routing = endpointConfiguration.UseTransport(transport);

routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

// Message serialization
endpointConfiguration.UseSerialization<SystemJsonSerializer>();
endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
endpointConfiguration.EnableInstallers();

builder.UseNServiceBus(endpointConfiguration);
builder.Services.AddHostedService<MessageSender>();

var app = builder.Build();
app.Run();
