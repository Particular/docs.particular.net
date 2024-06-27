## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the `NServiceBus.Core` meter source to the OpenTelemetry configuration:

snippet: opentelemetry-enablemeters

### Emitted meters

- `nservicebus.messaging.successes` - Total number of messages processed successfully by the endpoint
- `nservicebus.messaging.fetches` - Total number of messages fetched from the queue by the endpoint
- `nservicebus.messaging.failures` - Total number of messages processed unsuccessfully by the endpoint
- `nservicebus.messaging.handler_time` - The time the user handling code takes to handle a message
- `nservicebus.messaging.processing_time` - The time the endpoint takes to process a message from when it's fetched from the input queue to when processing completes. It includes:
  - Invoking all handlers and sagas for a single incoming message
  - Invoking the incoming message processing pipeline, which includes steps like deserialization or user defined pipeline behaviors.
- `nservicebus.messaging.critical_time` - The time between when a message is sent and when it is fully processed. It is a combination of:
  - Network send time: The time a message spends on the network before arriving in the destination queue
  - Queue wait time: The time a message spends in the destination queue before being picked up and processed
  - Processing time: The time it takes for the destination endpoint to process the message

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.