namespace Metrics_1
{
    using System;
    using NServiceBus;


    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region SendMetricDataToServiceControl

            var metrics = endpointConfiguration.EnableMetrics();
#pragma warning disable CS0618 // Type or member is obsolete
            metrics.SendMetricDataToServiceControl("SERVICE_CONTROL_METRICS_ADDRESS", TimeSpan.FromSeconds(5), "INSTANCE_ID_OPTIONAL");
#pragma warning restore CS0618 // Type or member is obsolete

            #endregion
        }
    }
}
