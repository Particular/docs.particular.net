
Enable OpenTelemetry instrumentation in NServiceBus:

snippet: opentelemetry-enableinstrumentation

With OpenTelemetry instrumentation enabled, tracing, metrics, and logging can be individually configured via the OpenTelemetry API itself.

## Traces

NServiceBus endpoints generate OpenTelemetry traces for incoming and outgoing messages. To capture trace information, add the `NServiceBus.Core` activity source to the OpenTelemetry configuration:

snippet: opentelemetry-enabletracing

### Emitted span structure

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

To force the creation of a new trace when receiving the message, the `SendOptions`-API can be used as follows:

snippet: opentelemetry-sendoptions-start-new-trace

This ensures a new trace is created, and links the send and receive spans, which looks as follows:

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

To force the continuation of the existing trace when receiving the message, the `PublishOptions`-API can be used as follows:

snippet: opentelemetry-publishoptions-continue-trace

This ensures the trace is continued and the receive span to be created as a child of the publish span, which looks as follows:

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

See the [OpenTelemetry samples](/samples/open-telemetry/) for instructions on how to send trace information to different tools.
