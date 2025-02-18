using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;
using Shared;

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");

        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), "host=rabbitmq");
        var routing = endpointConfiguration.UseTransport(transport);

        routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);

        endpointConfiguration.EnableInstallers();

        return endpointConfiguration;
    })
    .ConfigureServices(services => services.AddHostedService<MessageSender>())
    .Build()
    .RunAsync();