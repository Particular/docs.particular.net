namespace Core.OpenTelemetry;

using System;
using global::OpenTelemetry;
using global::OpenTelemetry.Trace;

public static class TraceConfiguration
{
    public static void SubscribeToAllTraceSources()
    {
        #region opentelemetry-enabletracing-all-sources

        var tracerProvider = Sdk.CreateTracerProviderBuilder()
            .AddSource("NServiceBus.Core")
            .AddSource("NServiceBus.Core.Handler")
            .AddSource("NServiceBus.Core.Recoverability")
            // ... Add exporters
            .Build();

        #endregion
    }

    public static void EnableV11Behavior()
    {
        #region opentelemetry-v11-behavior-switch

        // Must be set before the endpoint starts. Enables every version 11 OpenTelemetry behavior.
        AppContext.SetSwitch("NServiceBus.Core.OpenTelemetry.UseV11Behavior", true);

        #endregion
    }
}
