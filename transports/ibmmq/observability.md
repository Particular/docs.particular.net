---
title: Observability
summary: Tracing IBM MQ transport operations with OpenTelemetry
reviewed: 2026-03-23
component: IBMMQ
related:
- nservicebus/operations/opentelemetry
---

The IBM MQ transport instruments send, receive, and dispatch operations using `System.Diagnostics.Activity`, following the [OpenTelemetry messaging semantic conventions](https://opentelemetry.io/docs/specs/semconv/messaging/). This enables consistent tracing and monitoring across your messaging infrastructure.

## Activity source

The transport emits activities under the activity source named `NServiceBus.Transport.IBMMQ`. Register this source with the OpenTelemetry tracer provider to collect and export transport-level traces for monitoring and diagnostics:

```csharp
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing
        .AddSource("NServiceBus.Transport.IBMMQ")
        .AddOtlpExporter());
```

## Activities

The following activities are created for each transport operation. The `Kind` field indicates the activity's role in the messaging flow:
- **Consumer**: Receiving messages from the transport
- **Producer**: Sending or publishing messages
- **Internal**: Internal processing operations

| Activity name | Kind | Display name |
|:---|---|---|
| `NServiceBus.Transport.IBMMQ.Receive` | Consumer | `receive {queueName}` |
| `NServiceBus.Transport.IBMMQ.Dispatch` | Internal | `dispatch` |
| `NServiceBus.Transport.IBMMQ.PutToQueue` | Producer | `send {destination}` |
| `NServiceBus.Transport.IBMMQ.PutToTopic` | Producer | `publish {topicName}` |
| `NServiceBus.Transport.IBMMQ.Attempt` | Internal | _(unnamed)_ |

The `Attempt` activity wraps each processing attempt inside the immediate retry loop. It records failure details when message processing raises an exception.

NServiceBus core links its receive pipeline activity to the transport `Receive` activity. This creates a complete trace from message dequeue through handler execution.

## Tags

Activities are tagged with contextual information used for filtering, querying, and analyzing traces.
### Standard messaging tags

These tags follow the [OpenTelemetry messaging semantic conventions](https://opentelemetry.io/docs/specs/semconv/messaging/):

| Tag | Activities | Value |
|:---|---|---|
| `messaging.system` | All | `ibm_mq` |
| `messaging.destination.name` | `Receive`, `PutToQueue`, `PutToTopic` | Queue or topic name |
| `messaging.operation.type` | `Receive`, `PutToQueue`, `PutToTopic` | `receive`, `send`, or `publish` |
| `messaging.message.id` | `PutToQueue`, `PutToTopic` | Native MQ message ID as a hex string |
| `messaging.batch.message_count` | `Dispatch` | Total number of outgoing operations in the batch |

### Vendor-specific tags

| Tag | Activities | Value |
|:---|---|---|
| `nservicebus.transport.ibmmq.topic_string` | `PutToTopic` | The IBM MQ topic string used to open the topic |
| `nservicebus.transport.ibmmq.failure_count` | `Receive`, `Attempt` | Number of processing failures for the current message |

## Activity events

The following events are added to the active receive activity when a transaction outcome is recorded:

| Event | When |
|:---|---|
| `mq.commit` | Transaction committed successfully |
| `mq.backout` | Transaction backed out (message returned to queue) |

## Error status

When an operation fails, the activity status is set to `Error` with the exception message as the description. This applies to `Receive`, `Dispatch`, `PutToQueue`, `PutToTopic`, and `Attempt` activities.
