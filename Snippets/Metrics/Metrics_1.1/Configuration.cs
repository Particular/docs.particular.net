namespace Metrics_1_1
{
    using NServiceBus;

    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            #region Metrics-Enable

            var metricsOptions = endpointConfiguration.EnableMetrics();

            #endregion

            #region Metrics-Observers

            metricsOptions.RegisterObservers(ctx => { });

            #endregion
        }
    }
}