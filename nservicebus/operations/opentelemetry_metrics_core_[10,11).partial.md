## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the appropriate meter source (e.g. `NServiceBus.Core.Pipeline.Incoming`) to the OpenTelemetry configuration:

> [!NOTE]
> The metric definitions published by NServiceBus are not yet finalized and could change in a minor release.

snippet: opentelemetry-enablemeters

Because all metrics from a meter source are emitted by default, use an OpenTelemetry view to drop any that are not needed:

snippet: opentelemetry-metrics-filter-view

### execution.result tag

Several metrics carry an `execution.result` tag with a value of `"success"` or `"failure"`. The tag increases metric cardinality and the associated ingestion cost.

In version 10 the tag is emitted on `nservicebus.messaging.successes`, `nservicebus.messaging.failures`, `nservicebus.messaging.processing_time`, `nservicebus.messaging.critical_time`, `nservicebus.messaging.handler_time`, `nservicebus.messaging.deserialize_time`, `nservicebus.messaging.serialize_time`, and `nservicebus.sagas.fetch_time`.

The tag is not emitted when the [version 11 behavior](/nservicebus/operations/opentelemetry.md) is enabled, and it is removed in version 11. There is no option to keep it. The outcome stays available without the tag: `nservicebus.messaging.successes` and `nservicebus.messaging.failures` are separate instruments, and the histograms carry `error.type` when the operation fails.

To drop the tag on version 10, use an OpenTelemetry view.

### Meter name

NServiceBus emits its core metrics from the `NServiceBus.Core.Pipeline.Incoming` meter, version `0.4.0`. When the [version 11 behavior](/nservicebus/operations/opentelemetry.md#traces-version-11-behavior-opt-in) is enabled, the same instruments are emitted from the `NServiceBus.Core` meter, version `1.0.0`. The instrument names and tags are the same under both meter names, except for the `execution.result` tag described above.

Only one of the two meters exists at a time. With the version 11 behavior enabled, subscribe to the new name:

snippet: opentelemetry-enablemeters-v11

> [!WARNING]
> A `MeterProvider` that subscribes to the name that does not match the switch collects no NServiceBus core metrics, and reports no error. Change the `AddMeter` call in the same deployment that enables the switch. Dashboards, recording rules, and alerts that filter on the instrumentation scope, such as the `otel_scope_name` label in Prometheus, must be updated as well.

A wildcard subscription such as `AddMeter("NServiceBus.Core*")` matches both names and needs no change.

### Emitted meters

Meter source `NServiceBus.Core.Pipeline.Incoming`, or `NServiceBus.Core` when the version 11 behavior is enabled:

- [`nservicebus.messaging.successes`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-successfully-processed) - Total number of messages processed successfully by the endpoint
- [`nservicebus.messaging.fetches`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-pulled-from-queue) - Total number of messages fetched from the queue by the endpoint
- [`nservicebus.messaging.failures`](/monitoring/metrics/definitions.md#metrics-captured-number-of-message-processing-failures) - Total number of messages processed unsuccessfully by the endpoint
- [`nservicebus.messaging.handler_time`](/monitoring/metrics/definitions.md#metrics-captured-handler-time) - The time the user handling code takes to handle a message
- [`nservicebus.messaging.processing_time`](/monitoring/metrics/definitions.md#metrics-captured-processing-time) - The time the endpoint takes to process a message
- [`nservicebus.messaging.critical_time`](/monitoring/metrics/definitions.md#metrics-captured-critical-time) - The time between when a message is sent and when it is fully processed
- `nservicebus.messaging.active_messages` (UpDownCounter) - Number of messages currently being processed by the endpoint. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`
- `nservicebus.messaging.deserialize_time` - The time in seconds for deserializing an incoming message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`, `execution.result`, `error.type`
- `nservicebus.messaging.serialize_time` - The time in seconds for serializing an outgoing message. Tags: `nservicebus.message_type`, `execution.result`, `error.type`
- [`nservicebus.recoverability.immediate`](/monitoring/metrics/definitions.md#metrics-captured-immediate-retries) - Total number of immediate retries requested
- [`nservicebus.recoverability.delayed`](/monitoring/metrics/definitions.md#metrics-captured-delayed-retries) - Total number of delayed retries requested
- [`nservicebus.recoverability.error`](/monitoring/metrics/definitions.md#metrics-captured-moved-to-error-queue) - Total number of messages sent to the error queue
- `nservicebus.sagas.fetch_time` - The time in seconds for loading saga data from the persister. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.saga_type`, `execution.result`, `error.type`
- `nservicebus.outbox.duplicates` - Total number of messages deduplicated by the outbox. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`
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
