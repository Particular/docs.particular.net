namespace Core.OpenTelemetry;

using global::OpenTelemetry;
using global::OpenTelemetry.Trace;
using NServiceBus;

public static class TraceConfiguration
{
    public static void ConfigureSendTraceMode(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-trace-mode-send

        var options = endpointConfiguration.Tracing();
        // Default: ContinueExisting - the receiver continues the sender's trace.
        // Set to StartNew to always start a new linked trace on the receiver.
        options.SendTraceMode = TraceMode.StartNew;

        #endregion
    }

    public static void ConfigurePublishTraceMode(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-trace-mode-publish

        var options = endpointConfiguration.Tracing();
        // Default: StartNew - subscribers start a new trace linked to the publish span.
        // Set to ContinueExisting to continue the publisher's trace in the subscriber.
        options.PublishTraceMode = TraceMode.ContinueExisting;

        #endregion
    }

    public static void ConfigureDelayedTraceMode(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-trace-mode-delayed

        var options = endpointConfiguration.Tracing();
        // Default for all three is StartNew (new linked trace at delivery time).
        // Set to ContinueExisting to continue the originating trace instead.
        options.DelayedDelivery.SendOperationTraceMode = TraceMode.ContinueExisting;
        options.DelayedDelivery.SagaTimeoutTraceMode = TraceMode.ContinueExisting;
        options.Recoverability.DelayedRetryTraceMode = TraceMode.ContinueExisting;

        #endregion
    }

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

    public static void EnableHandlerActivitySource()
    {
        #region opentelemetry-handler-activity-source-switch

        // Must be set before the endpoint starts.
        AppContext.SetSwitch("NServiceBus.Core.OpenTelemetry.UseHandlerActivitySource", true);

        #endregion
    }

    public static void UseDestinationInSpanNames(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-span-names-destination

        var options = endpointConfiguration.Tracing();
        options.UseMessageDestinationInSpanNames = true;

        #endregion
    }

    public static void DisableDispatchingEvents(EndpointConfiguration endpointConfiguration)
    {
        #region opentelemetry-dispatching-events-disable

        var options = endpointConfiguration.Tracing();
        options.EmitMessageDispatchingEvents = false;

        #endregion
    }

    public static void EnableDistributedContextPropagator()
    {
        #region opentelemetry-distributed-context-propagator-switch

        // Must be set before the endpoint starts.
        // Opts in to W3C DistributedContextPropagator-based trace context propagation.
        // This becomes the default in version 11.
        AppContext.SetSwitch("NServiceBus.Core.OpenTelemetry.UseDistributedContextPropagator", true);

        #endregion
    }
}
