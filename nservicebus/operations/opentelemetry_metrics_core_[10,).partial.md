## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the appropriate meter source (e.g. `NServiceBus.Core.Pipeline.Incoming`) to the OpenTelemetry configuration:

> [!NOTE]
> The metric definitions published by NServiceBus are not yet finalized and could change in a minor release.

snippet: opentelemetry-enablemeters

Because all metrics from a meter source are emitted by default, use an OpenTelemetry view to drop any that are not needed:

snippet: opentelemetry-metrics-filter-view

### Emitted meters

Meter source `NServiceBus.Core.Pipeline.Incoming`:

- [`nservicebus.messaging.successes`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-successfully-processed) - Total number of messages processed successfully by the endpoint
- [`nservicebus.messaging.fetches`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-pulled-from-queue) - Total number of messages fetched from the queue by the endpoint
- [`nservicebus.messaging.failures`](/monitoring/metrics/definitions.md#metrics-captured-number-of-message-processing-failures) - Total number of messages processed unsuccessfully by the endpoint
- [`nservicebus.messaging.handler_time`](/monitoring/metrics/definitions.md#metrics-captured-handler-time) - The time the user handling code takes to handle a message
- [`nservicebus.messaging.processing_time`](/monitoring/metrics/definitions.md#metrics-captured-processing-time) - The time the endpoint takes to process a message
- [`nservicebus.messaging.critical_time`](/monitoring/metrics/definitions.md#metrics-captured-critical-time) - The time between when a message is sent and when it is fully processed
- `nservicebus.messaging.deserialize_time` - The time in seconds for deserializing an incoming message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `execution.result`, `error.type`
- `nservicebus.messaging.serialize_time` - The time in seconds for serializing an outgoing message. Tags: `nservicebus.message_type`, `execution.result`, `error.type`
- [`nservicebus.recoverability.immediate`](/monitoring/metrics/definitions.md#metrics-captured-immediate-retries) - Total number of immediate retries requested
- [`nservicebus.recoverability.delayed`](/monitoring/metrics/definitions.md#metrics-captured-delayed-retries) - Total number of delayed retries requested
- [`nservicebus.recoverability.error`](/monitoring/metrics/definitions.md#metrics-captured-moved-to-error-queue) - Total number of messages sent to the error queue
- `nservicebus.sagas.fetch_time` - The time in seconds for loading saga data from the persister. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.saga_type`, `execution.result`, `error.type`
- `nservicebus.outbox.fetch_time` - The time in seconds for querying the outbox storage for deduplication. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- `nservicebus.outbox.store_time` - The time in seconds for storing a message in the outbox storage. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- `nservicebus.persistence.commit_time` - The time in seconds for completing the synchronized storage session. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`

Starting NServiceBus V10.1 envelope unwrapping metrics are emitted as part of the `NServiceBus.Core.Pipeline.Incoming` source:

- [`nservicebus.envelope.unwrapped`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics) - Total number of times when an envelope handler failed to unwrap an incoming message. Emitted for every unwrapping attempt

Meter source `NServiceBus.TransactionalSession`:

- [`nservicebus.transactional_session.commit.duration`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time the endpoint takes to commit the session in the Transactional Session
- [`nservicebus.transactional_session.dispatch.duration`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time the endpoint takes to dispatch the control message in the Transactional Session
- [`nservicebus.transactional_session.control_message.attempts`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - Total number of attempts to process the control message in the Transactional Session
- [`nservicebus.transactional_session.control_message.transit_time`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time between dispatching the control message and starting to process it in the Transactional Session

Meter source `NServiceBus.Envelope.CloudEvents`:

- [`nservicebus.envelope.cloud_events.received.unwrapping_attempt`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of unwrapping attempts
- [`nservicebus.envelope.cloud_events.received.invalid_message`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of received messages not conforming to the specification
- [`nservicebus.envelope.cloud_events.received.unexpected_version`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of received messages with unexpected version field value


See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.
