namespace Core9
{
    using NServiceBus;

    class Configuration
    {
        void EnableOpenTelemetry(EndpointConfiguration endpointConfiguration)
        {
            #region opentelemetry-enableinstrumentation

            endpointConfiguration.EnableOpenTelemetry();

            #endregion
        }
    }
}