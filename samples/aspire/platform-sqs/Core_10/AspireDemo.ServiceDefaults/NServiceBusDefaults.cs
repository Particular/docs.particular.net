using Microsoft.Extensions.Configuration;

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
            var resourceNamePrefix = builder.Configuration["RESOURCE_NAME_PREFIX"] ?? "";

            var routing = endpointConfiguration.UseTransport(new SqsTransport()
            {
                QueueNamePrefix = resourceNamePrefix,
                TopicNamePrefix = resourceNamePrefix,
            });
            #endregion

            endpointConfiguration.UseSerialization<SystemJsonSerializer>();
            endpointConfiguration.SendHeartbeatTo($"{resourceNamePrefix}Particular.ServiceControl");
            endpointConfiguration.AuditProcessedMessagesTo($"{resourceNamePrefix}audit");

            var metrics = endpointConfiguration.EnableMetrics();
            metrics.SendMetricDataToServiceControl($"{resourceNamePrefix}Particular.Monitoring", TimeSpan.FromSeconds(1));

            #region enable-installers
            endpointConfiguration.EnableInstallers();
            #endregion

            configureEndpoint?.Invoke(endpointConfiguration, routing);

            builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

            return builder;
        }
    }
}

public static class NServiceBusExtensions
{
    public static TBuilder AddNServiceBusEndpoint<TBuilder>(this TBuilder builder, string endpointName, Action<EndpointConfiguration, RoutingSettings>? configure = null)
        where TBuilder : IHostApplicationBuilder
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        #region transport-config
        var resourceNamePrefix = builder.Configuration["RESOURCE_NAME_PREFIX"] ?? "";

        var routing = endpointConfiguration.UseTransport(new SqsTransport()
        {
            QueueNamePrefix = resourceNamePrefix,
            TopicNamePrefix = resourceNamePrefix,
        });
        #endregion

        configure?.Invoke(endpointConfiguration, routing);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.SendHeartbeatTo($"{resourceNamePrefix}Particular.ServiceControl");
        endpointConfiguration.AuditProcessedMessagesTo($"{resourceNamePrefix}audit");

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.SendMetricDataToServiceControl($"{resourceNamePrefix}Particular.Monitoring", TimeSpan.FromSeconds(1));

        #region enable-installers
        endpointConfiguration.EnableInstallers();
        #endregion

        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

        return builder;
    }
}