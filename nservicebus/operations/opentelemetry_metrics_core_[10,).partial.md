## Meters

NServiceBus endpoints can be configured to expose metrics related to message processing. To capture meter information, add the NServiceBus meter sources to the OpenTelemetry configuration. A wildcard subscribes to all of them:

> [!NOTE]
> The metric definitions published by NServiceBus are not yet finalized and could change in a minor release.

snippet: opentelemetry-enablemeters

Because all metrics from a meter source are emitted by default, use an OpenTelemetry view to drop any that are not needed:

snippet: opentelemetry-metrics-filter-view

#if-version [10,11)
### execution.result tag

Several metrics carry an `execution.result` tag with a value of `"success"` or `"failure"`. The tag increases metric cardinality and the associated ingestion cost.

In version 10 the tag is emitted on `nservicebus.messaging.successes`, `nservicebus.messaging.failures`, `nservicebus.messaging.processing_time`, `nservicebus.messaging.critical_time`, `nservicebus.messaging.handler_time`, `nservicebus.messaging.deserialize_time`, `nservicebus.messaging.serialize_time`, and `nservicebus.sagas.fetch_time`.

The tag is not emitted when the [version 11 behavior](/nservicebus/operations/opentelemetry.md) is enabled, and it is removed in version 11. There is no option to keep it. The outcome stays available without the tag: `nservicebus.messaging.successes` and `nservicebus.messaging.failures` are separate instruments, and the histograms carry `error.type` when the operation fails.

To drop the tag on version 10, use an OpenTelemetry view.

### Meter name and version

#### Default behavior

NServiceBus emits its core metrics from the `NServiceBus.Core.Pipeline.Incoming` meter, version `0.4.0`.

#### Version 11 behavior

When the [version 11 behavior](/nservicebus/operations/opentelemetry.md#traces-version-11-behavior-opt-in) is enabled, the same instruments are emitted from the `NServiceBus.Core` meter, version `1.0.0`, and the `NServiceBus.Core.Pipeline.Incoming` meter is not emitted. The instrument names and tags do not change, except for the `execution.result` tag described above.

The wildcard subscription shown above matches both meter names, so it keeps working whether or not the switch is enabled.

> [!WARNING]
> A `MeterProvider` that subscribes to `NServiceBus.Core.Pipeline.Incoming` by name collects no NServiceBus core metrics once the switch is enabled, and reports no error. Change the `AddMeter` call in the same deployment that enables the switch.

Dashboards, recording rules, and alerts that filter on the instrumentation scope, such as the `otel_scope_name` and `otel_scope_version` labels in Prometheus, must be updated as well.
#end-if

### Emitted meters

#if-version [10,11)
Meter source `NServiceBus.Core.Pipeline.Incoming`, version `0.4.0`, or `NServiceBus.Core`, version `1.0.0`, when the version 11 behavior is enabled:
#end-if
#if-version [11,)
Meter source `NServiceBus.Core`, version `1.0.0`:
#end-if

- [`nservicebus.messaging.successes`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-successfully-processed) - Total number of messages processed successfully by the endpoint
- [`nservicebus.messaging.fetches`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-pulled-from-queue) - Total number of messages fetched from the queue by the endpoint
- [`nservicebus.messaging.failures`](/monitoring/metrics/definitions.md#metrics-captured-number-of-message-processing-failures) - Total number of messages processed unsuccessfully by the endpoint
- [`nservicebus.messaging.handler_time`](/monitoring/metrics/definitions.md#metrics-captured-handler-time) - The time the user handling code takes to handle a message
- [`nservicebus.messaging.processing_time`](/monitoring/metrics/definitions.md#metrics-captured-processing-time) - The time the endpoint takes to process a message
- [`nservicebus.messaging.critical_time`](/monitoring/metrics/definitions.md#metrics-captured-critical-time) - The time between when a message is sent and when it is fully processed
- [`nservicebus.messaging.active_messages`](/monitoring/metrics/definitions.md#metrics-captured-active-messages) (UpDownCounter) - Number of messages currently being processed by the endpoint. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`
- [`nservicebus.messaging.deserialize_time`](/monitoring/metrics/definitions.md#metrics-captured-deserialization-time) - The time in seconds for deserializing an incoming message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`, `error.type`
- [`nservicebus.messaging.serialize_time`](/monitoring/metrics/definitions.md#metrics-captured-serialization-time) - The time in seconds for serializing an outgoing message. Tags: `nservicebus.message_type`, `error.type`
- [`nservicebus.recoverability.immediate`](/monitoring/metrics/definitions.md#metrics-captured-immediate-retries) - Total number of immediate retries requested
- [`nservicebus.recoverability.delayed`](/monitoring/metrics/definitions.md#metrics-captured-delayed-retries) - Total number of delayed retries requested
- [`nservicebus.recoverability.error`](/monitoring/metrics/definitions.md#metrics-captured-moved-to-error-queue) - Total number of messages sent to the error queue
- [`nservicebus.sagas.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-saga-fetch-time) - The time in seconds for loading saga data from the persister. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.saga_type`, `error.type`
- [`nservicebus.outbox.duplicates`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - Total number of messages deduplicated by the outbox. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`
- [`nservicebus.outbox.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - The time in seconds for querying the outbox storage for deduplication. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.outbox.store_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - The time in seconds for storing a message in the outbox storage. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.persistence.commit_time`](/monitoring/metrics/definitions.md#metrics-captured-storage-session-commit-time) - The time in seconds for completing the synchronized storage session. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`
- [`nservicebus.envelope.unwrapped`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics) - Total number of times when an envelope handler failed to unwrap an incoming message. Emitted for every unwrapping attempt, starting with version 10.1

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
