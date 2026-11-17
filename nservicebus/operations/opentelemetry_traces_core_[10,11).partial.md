### Trace sources

NServiceBus emits spans from three ActivitySources:

| Source | Description |
|---|---|
| `NServiceBus.Core` | Pipeline spans: send, publish, process |
| `NServiceBus.Core.Handler` | Handler invocation spans (one per handler per message) |
| `NServiceBus.Core.Recoverability` | Recoverability action spans (immediate retry, delayed retry, move to error, discard) |

Subscribe to the sources needed for the endpoint's observability requirements:

snippet: opentelemetry-enabletracing-all-sources

> [!NOTE]
> In version 10, handler spans are emitted from `NServiceBus.Core`. They move to `NServiceBus.Core.Handler` only when the version 11 behavior is enabled, as described under *Version 11 behavior opt-in* below. In version 11 they always come from `NServiceBus.Core.Handler`.
>
> A tracer that subscribes only to `NServiceBus.Core` therefore receives no handler spans once the version 11 behavior is active.

All three sources report version `0.1.0` in version 10, and `1.0.0` when the version 11 behavior is enabled. Use the version to tell the two sets of span names and tags apart.

Subscribing to `NServiceBus.Core.Handler` without subscribing to `NServiceBus.Core` suppresses handler spans - `Activity.Current` inside handlers and behaviors becomes the pipeline span. This enables a flattened trace view where handler work appears directly on the process span.

### Version 11 behavior opt-in

Some OpenTelemetry behaviors change in version 11. Version 10 keeps the version 10 behavior by default. One AppContext switch enables all of the changes together, so an endpoint can adopt the version 11 telemetry before it upgrades:

snippet: opentelemetry-v11-behavior-switch

The switch can also be set without code. With an environment variable:

```text
DOTNET_NServiceBus_Core_OpenTelemetry_UseV11Behavior=true
```

Or in the project file:

```xml
<ItemGroup>
  <RuntimeHostConfigurationOption Include="NServiceBus.Core.OpenTelemetry.UseV11Behavior" Value="true" />
</ItemGroup>
```

The switch is read once, before the endpoint starts, and applies to the whole process. Two endpoints in the same process cannot use different modes.

The switch enables all of these changes:

| Behavior | Version 10 | Version 11 |
|---|---|---|
| Handler span source | `NServiceBus.Core` | `NServiceBus.Core.Handler` |
| ActivitySource version | `0.1.0` | `1.0.0` |
| Parent of the process span under an instrumented transport SDK | NServiceBus send span | SDK receive span, with a link to the send span |
| Span names | generic, for example `process message` | operation and target, for example `process orders` |
| Trace context and baggage propagation | custom propagator | `System.Diagnostics.DistributedContextPropagator` |
| `Start dispatching` and `Finished dispatching` span events | emitted | not emitted |
| Meter name and version | `NServiceBus.Core.Pipeline.Incoming`, `0.4.0` | `NServiceBus.Core`, `1.0.0` |
| Span status on success | `Ok` | `Unset` |
| `execution.result` metric tag | emitted | not emitted |
| `otel.status_code`, `otel.status_description` and `exception.escaped` | emitted on failures | not emitted |
| Outbox deduplication span tag | `nservicebus.outbox.deduplicate-message` | `nservicebus.outbox.deduplicated_message` |
| `nservicebus.event_types` and `nservicebus.enclosed_message_types` span tags | delimited string | array of type names |

`PublishTraceMode` is not part of the switch. It stays configurable in version 11; only its default changes. See *Publish operations* below.

