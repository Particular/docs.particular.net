---
title: Non-Durable Transport
summary: A transport for exchanging NServiceBus messages in memory in a non-durable fashion
component: NonDurableTransport
reviewed: 2026-07-31
related:
 - persistence/non-durable
 - samples/non-durable-transport
redirects:
 - nservicebus/non-durable-transport
---

The Non-Durable Transport exchanges NServiceBus messages in memory. Because messages are held only in process memory, they are lost when the process ends. This trade-off makes the transport exceptionally fast and removes all external infrastructure dependencies, which is ideal when throughput matters more than durability.

Use this transport when:

- Endpoints run in the same process and data loss on restart is acceptable.
- High throughput is required and the overhead of a persistent queueing system is unnecessary.
- Starting with synchronous, mediator-like behavior and gradually moving toward true asynchronicity.
- Integrating with webhooks: the HTTP endpoint immediately acknowledges the caller by dispatching work to a local non-durable queue. NServiceBus retries the work as needed, and transient failures can be discarded rather than moved to an error queue.

> [!WARNING]
> Messages are lost when the process ends. Ensure this trade-off is acceptable for the target scenario.

## Transport at a glance

|Feature                    |   |
|:---                       |---
|Transactions               |None, ReceiveOnly, SendsAtomicWithReceive
|Pub/Sub                    |Native
|Timeouts                   |Native
|Large message bodies       |In-memory only; limited by available process memory
|Scale-out                  |Competing consumer (single process only)
|Scripted Deployment        |Not supported
|Installers                 |Not supported
|Native integration         |Not supported
|OpenTelemetry tracing      |[Supported](observability.md)
|Case Sensitive             |Yes
|Aspire integration         |No

## Prerequisites

- All endpoints must be hosted in the same process.
- The system must be able to afford losing all messages when the process ends.

## Usage

snippet: NonDurableTransport

The parameterless constructor automatically shares a single in-memory broker across all endpoints in the same process. Endpoints communicate with each other without additional broker configuration.

### Sharing a broker across endpoints

By default, multiple endpoints in the same process automatically share a single in-memory broker. The transport resolves a broker with the following precedence:

1. A `NonDurableBroker` registered in dependency injection.
2. A broker passed via `NonDurableTransportOptions`.
3. A static shared broker used by all parameterless-constructor instances.

Explicitly supply a broker when you need isolation between endpoint groups or when testing:

snippet: NonDurableTransport-SharedBroker

## Transactions and delivery guarantees

The transport supports the following [Transport Transaction Modes](/transports/transactions.md):

- Sends atomic with Receive (Default)
- Receive Only
- Unreliable (Transactions Disabled)

## Transaction sharing with non-durable persistence

When the [Non-Durable Persistence](/persistence/non-durable) is used together with the Non-Durable Transport in `SendsAtomicWithReceive` mode, the persistence automatically detects the transport's transaction and enlists in it. Saga updates and outgoing messages commit or roll back atomically for the non-outbox path. When the outbox is enabled, the outbox transaction provides its own consistency boundary.

Use both together when:

- Sagas must be updated atomically with message dispatch.
- A single-process system needs transactional message handling without external infrastructure.
- The overhead of a persistent outbox or distributed transaction coordinator is undesirable.

## Inline execution

The transport supports an optional inline execution mode. When enabled, sends to local queues are executed synchronously in the sending thread rather than being enqueued for the pump to process later. This behaves like a mediator pattern: the caller waits for the handler to complete and receives any exceptions immediately.

This is useful for:

- Gradually moving a codebase from synchronous method calls toward asynchronous messaging.
- Scenarios that require mediator-like behavior with immediate feedback.
- Tests that need deterministic, immediate execution without background polling.

Providing a non-null `InlineExecutionOptions` enables the inline execution mode, and additional settings are available as properties on the class:

snippet: NonDurableTransport-InlineExecution

## Shutdown behavior

The transport supports configurable shutdown behavior via `NonDurableTransportShutdownBehavior` on `NonDurableTransportOptions`.

snippet: NonDurableTransport-ShutdownBehavior

### DrainQueueBeforeShutdown (default)

The default behavior implements a graceful shutdown that attempts to drain the in-memory queue until the queue is empty. If the cancellation token is triggered, cancellation signals are propagated to all message handlers.

This behavior deviates from other transports but ensures that any in-progress multi-message flows are given a chance to complete before the endpoint shuts down. For the queue to drain, producers must be stopped before consumers so that the remaining messages can be processed. This gives slightly better reliability guarantees, which is why it is the default.

However, if the queue never empties, the endpoint will be unable to shut down until the cancellation token is signaled. This could happen, for example, if a message handler always sends a new message to the queue in a loop.

### ShutdownAfterHandlerExit

This mode more closely resembles durable message transports. The endpoint allows message pipelines admitted before shutdown to complete but does not admit additional queued or inline message pipelines. A message already fetched from the queue when shutdown begins is considered in-flight.

An inline operation attempted after its destination receiver begins stopping is rejected. The originating parent message then follows its configured recoverability policy. If recoverability requests a retry, the parent message is requeued and remains buffered until processing restarts.

If a cancellation token is provided to the Stop method and it signals cancellation, the in-flight message handlers will be interrupted to force the endpoint to stop faster.

Buffered messages remain in the queue on shutdown. They can be processed if the same receiver starts again, for example, through `ChangeConcurrency`; otherwise they are lost when the `NonDurableBroker` is disposed. A new endpoint using the same broker can process a buffered message, but it cannot complete an inline-execution dispatch task owned by the previous endpoint instance. Use `DrainQueueBeforeShutdown` if inline cascades must be given an opportunity to complete before shutdown returns.

## Broker disposal

Disposing the `NonDurableBroker` completes all queues and stops the delayed message pump. Any buffered messages will be lost. Receivers drain any remaining buffered envelopes before exiting.

> [!NOTE]
> Because the transport uses a static shared broker by default, the broker is not automatically disposed when a single endpoint stops. Explicitly dispose the broker only when you created it manually and want to free memory and signal completion to all queues. When the broker is registered in the dependency injection container, the container manages its disposal.

## Advantages

- Extremely fast; no I/O overhead.
- No external infrastructure required.
- Ideal for automated testing and local development.
- Inline execution enables mediator-like behavior and gradual adoption of asynchronous messaging.

## Disadvantages

- All messages are lost when the process ends.
- Endpoints must run in the same process.
- No native integration with external systems. Use the [NServiceBus Messaging Bridge](/nservicebus/bridge) to connect non-durable endpoints to endpoints using a durable transport. For example, tenant endpoints can run on the non-durable transport while ServiceControl and audit infrastructure run on SQL Server or Azure Service Bus.
