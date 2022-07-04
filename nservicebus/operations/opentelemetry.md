---
title: OpenTelemetry
summary: Observing NServiceBus endpoints via OpenTelemetry
component: core
reviewed: 2022-06-29
related:
 - samples/open-telemetry
---

NServiceBus version 8 and above provides diagnostic trace and meter information via [OpenTelemetry](https://opentelemetry.io/docs/instrumentation/net/).

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

NServiceBus endpoints can be configured to expose OpenTelemetry meters related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

```csharp
using var meterProvider = Sdk.CreateMeterProviderBuilder()
    // All all NServiceBus meters
    .AddMeter("NServiceBus.*")
    // Add an exporter
    .AddConsoleExporter()
    .Build();
```

Once the source has been added, enable OpenTelemetry meterics on the NServiceBus endpoint configuration:

```csharp
endpointConfiguration.EnableOpenTelemetryMetrics();
```

See [samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.