namespace Core.OpenTelemetry;

using NServiceBus;

public static class ExceptionRecording
{
    public static void ConfigureLogsOnly(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-exception-recording-logs

        var options = endpointConfiguration.Tracing();
        options.ExceptionRecordingMode = ExceptionRecordingMode.Logs;

        #endregion
    }
}
