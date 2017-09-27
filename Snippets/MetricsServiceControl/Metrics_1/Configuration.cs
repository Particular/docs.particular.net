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
#pragma warning disable 618
            metrics.SendMetricDataToServiceControl("SERVICE_CONTROL_METRICS_ADDRESS", TimeSpan.FromSeconds(5));
#pragma warning restore 618

            #endregion
        }
    }
}
