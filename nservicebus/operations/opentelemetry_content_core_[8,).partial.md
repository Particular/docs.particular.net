
Enable OpenTelemetry instrumentation in NServiceBus:

snippet: opentelemetry-enableinstrumentation

With OpenTelemetry instrumentation enabled, tracing, metrics, and logging can be individually configured via the OpenTelemetry API itself.

## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

snippet: opentelemetry-enabletracing

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.

## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

snippet: opentelemetry-enablemeters

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.

## Logging

NServiceBus supports logging out of the box. To collect OpenTelemetry-compatible logging in NServiceBus endpoints, it's possible to configure the endpoint to connect traces and logging when using `Microsoft.Extensions.Logging` package. See the [_Connecting OpenTelemetry traces and logs_ sample](/samples/open-telemetry/logging) for more details.
