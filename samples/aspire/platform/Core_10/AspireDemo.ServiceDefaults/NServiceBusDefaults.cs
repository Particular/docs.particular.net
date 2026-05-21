namespace Microsoft.Extensions.Hosting;

public static class NServiceBusExtensions
{
    public static TBuilder AddNServiceBusEndpoint<TBuilder>(this TBuilder builder, string endpointName, Action<EndpointConfiguration, RoutingSettings>? configure = null)
        where TBuilder : IHostApplicationBuilder
    {
        var endpointConfiguration = new EndpointConfiguration(endpointName);

        #region transport-config
        var routing = endpointConfiguration.UseTransport(new LearningTransport());
        #endregion

        configure?.Invoke(endpointConfiguration, routing);

        endpointConfiguration.UseSerialization<SystemJsonSerializer>();
        endpointConfiguration.SendHeartbeatTo("Particular.ServiceControl");
        endpointConfiguration.AuditProcessedMessagesTo("audit");

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.SendMetricDataToServiceControl("Particular.Monitoring", TimeSpan.FromSeconds(1));

        #region enable-installers
        endpointConfiguration.EnableInstallers();
        #endregion

        builder.UseNServiceBus(endpointConfiguration);

        return builder;
    }
}