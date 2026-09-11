namespace Core.OpenTelemetry;

using NServiceBus;

public static class MetersConfiguration
{
    public static void DisableExecutionResultTags(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-meters-disable-execution-result-tags

        var options = endpointConfiguration.Tracing();
        options.Meters.EmitExecutionResultTags = false;

        #endregion
    }
}
