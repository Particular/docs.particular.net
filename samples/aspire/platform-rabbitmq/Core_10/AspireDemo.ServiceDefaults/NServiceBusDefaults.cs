using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Transport;

namespace Microsoft.Extensions.Hosting;

public static class NServiceBusDefaults
{
    extension(IHostApplicationBuilder builder)
    {
        public IHostApplicationBuilder AddNServiceBusEndpoint(string name,
            Action<EndpointConfiguration, RoutingSettings>? configureEndpoint = null)
        {
            var endpointConfiguration = new EndpointConfiguration(name);

            #region transport-config
            var connectionString = builder.Configuration.GetConnectionString("transport")
                    ?? throw new InvalidOperationException($"Endpoint '{name}' has no transport configured. Provide a 'ConnectionStrings:transport' connection string.");
            var transport = new RabbitMQTransport(RoutingTopology.Conventional(QueueType.Quorum), connectionString);
            var routing = endpointConfiguration.UseTransport(transport);
            #endregion

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
            endpointConfiguration.AuditProcessedMessagesTo("audit");

            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

            #region enable-installers
            endpointConfiguration.EnableInstallers();
            #endregion

            configureEndpoint?.Invoke(endpointConfiguration, routing);

            return builder.UseNServiceBus(endpointConfiguration);
        }
    }
}