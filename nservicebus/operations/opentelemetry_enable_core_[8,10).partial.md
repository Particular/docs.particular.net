Enable OpenTelemetry instrumentation in NServiceBus:

snippet: opentelemetry-enableinstrumentation

With OpenTelemetry instrumentation enabled, tracing, metrics, and logging can be individually configured via the OpenTelemetry API itself.

## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

snippet: opentelemetry-enabletracing