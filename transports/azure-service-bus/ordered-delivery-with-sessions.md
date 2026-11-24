---
title: Ordered delivery with sessions
component: ASBS
summary: How the Azure Service Bus transport supports ordered delivery through the use of sessions
related:
 - transports/azure-service-bus/configuration
 - samples/azure-service-bus-netstandard/sessions
reviewed: 2026-11-23
versions: '[6.6,)'
---

## Ordered delivery

Azure Service Bus supports ordered delivery through [message sessions](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-sessions). Messages that share a session ID are delivered in the order they were sent.

To process messages in order, enable sessions on the transport:

snippet: enable-sessions

Sessions apply to the whole endpoint. When installers are enabled, the transport creates the endpoint's input queue, satellite queues, and subscriptions with sessions required.

Azure Service Bus can't enable sessions on an existing queue or subscription, and installers don't update entities that already exist. If sessions are enabled on an endpoint whose queue doesn't require sessions, the endpoint can't receive its messages and logs an error that the entity isn't session-enabled.

> [!NOTE]
> Sessions are not supported on [send-only endpoint](/nservicebus/endpoints/#send-only), but they can send messages to a session-enabled endpoint through SendOptions.
> Sessions are not supported when using the [migration topology](/transports/upgrades/asbs-4to5.md). A send-only endpoint doesn't need sessions enabled to send messages with a session ID.

### Setting the session ID

Every message sent to a session-enabled queue or published to a session-enabled subscription must have a session ID. Sending a message without a session ID to a session-enabled queue throws an exception. A session-enabled queue or subscription can therefore only process messages that have a session ID.

Choose a session ID that identifies the context in which order matters, for example an order ID. Messages that should be processed in order must share the same session ID.

To send a message to a session-enabled queue:

snippet: sending-set-session-id

To publish a message to a session-enabled topic:

snippet: publishing-set-session-id

The session ID is set for each message, including messages sent or published from a handler. Only [saga timeouts](/nservicebus/sagas/timeouts.md) automatically get the session ID of the message that requested them.

Messages with a session ID that a handler sends or publishes are dispatched in the order in which the handler sent or published them.

### Processing

The transport processes one message per session at a time on each endpoint instance. Messages from different sessions are processed concurrently, up to the endpoint's [concurrency limit](/nservicebus/operations/tuning.md).

Session-enabled endpoints don't support `TransportTransactionMode.None`. The endpoint fails to start when it's configured.

### Failures

Immediate retries are supported and don't break the processing order.

[Delayed retries](/nservicebus/recoverability/#delayed-retries) would break the processing order, so session-enabled endpoints don't support them. The endpoint fails to start unless delayed retries are [disabled](/nservicebus/recoverability/configure-delayed-retries.md#disabling-through-code).

This applies only to delayed retries. Session-enabled endpoints support [delayed delivery](/nservicebus/messaging/delayed-delivery.md), including [saga timeouts](/nservicebus/sagas/timeouts.md).

When immediate retries are exhausted, the message is moved to the error queue, and the next message in the session is processed. This breaks the processing order. Retrying a message from the error queue or the dead-letter queue doesn't restore its original position in the session.

> [!NOTE]
> ServiceControl 7.0 or higher is required to replay error messages from ServicePulse for session-enabled endpoints.

### Publish/subscribe

A session-enabled endpoint receives events through session-enabled subscriptions. The transport forwards events from each subscription to the endpoint's input queue, for the events that the endpoint subscribes to, through [automatic subscriptions](/nservicebus/messaging/publish-subscribe/controlling-what-is-subscribed.md) or an explicit subscribe call. Events in a subscription that the endpoint doesn't subscribe to aren't forwarded.

Creating subscriptions requires `Manage` rights. See [access rights](/transports/azure-service-bus/configuration.md#entity-creation-access-rights) for the requirements.

Forwarding a large backlog, for example after the endpoint has been stopped, can exceed the namespace's throughput limits, and Azure Service Bus can throttle it.

### Session state

[Session state](https://learn.microsoft.com/en-us/azure/service-bus-messaging/message-sessions#message-session-state) stores data with the session on the broker. Use it to share state between the messages of a session, for example to build lightweight workflows.

Get the session state from the handler context using `GetSessionState()`, read it with `Get()`, and write it with `Set()`:

snippet: session-state-usage

In `SendsAtomicWithReceive` mode, session state changes are committed together with the incoming message. In `ReceiveOnly` mode, which the [outbox](/nservicebus/outbox/) requires, they are written immediately and aren't rolled back when processing fails. The outbox doesn't cover session state, so make session state changes idempotent when the message can be processed more than once.

Session state stays on the broker after all messages in the session have been processed, and it counts toward the queue's storage quota. Remove it with `Clear()` when the session is complete:

snippet: session-state-clear

### Sagas

When a [saga](/nservicebus/sagas/) handles messages from a session-enabled queue, correlate it on the same value that's used as the session ID. All messages for a saga instance are then processed in order.

### Endpoint setup and licensing

If a logical endpoint needs to process some message types in order while allowing others to be processed without ordering, it should be split into two endpoints: one with sessions enabled and the other with sessions disabled.

The two endpoints can also be [hosted in one process](/nservicebus/hosting/core-hosting.md#hosting-multiple-endpoints).

These endpoints should have distinct names, such as `Sales.Ordered` and `Sales.Unordered`. However, they will be treated as a single logical endpoint for licensing purposes and will count as one licensed endpoint.
