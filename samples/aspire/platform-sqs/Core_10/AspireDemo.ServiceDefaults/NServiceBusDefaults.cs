namespace Microsoft.Extensions.Hosting;

public static class NServiceBusExtensions
{
    public static TBuilder AddNServiceBusEndpoint<TBuilder>(this TBuilder builder, string endpointName, Action<EndpointConfiguration, RoutingSettings>? configure = null)
        where TBuilder : IHostApplicationBuilder
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        #region transport-config
        var queuePrefix = builder.Configuration["QUEUE_NAME_PREFIX"] ?? "";

        var routing = endpointConfiguration.UseTransport(new SqsTransport()
        {
            QueueNamePrefix = queuePrefix,
            TopicNamePrefix = queuePrefix,
        });
        #endregion

        configure?.Invoke(endpointConfiguration, routing);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.SendHeartbeatTo($"{queuePrefix}Particular.ServiceControl");
        endpointConfiguration.AuditProcessedMessagesTo($"{queuePrefix}audit");

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.SendMetricDataToServiceControl($"{queuePrefix}Particular.Monitoring", TimeSpan.FromSeconds(1));

        #region enable-installers
        endpointConfiguration.EnableInstallers();
        #endregion

        builder.Services.AddNServiceBusEndpoint(endpointConfiguration);

        return builder;
    }
}