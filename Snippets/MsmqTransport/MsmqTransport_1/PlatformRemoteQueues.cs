using System;
using System.Threading.Tasks;
using NServiceBus;

class PlatformRemoteQueues
{
    async Task Snippets(EndpointConfiguration endpointConfiguration, IEndpointInstance endpoint)
    {
        #region ConfigureErrorRemoteQueue

        endpointConfiguration.SendFailedMessagesTo("Error_Queue@machinename");

        #endregion

        #region ConfigureAuditRemoteQueue

        endpointConfiguration.AuditProcessedMessagesTo("Audit_Queue@machinename");

        #endregion

        #region ConfigureMetricsRemoteQueue

        var metrics = endpointConfiguration.EnableMetrics();
        metrics.SendMetricDataToServiceControl(
            serviceControlMetricsAddress: "ServiceControl_Metrics_Queue@machinename",
            interval: TimeSpan.FromMinutes(1),
            instanceId: "INSTANCE_ID_OPTIONAL");

        #endregion

        #region ConfigureHeartbeatsRemoteQueue

        endpointConfiguration.SendHeartbeatTo(
            serviceControlQueue: "ServiceControl_Queue@machinename",
            frequency: TimeSpan.FromSeconds(15),
            timeToLive: TimeSpan.FromSeconds(30));

        #endregion

        #region ConfigureCustomChecksRemoteQueue

        endpointConfiguration.ReportCustomChecksTo(
            serviceControlQueue: "ServiceControl_Queue@machinename",
            timeToLive: TimeSpan.FromSeconds(10));

        #endregion

        #region OverridingPhysicalRouting

        var options = new SendOptions();
        options.SetDestination("MyDestination@machinename");
        options.RouteReplyTo("MyDestination@machinename");
        await endpoint.Send(new MyMessage(), options);

        #endregion
    }

    class MyMessage : ICommand { }
}