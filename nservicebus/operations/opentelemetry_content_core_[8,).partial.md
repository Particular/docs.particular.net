## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

snippet: opentelemetry-enabletracing

WARN: The tracer provider must be built before the NServiceBus endpoint is started or trace information may not be captured.

See [samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.

## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

snippet: opentelemetry-enablemeters

WARN: The meter provider must be built before the NServiceBus endpoint is started or meter information may not be captured.

See [samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.

## Logging

NServiceBus already supports logging out of the box. To collect OpenTelemetry-compatible logging in NServiceBus endpoints, it's possible to configure the endpoint to connect traces and logging when using Microsoft.Extensions.Logging.

TODO: Link to sample in the other PR.
