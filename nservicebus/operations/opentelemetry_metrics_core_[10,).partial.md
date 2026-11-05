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

The tag is not emitted when the [version 11 behavior](/nservicebus/operations/opentelemetry.md) is enabled, and it is removed in version 11. There is no option to keep it. The outcome stays available without the tag: the histograms carry `error.type` when the operation fails, and message processing outcomes are recorded by `nservicebus.messaging.successes` and `nservicebus.messaging.failures`, or by `nservicebus.messaging.processing_attempts` with the version 11 behavior.

To drop the tag on version 10, use an OpenTelemetry view.

### Meter name and version

#### Default behavior

NServiceBus emits its core metrics from the `NServiceBus.Core.Pipeline.Incoming` meter, version `0.4.0`.

#### Version 11 behavior

When the [version 11 behavior](/nservicebus/operations/opentelemetry.md#traces-version-11-behavior-opt-in) is enabled, the same instruments are emitted from the `NServiceBus.Core` meter, version `1.0.0`, and the `NServiceBus.Core.Pipeline.Incoming` meter is not emitted. The instruments change as well:

- `nservicebus.messaging.successes`, `nservicebus.messaging.failures`, and `nservicebus.messaging.fetches` are replaced by `nservicebus.messaging.processing_attempts`, which carries `error.type` on failed attempts.
- `nservicebus.envelope.unwrapped` is renamed to `nservicebus.envelope.unwrap_attempts` and counts every envelope handler invocation.
- Counters declare a unit, such as `{attempt}`, `{retry}`, or `{message}`.
- Duration histograms advise bucket boundaries in seconds.
- The `execution.result` tag described above is not emitted.

See the [upgrade guide](/nservicebus/upgrades/10to11/#opentelemetry-metrics) for details.

The wildcard subscription shown above matches both meter names, so it keeps working whether or not the switch is enabled.

> [!WARNING]
> A `MeterProvider` that subscribes to `NServiceBus.Core.Pipeline.Incoming` by name collects no NServiceBus core metrics once the switch is enabled, and reports no error. Change the `AddMeter` call in the same deployment that enables the switch.

Dashboards, recording rules, and alerts that filter on the instrumentation scope, such as the `otel_scope_name` and `otel_scope_version` labels in Prometheus, must be updated as well.
#end-if

### Emitted meters

#if-version [10,11)
Meter source `NServiceBus.Core.Pipeline.Incoming`, version `0.4.0`, or `NServiceBus.Core`, version `1.0.0`, when the version 11 behavior is enabled. With the version 11 behavior, the instruments change as described in [meter name and version](#meters-meter-name-and-version-version-11-behavior).

- [`nservicebus.messaging.successes`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-successfully-processed) - Total number of messages processed successfully by the endpoint
- [`nservicebus.messaging.fetches`](/monitoring/metrics/definitions.md#metrics-captured-number-of-messages-pulled-from-queue) - Total number of messages fetched from the queue by the endpoint
- [`nservicebus.messaging.failures`](/monitoring/metrics/definitions.md#metrics-captured-number-of-message-processing-failures) - Total number of messages processed unsuccessfully by the endpoint
- [`nservicebus.messaging.handler_time`](/monitoring/metrics/definitions.md#metrics-captured-handler-time) - The time the user handling code takes to handle a message
- [`nservicebus.messaging.processing_time`](/monitoring/metrics/definitions.md#metrics-captured-processing-time) - The time the endpoint takes to process a message
- [`nservicebus.messaging.critical_time`](/monitoring/metrics/definitions.md#metrics-captured-critical-time) - The time between when a message is sent and when it is fully processed
- [`nservicebus.messaging.active_messages`](/monitoring/metrics/definitions.md#metrics-captured-active-messages) (UpDownCounter) - Number of messages currently being processed by the endpoint. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`
- [`nservicebus.messaging.deserialize_time`](/monitoring/metrics/definitions.md#metrics-captured-deserialization-time) - The time in seconds for deserializing an incoming message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`, `error.type`
- [`nservicebus.messaging.serialize_time`](/monitoring/metrics/definitions.md#metrics-captured-serialization-time) - The time in seconds for serializing an outgoing message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `error.type`
- [`nservicebus.recoverability.immediate`](/monitoring/metrics/definitions.md#metrics-captured-immediate-retries) - Total number of immediate retries requested
- [`nservicebus.recoverability.delayed`](/monitoring/metrics/definitions.md#metrics-captured-delayed-retries) - Total number of delayed retries requested
- [`nservicebus.recoverability.error`](/monitoring/metrics/definitions.md#metrics-captured-moved-to-error-queue) - Total number of messages sent to the error queue
- [`nservicebus.sagas.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-saga-fetch-time) - The time in seconds for loading saga data from the persister. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.saga_type`, `error.type`
- [`nservicebus.outbox.duplicates`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - Total number of messages deduplicated by the outbox. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`
- [`nservicebus.outbox.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - The time in seconds for querying the outbox storage for deduplication. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.outbox.store_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) - The time in seconds for storing a message in the outbox storage. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.persistence.commit_time`](/monitoring/metrics/definitions.md#metrics-captured-storage-session-commit-time) - The time in seconds for completing the synchronized storage session. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`
- [`nservicebus.envelope.unwrapped`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics) - Total number of times when an envelope handler failed to unwrap an incoming message. Emitted for every unwrapping attempt, starting with version 10.1
#end-if
#if-version [11,)
Meter source `NServiceBus.Core`, version `1.0.0`:

- `nservicebus.messaging.processing_attempts` (Counter, `{attempt}`) - Number of message processing attempts. Failed attempts carry `error.type`. Messages deduplicated by the [outbox](/nservicebus/outbox/) count as successful attempts. Attempts canceled because the endpoint is stopping carry `error.type` `System.OperationCanceledException`. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`, `error.type`
- [`nservicebus.messaging.active_messages`](/monitoring/metrics/definitions.md#metrics-captured-active-messages) (UpDownCounter, `{message}`) - Number of messages currently being processed by the endpoint. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`
- [`nservicebus.messaging.critical_time`](/monitoring/metrics/definitions.md#metrics-captured-critical-time) (Histogram, `s`) - The time between when a message is sent, or scheduled to be delivered, and when it is fully processed. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`
- [`nservicebus.messaging.processing_time`](/monitoring/metrics/definitions.md#metrics-captured-processing-time) (Histogram, `s`) - The time between when a message is fetched from the input queue and when it is successfully processed. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`
- [`nservicebus.messaging.handler_time`](/monitoring/metrics/definitions.md#metrics-captured-handler-time) (Histogram, `s`) - The time a message handler takes to handle a message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_type`, `error.type`
- [`nservicebus.messaging.deserialize_time`](/monitoring/metrics/definitions.md#metrics-captured-deserialization-time) (Histogram, `s`) - The time for deserializing an incoming message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.enclosed_message_types`, `error.type`
- [`nservicebus.messaging.serialize_time`](/monitoring/metrics/definitions.md#metrics-captured-serialization-time) (Histogram, `s`) - The time for serializing an outgoing message. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `error.type`
- [`nservicebus.recoverability.immediate`](/monitoring/metrics/definitions.md#metrics-captured-immediate-retries) (Counter, `{retry}`) - Number of immediate retries requested. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_type`, `error.type`
- [`nservicebus.recoverability.delayed`](/monitoring/metrics/definitions.md#metrics-captured-delayed-retries) (Counter, `{retry}`) - Number of delayed retries requested. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_type`, `error.type`
- [`nservicebus.recoverability.error`](/monitoring/metrics/definitions.md#metrics-captured-moved-to-error-queue) (Counter, `{message}`) - Number of messages moved to the error queue. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_type`, `error.type`
- [`nservicebus.sagas.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-saga-fetch-time) (Histogram, `s`) - The time for loading saga data from the persister. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.saga_type`, `error.type`
- [`nservicebus.outbox.duplicates`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) (Counter, `{message}`) - Number of duplicate messages detected by the outbox. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`
- [`nservicebus.outbox.fetch_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) (Histogram, `s`) - The time for querying the outbox storage for deduplication. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.outbox.store_time`](/monitoring/metrics/definitions.md#metrics-captured-outbox-metrics) (Histogram, `s`) - The time for storing a message in the outbox storage. Tags: `nservicebus.queue`, `nservicebus.discriminator`
- [`nservicebus.persistence.commit_time`](/monitoring/metrics/definitions.md#metrics-captured-storage-session-commit-time) (Histogram, `s`) - The time for completing the synchronized storage session. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.message_type`, `nservicebus.message_handler_types`
- `nservicebus.envelope.unwrap_attempts` (Counter, `{attempt}`) - Number of envelope handler invocations. Failed invocations carry `error.type`. Tags: `nservicebus.queue`, `nservicebus.discriminator`, `nservicebus.envelope.unwrapper_type`, `error.type`
#end-if

Meter source `NServiceBus.TransactionalSession`:

- [`nservicebus.transactional_session.commit.duration`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time the endpoint takes to commit the session in the Transactional Session
- [`nservicebus.transactional_session.dispatch.duration`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time the endpoint takes to dispatch the control message in the Transactional Session
- [`nservicebus.transactional_session.control_message.attempts`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - Total number of attempts to process the control message in the Transactional Session
- [`nservicebus.transactional_session.control_message.transit_time`](/monitoring/metrics/definitions.md#metrics-captured-transactional-session-metrics) - The time between dispatching the control message and starting to process it in the Transactional Session

Meter source `NServiceBus.Envelope.CloudEvents`:

- [`nservicebus.envelope.cloud_events.received.unwrapping_attempt`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of unwrapping attempts
- [`nservicebus.envelope.cloud_events.received.invalid_message`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of received messages not conforming to the specification
- [`nservicebus.envelope.cloud_events.received.unexpected_version`](/monitoring/metrics/definitions.md#metrics-captured-envelope-handling-metrics-cloudevents-specific-metrics) - Total number of received messages with unexpected version field value

### Exporting metrics

How a backend stores metrics affects which data survives the export. The following sections describe settings that avoid losing data and limit cost.

#### Temporality

The OpenTelemetry .NET SDK exports metrics with cumulative temporality by default: every export reports a running total, and the backend derives increments from consecutive points. The first point of a new series has no predecessor, so some backends treat it as a baseline and discard it. For example, the first failure with a new `error.type` value is lost with the Amazon CloudWatch EMF exporter and with Datadog in cumulative mode, and the Prometheus `increase()` function undercounts it.

With delta temporality, every point reports only what happened during the export interval, so no point needs a baseline and nothing is discarded.

- **Amazon CloudWatch (EMF exporter) and Datadog:** use delta temporality. Datadog recommends it, and the CloudWatch EMF exporter does not convert cumulative histograms, so their count and sum would be reported as totals since the process started.
- **Azure Monitor:** the Azure Monitor exporter always uses delta temporality. No configuration is needed.
- **Prometheus:** keep cumulative temporality. Prometheus is built around cumulative counters.

Delta temporality is configured on the exporter, for example by setting the `OTEL_EXPORTER_OTLP_METRICS_TEMPORALITY_PREFERENCE` environment variable to `delta`, or in code:

```csharp
Sdk.CreateMeterProviderBuilder()
    .AddMeter("NServiceBus.*")
    .AddOtlpExporter((exporterOptions, metricReaderOptions) =>
        metricReaderOptions.TemporalityPreference = MetricReaderTemporalityPreference.Delta)
    .Build();
```

#### Histogram buckets

#if-version [10,11)
By default, the duration histograms do not advise bucket boundaries, so the OpenTelemetry .NET SDK applies its default boundaries (0, 5, 10, 25, … 10,000), which are intended for milliseconds, and nearly every value falls into the first bucket. Configure a view with boundaries in seconds, as shown below. With the version 11 behavior, the duration histograms advise boundaries from the OpenTelemetry semantic conventions: 5 milliseconds to 10 seconds for serialization, outbox, saga, and persistence calls, and 10 milliseconds to 300 seconds for handler, processing, and critical time.
#end-if
#if-version [11,)
The duration histograms advise explicit bucket boundaries from the OpenTelemetry semantic conventions. Serialization, outbox, saga, and persistence calls use boundaries from 5 milliseconds to 10 seconds. Handler, processing, and critical time use boundaries from 10 milliseconds to 300 seconds.
#end-if

Bucket boundaries only matter on backends that keep the buckets, such as Prometheus-compatible backends and Datadog, where they determine the accuracy of percentiles. The Azure Monitor exporter and the Amazon CloudWatch EMF exporter reduce a histogram to its count, sum, minimum, and maximum.

Values above the highest boundary are counted in an overflow bucket. Datadog reports them at the highest boundary, so with the advised boundaries a critical time above 300 seconds appears as 300 seconds in percentiles. To use different boundaries, configure a view:

```csharp
Sdk.CreateMeterProviderBuilder()
    .AddMeter("NServiceBus.*")
    .AddView("nservicebus.messaging.critical_time", new ExplicitBucketHistogramConfiguration
    {
        Boundaries = [0.01, 0.1, 1, 10, 60, 300, 900, 3600]
    })
    .Build();
```

Views and the base2 exponential histogram aggregation take precedence over advised boundaries.

#### Tag cardinality

Every unique combination of tag values creates a separate time series, and most backends bill or limit the number of series. Datadog bills each combination as a custom metric, and a histogram counts as five. Amazon CloudWatch creates a billed metric for each dimension combination. Azure Monitor limits the number of dimension values when dimensions are enabled for custom metrics.

The tags `nservicebus.message_type`, `nservicebus.message_handler_types`, `nservicebus.message_handler_type`, `nservicebus.saga_type`, and `nservicebus.enclosed_message_types` have one value per message, handler, or saga type. On endpoints that handle many message types, they create many series. To keep only some tags, configure a view:

```csharp
Sdk.CreateMeterProviderBuilder()
    .AddMeter("NServiceBus.*")
    .AddView("nservicebus.recoverability.error", new MetricStreamConfiguration
    {
        TagKeys = ["nservicebus.queue", "nservicebus.discriminator", "error.type"]
    })
    .Build();
```

#### Error rates

#if-version [10,11)
`nservicebus.messaging.failures` does not count attempts canceled because the endpoint is stopping. With the version 11 behavior, `nservicebus.messaging.processing_attempts` records them with `error.type` set to `System.OperationCanceledException`.
#end-if
#if-version [11,)
`nservicebus.messaging.processing_attempts` records attempts canceled because the endpoint is stopping with `error.type` set to `System.OperationCanceledException`.
#end-if

Every deployment or restart therefore records some canceled attempts. Exclude this value from error-rate queries and alerts that should report processing failures only. For example, in PromQL with the default name translation:

```text
sum by (nservicebus_queue) (rate(nservicebus_messaging_processing_attempts_total{error_type!="", error_type!="System.OperationCanceledException"}[5m]))
/
sum by (nservicebus_queue) (rate(nservicebus_messaging_processing_attempts_total{error_type!="System.OperationCanceledException"}[5m]))
```

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send metric information to different tools.
