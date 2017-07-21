namespace Metrics_1_1
{
    using NServiceBus;

    public class Configuration
    {
        public void ConfigureEndpoint(EndpointConfiguration endpointConfiguration)
        {
            var metricsOptions = endpointConfiguration.EnableMetrics();

            #region Metrics-Observers

            metricsOptions.RegisterObservers(ctx => { });

            #endregion
        }
    }
}