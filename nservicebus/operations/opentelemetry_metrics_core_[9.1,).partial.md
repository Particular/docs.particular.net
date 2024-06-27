## Metrics

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

snippet: opentelemetry-enablemeters

### Emitted meters

- `nservicebus.messaging.successes` - Total number of messages processed successfully by the endpoint
- `nservicebus.messaging.fetches` - Total number of messages fetched from the queue by the endpoint
- `nservicebus.messaging.failures` - Total number of messages processed unsuccessfully by the endpoint

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.