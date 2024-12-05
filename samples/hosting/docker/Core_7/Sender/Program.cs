using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NServiceBus;
using Sender;
using Shared;

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Sender");

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=rabbitmq");
        transport.UseConventionalRoutingTopology(QueueType.Quorum);

        var routing = transport.Routing();
        routing.RouteToEndpoint(typeof(RequestMessage), "Samples.Docker.Receiver");

        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
        return endpointConfiguration;
    })
    .ConfigureServices(services => services.AddHostedService<MessageSender>())
    .Build()
    .RunAsync();