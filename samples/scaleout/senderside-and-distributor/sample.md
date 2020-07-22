---
title: Multi-message conversations when scaling out
summary: How messages belong to the same conversation are routed depending on the scaling out approach used
component: Distributor
reviewed: 2020-07-22
related:
 - transports/msmq/sender-side-distribution
 - transports/msmq/distributor
 - samples/scaleout/distributor-upgrade
 - samples/scaleout/senderside
---

The sample demonstrates differences in behavior of some of the NServiceBus APIs that depend on the scale-out method.


## Prerequisites

Make sure MSMQ is installed and configured as described in the [MSMQ Transport - MSMQ Configuration](/transports/msmq/#msmq-configuration) section.


## Running the sample

 1. Start the solution.
 1. Press `A` in Sender.V5 console.
 1. Wait until a `Confirming order {orderId}` message shows up in one of the worker consoles.
 1. Notice that the messages that took part in the order flow conversation were processed by both workers in the round-robin way, each time going through the Distributor.
 1. Press `A` in Sender.V6 console.
 1. Wait until a `Confirming order {orderId}` message shows up in one of the worker consoles.
 1. Notice that all the messages that took part in the order flow conversation were processed by a single worker.
 1. Press `A` in Sender.V6 console.
 1. Wait until a `Confirming order {orderId}` message shows up in one of the worker consoles.
 1. Notice that all the messages that took part in the order flow conversation were processed by a single worker (different than previously).
 1. Press `B` in the Sender.V5 console.
 1. Notice the message fails twice until it ultimately succeeds. Each retry is routed to different worker.
 1. Press `B` in the Sender.V6 console.
 1. Notice the message fails twice until it ultimately succeeds. Each retry is routed to different worker.


## Code walk-through
 
This sample contains five NServiceBus console applications

 * `Sender.V5` sends commands to the scaled out endpoint via [Distributor](/transports/msmq/distributor/).
 * `Sender.V6` sends commands to the scaled out endpoint directly using [Sender-Side Distribution](/transports/msmq/sender-side-distribution.md)
 * `Distributor`
 * `Worker.1` and `Worker.2` process commands simulating order flow. They represent the scaled out endpoint.


## APIs that depend on scaling out approach

The behavior of following APIs depend on the selected scaling out approach. The behaviour is slightly different if a message is sent by the Distributor, or from when it is sent by other endpoints. In case of [delayed retries](/nservicebus/recoverability/#delayed-retries) the behavior doesn't depend on which endpoint sent it, it's based on configuration only.


### SendLocal

[Sending to the local endpoint](/nservicebus/messaging/send-a-message.md#sending-to-self) when processing a message sent via a Distributor routes the message back to the Distributor. The Distributor then routes it to the first available worker.

snippet: SendLocal

When a message was sent directly or when it was called from outside of a message handler, the message is routed to the local queue of processing instance.


### Defer

[Deferring a message](/nservicebus/messaging/delayed-delivery.md) to the local endpoint when processing a message sent via a Distributor creates a pending timeout with destination set to the Distributor's queue. When that timeout is due, the message is sent back to the Distributor. The Distributor then routes it to the first available worker.

snippet: Defer


### ReplyTo

Sending a message when processing a message sent via a Distributor sets the [reply to](/nservicebus/messaging/routing.md#reply-routing) header to the Distributor's queue. 

snippet: Reply

When another endpoint replies to such message, the reply is routed to the Distributor. The Distributor then routes it to the first available worker.


### Delayed retries

snippet: DelayedRetry

Moving a message to delayed retries in MSMQ uses the same mechanism as message deferrals mentioned above. However, the behaviour is identical for messages sent via Distributor and others. The message is always deferred to the Distributor queue if the endpoint is configured to enlist with the Distributor.

snippet: Enlisting

Otherwise, the message is deferred to the local instance queue.
