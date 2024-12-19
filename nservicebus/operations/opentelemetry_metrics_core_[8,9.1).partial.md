## Metrics

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

> [!NOTE]
> The metric definitions published by NServiceBus are not yet finalized and could change in a minor release.

snippet: opentelemetry-enablemeters

### Emitted meters

- `nservicebus.messaging.successes` - Total number of messages processed successfully by the endpoint
- `nservicebus.messaging.fetches` - Total number of messages fetched from the queue by the endpoint
- `nservicebus.messaging.failures` - Total number of messages processed unsuccessfully by the endpoint

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.

### Additional metrics

The NServiceBus.Metrics package provides additional metrics that are not yet exposed via OpenTelemetry.

These metrics include:

- Critical time
- Processing time
- Handler time
- Retries

To expose these metrics via OpenTelemetry, a shim can be used. The [OpenTelemetry shim sample](/samples/open-telemetry/metrics-shim/) demonstrates how to set up a shim that exposes NServiceBus.Metrics in an OpenTelemetry format and exports them to Azure Application Insights.