namespace Core.OpenTelemetry;

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