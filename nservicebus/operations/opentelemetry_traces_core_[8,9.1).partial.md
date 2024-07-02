
Enable OpenTelemetry instrumentation in NServiceBus:

snippet: opentelemetry-enableinstrumentation

With OpenTelemetry instrumentation enabled, tracing, metrics, and logging can be individually configured via the OpenTelemetry API itself.

## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

snippet: opentelemetry-enabletracing

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.
