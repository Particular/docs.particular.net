using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus(_ =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");

        #region TransportConfiguration

        var connectionString = "host=rabbitmq";
        var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
        endpointConfiguration.UseTransport(transport);

        #endregion

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
        endpointConfiguration.EnableInstallers();

        return endpointConfiguration;
    })
    .Build()
    .RunAsync();