> [!WARNING]
> The switch changes what an OpenTelemetry consumer sees. Dashboards, alerts and queries that match on span names, on a span status of `Ok`, on the meter name, on the `execution.result` metric tag, or on the span tags above must be updated. A `MeterProvider` must subscribe to the meter name that matches the switch, or use a wildcard, as described under [Meter name and version](#meters-meter-name-and-version). The baggage wire format changes as well, which affects endpoints that exchange baggage with endpoints that do not have the switch enabled. Read the [version 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/) before the switch is enabled.

In version 11 these behaviors are the only behaviors, and the switch is removed.

### Span relationships

#### Send operations

A span is emitted for each message sent by an NServiceBus endpoint. When the message is received, a receive span is created as a child to the send span.

```mermaid
flowchart LR;
  subgraph SENDER
  direction TB
   NSBM1[NServiceBus Send span]
  end
  subgraph RECEIVER
  direction TB
  PRM1[NServiceBus Process span]

  end
  NSBM1--child--> PRM1
```

The default trace behavior for sends is to continue the existing trace: the receiver span is a child of the sender span. To override this for a specific message, use `SendOptions`:

snippet: opentelemetry-sendoptions-start-new-trace

This creates a new trace on the receiver and links the send and receive spans:

```mermaid
flowchart LR;
  subgraph SENDER
  direction TB
   NSBM1[NServiceBus Send span]
  end
  subgraph RECEIVER
  direction TB
  PRM1[NServiceBus Receive span]

  end
  NSBM1-. link .-PRM1;
```

To change the default for all sends from an endpoint, set `SendTraceMode`:

snippet: opentelemetry-trace-mode-send

#### Publish operations

A span is emitted for each message published by an NServiceBus endpoint. When the message is processed by a subscriber, a process span is created in a new trace, which is linked to the publish span.


```mermaid
flowchart LR;
  subgraph PRODUCER
  direction TB
   NSBM1[NServiceBus Publish span]
  end
  subgraph CONSUMER
  direction TB
  PRM1[NServiceBus Process span]

  end
  NSBM1-. link .-PRM1;
```

The default trace behavior for publishes is to start a new linked trace on each subscriber. To override this for a specific event, use `PublishOptions`:

snippet: opentelemetry-publishoptions-continue-trace

This continues the publisher's trace in the subscriber:

```mermaid
flowchart LR;
  subgraph PRODUCER
  direction TB
   NSBM1[NServiceBus Publish span]
  end
  subgraph CONSUMER
  direction TB
  PRM1[NServiceBus Process span]

  end
  NSBM1--child--> PRM1
```

To change the default for all publishes from an endpoint, set `PublishTraceMode`:

snippet: opentelemetry-trace-mode-publish

The opposite override is also available. Use it to keep the new trace for one event after the endpoint
default was changed to `ContinueExisting`:

snippet: opentelemetry-publishoptions-start-new-trace

Or for all publishes from the endpoint:

snippet: opentelemetry-trace-mode-publish-start-new

> [!NOTE]
> In version 10, `PublishTraceMode` defaults to `StartNew`. In version 11 the default becomes
> `ContinueExisting`, so subscribers continue the publisher's trace unless the endpoint or the publish
> opts out. `PublishTraceMode` is not governed by the version 11 behavior switch; only its default
> changes with the major version.

Per-message overrides (`StartNewTraceOnReceive`, `ContinueExistingTraceOnReceive`) always take precedence over the endpoint-level defaults.

#### Transport SDK spans

Some transport SDKs, such as the Azure Service Bus, RabbitMQ, and Amazon SQS clients, emit their own spans for the native send and receive operations. When the endpoint subscribes to the SDK's ActivitySource, the SDK receive span is the ambient `Activity.Current` at the moment NServiceBus starts processing a message.

In version 10, the default is unchanged in that situation: the NServiceBus process span is a child of the NServiceBus send span. The process span becomes a child of the SDK receive span, with a link back to the NServiceBus send span, when the version 11 behavior is enabled:

```mermaid
flowchart LR;
  subgraph SENDER
  direction TB
   NSBM1[NServiceBus Send span]
  end
  subgraph RECEIVER
  direction TB
  SDK1[Transport SDK Receive span]
  PRM1[NServiceBus Process span]
  end
  SDK1--child--> PRM1
  NSBM1-. link .-PRM1;
```

If no listener is subscribed to the SDK's ActivitySource, no SDK span exists and the process span is created as described in the sections above, whether or not the switch is set. This is the only behavior in version 11.

### Delayed messages

When a message is delayed - whether by explicit delay (`SendOptions.DelayDeliveryWith`), saga timeout, or delayed retry - a new linked trace is started at delivery time by default. This reflects that the receive operation happens at a different moment in time than the send or retry decision.

The trace behavior for each category of delayed message is configurable independently:

snippet: opentelemetry-trace-mode-delayed

| Option | Default | Applies to |
|---|---|---|
| `DelayedDelivery.SendOperationTraceMode` | `StartNew` | `SendOptions.DelayDeliveryWith` / `DoNotDeliverBefore` |
| `DelayedDelivery.SagaTimeoutTraceMode` | `StartNew` | Saga timeouts (`Saga.RequestTimeout`) |
| `Recoverability.DelayedRetryTraceMode` | `StartNew` | Delayed retries driven by recoverability policy |

### Recoverability spans

When a message cannot be processed successfully, NServiceBus emits a recoverability span from the `NServiceBus.Core.Recoverability` ActivitySource. The span carries a `nservicebus.recoverability_action` tag indicating the outcome:

| Tag value | Meaning |
|---|---|
| `immediate_retry` | Message will be retried immediately |
| `delayed_retry` | Message will be retried after a delay |
| `move_to_error` | Message is moved to the error queue |
| `discard` | Message is discarded without further processing |

A recoverability span covers the recoverability decision and carrying out the action. It is created from the trace context of the incoming message, in the same way as the process span, so it is not a child of the process span. It links to the innermost span that failed. The exception is logged under the recoverability span, see [exception recording](#traces-exception-recording). To receive recoverability spans, subscribe to the `NServiceBus.Core.Recoverability` ActivitySource.

### Span names

In version 10, NServiceBus uses generic operation names for spans, such as `process message` and `publish event`. When the version 11 behavior is enabled, the names follow the OpenTelemetry messaging semantic convention format `{operation} {target}`. The target is the queue for receive and recoverability spans, and the message type name for outgoing spans:

| Operation | Version 10 | Version 11 |
|---|---|---|
| Process | `process message` | `process {receiveAddress}` |
| Send | `send message` | `send message {MessageType}` |
| Publish | `publish event` | `publish {EventType}` |
| Reply | `reply` | `reply {MessageType}` |
| Subscribe | `subscribe event` | `subscribe event {EventType}` |
| Unsubscribe | `unsubscribe event` | `unsubscribe event {EventType}` |
| Immediate retry | `immediate retry` | `immediate retry {receiveAddress}` |
| Delayed retry | `delayed retry` | `delayed retry {receiveAddress}` |
| Move to error | `move to error` | `move to {errorQueue}` |
| Discard | `discard` | `discard` |

A subscribe span for several event types lists the type names separated by spaces. Message type names are short names, not full type names.

### Dispatching events

When outgoing messages are dispatched during message processing, NServiceBus adds two span events to the incoming pipeline span:

- `"Start dispatching"` - emitted before dispatch, includes a `message-count` event tag
- `"Finished dispatching"` - emitted after dispatch completes

These events are emitted in version 10. They are not emitted when the version 11 behavior is enabled, and they are removed in version 11. There is no option to keep them.

### Context propagation

NServiceBus propagates the [W3C Trace Context](https://www.w3.org/TR/trace-context/) and [W3C Baggage](https://www.w3.org/TR/baggage/) headers between endpoints.

In addition to the W3C `traceparent` header, NServiceBus writes the context of the send or publish span to the `NServiceBus.TraceParent` header. Transport SDKs that emit their own spans overwrite `traceparent` on the message with the context of their native send span, and the NServiceBus-specific header keeps the NServiceBus send span reachable for the receiver. Receivers use `NServiceBus.TraceParent` when present and fall back to `traceparent`, so messages from endpoints on versions that only write the W3C header continue the trace as before.

#### Which messages carry trace context

Only messages that flow through the outgoing pipeline - sends, publishes, replies, and delayed messages - receive the context of the current activity. Messages that NServiceBus forwards on behalf of a received message keep the trace headers of that message unchanged:

- Messages moved to the error queue
- Delayed retries
- Audit copies
- Messages forwarded with `IMessageProcessingContext.ForwardCurrentMessageTo`

This keeps the forwarded message correlated to its original sender instead of the span that happened to be active while it was forwarded. Control messages that NServiceBus sends outside the outgoing pipeline, such as message-driven subscribe and unsubscribe requests and ServiceControl retry acknowledgements, carry the current activity's context.

Custom `RecoverabilityAction` and `AuditAction` implementations that forward the received message get the same behavior: the headers are dispatched as they were received, so the trace stays intact without any additional work.

In version 10, NServiceBus uses a custom propagator. Propagation moves to the built-in .NET `DistributedContextPropagator` when the version 11 behavior is enabled, and the custom propagator is removed in version 11. The `baggage` header format changes with it. See the [version 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/) for the serialization details and for the effect on a rolling upgrade.

### Span status

NServiceBus sets the span status to `Ok` when an operation completes without an error. 

### Failed spans and the error.type tag

When an operation fails, NServiceBus sets the status of every span the exception passes through to `Error` and adds an `error.type` tag containing the fully qualified exception type name. For example, an exception thrown by a handler marks the handler span and the process span, and an exception from a send inside a handler also marks the send span. The status and the tag are not exception details, so marking every span does not record the exception more than once.

### Exception recording

NServiceBus reports each exception once in the log and, depending on the exception recording mode, once in the trace.

#### Logging

An exception is logged once, by the part of NServiceBus that knows the outcome:

| Failure | Logged by | Log level |
|---|---|---|
| Processing an incoming message | The recoverability action that is carried out | `Info` for an immediate retry, `Warn` for a delayed retry, `Error` for a move to the error queue, `Info` for a discard |
| An operation on the message session, outside of message processing, such as a send or publish | The operation | `Warn` |
| Recoverability itself | The [critical error](/nservicebus/hosting/critical-errors.md) | `Fatal` |

A failure while processing a message is not logged where the exception is thrown, because at that point it is not yet known whether the message will be retried or moved to the error queue. The recoverability log entry includes the full exception and is written under the recoverability span. A failed message session operation is logged under the span of that operation. The entries are logged in both exception recording modes, and also when tracing is not enabled. A custom `RecoverabilityAction` logs nothing unless it logs itself.

To see the exception details next to the trace, use a logging provider that attaches the trace context to each log record, such as the OpenTelemetry logging provider for `Microsoft.Extensions.Logging`.

#### Exception recording mode

The `ExceptionRecordingMode` property on `InstrumentationOptions` controls whether the exception details are also recorded in the trace. It does not affect logging.

| Mode | Exception details in the trace |
|---|---|
| `SpanAndLogs` (default) | An `exception` event with the `exception.type`, `exception.message`, and `exception.stacktrace` attributes, added once, on the innermost span the exception passed through |
| `Logs` | None. The failed spans have the `Error` status and the `error.type` tag only |

> [!NOTE]
> The [OpenTelemetry semantic conventions for exceptions](https://opentelemetry.io/docs/specs/semconv/exceptions/exceptions-logs/) are moving exception details from span events to log records. NServiceBus follows this transition. In version 11, exception details are recorded only in the log. See the [version 10 to 11 upgrade guide](/nservicebus/upgrades/10to11/).

`Logs` mode avoids storing the exception details twice. It is a good fit when log aggregation is the primary tool for investigating failures, or when span event storage is expensive or not supported by the observability backend. To use it, configure `ExceptionRecordingMode`:

snippet: opentelemetry-exception-recording-logs

#### Environment variable override

The exception recording mode can also be set with the [`OTEL_SEMCONV_EXCEPTION_SIGNAL_OPT_IN`](https://opentelemetry.io/docs/specs/semconv/exceptions/exceptions-logs/) environment variable, which is part of the standard OpenTelemetry transition mechanism from span events to log records. The environment variable takes precedence over the value configured in code, so operators can change the mode per environment through deployment configuration, without a code change or redeploy.

| Environment variable value | Equivalent `ExceptionRecordingMode` |
|---|---|
| `logs` | `Logs` |
| `logs/dup` | `SpanAndLogs` |

When the environment variable is not set, the value configured in code is used, defaulting to `SpanAndLogs`. Any other value is ignored without a warning, and the value configured in code is used. Check `ExceptionRecordingMode` in the [startup diagnostics](#traces-startup-diagnostics) to confirm the mode an endpoint runs with.

### Startup diagnostics

The effective instrumentation options are written to the `OpenTelemetry` section of the [startup diagnostics](/nservicebus/hosting/startup-diagnostics.md) file. Use this section to verify which trace modes and exception recording mode an endpoint runs with. `ExceptionRecordingMode` shows the value after the `OTEL_SEMCONV_EXCEPTION_SIGNAL_OPT_IN` environment variable override is applied.

```json
"OpenTelemetry": {
  "SendTraceMode": "ContinueExisting",
  "PublishTraceMode": "StartNew",
  "ExceptionRecordingMode": "SpanAndLogs",
  "Recoverability": {
    "DelayedRetryTraceMode": "StartNew"
  },
  "DelayedDelivery": {
    "SendOperationTraceMode": "StartNew",
    "SagaTimeoutTraceMode": "StartNew"
  },
  "UseV11Behavior": false
}
```

`UseV11Behavior` shows whether the `NServiceBus.Core.OpenTelemetry.UseV11Behavior` AppContext switch is enabled. The switch turns on the version 11 behaviors described in [Version 11 behavior opt-in](#traces-version-11-behavior-opt-in). It does not change the `PublishTraceMode` default, which stays `StartNew` until version 11.

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.
