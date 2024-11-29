using Microsoft.Extensions.Hosting;
using NServiceBus;
using Shared;

await Host.CreateDefaultBuilder(args)
    .UseNServiceBus(ctx =>
    {
        var endpointConfiguration = new EndpointConfiguration("Samples.Docker.Receiver");

        #region TransportConfiguration

        var transport = endpointConfiguration.UseTransport<RabbitMQTransport>();
        transport.ConnectionString("host=rabbitmq");
        transport.UseConventionalRoutingTopology(QueueType.Quorum);

        #endregion

        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.DefineCriticalErrorAction(CriticalErrorActions.RestartContainer);
        return endpointConfiguration;
    })
    .RunConsoleAsync();