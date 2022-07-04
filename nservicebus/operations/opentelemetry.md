---
title: OpenTelemetry
summary: Observability of NServiceBus endpoints with OpenTelemetry
component: core
reviewed: 2022-06-29
related:
 - samples/open-telemetry
---

NServiceBus version 8 and above supports [OpenTelemetry](https://opentelemetry.io/docs/instrumentation/net/) through traces, metrics, and logging.

## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

```csharp
using var traceProvider = Sdk.CreateTracerProviderBuilder()
    // Add all NServiceBus sources
    .AddSource("NServiceBus.*")
    // Add an exporter
    .AddConsoleExporter()
    .Build();
```

See [samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.

## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

```csharp
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    // All all NServiceBus meters
    .AddMeter("NServiceBus.*")
    // Add an exporter
    .AddConsoleExporter()
    .Build();
```

Once the source has been added, enable OpenTelemetry metrics on the NServiceBus endpoint configuration:

```csharp
endpointConfiguration.EnableOpenTelemetryMetrics();
```

## Logging

NServiceBus already supports logging out of the box. To collect OpenTelemetry-compatible logging in NServiceBus endpoints, it's possible to configure the endpoint to connect traces and logging when using Microsoft.Extensions.Logging.

TODO: Link to sample in the other PR.
See [samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.