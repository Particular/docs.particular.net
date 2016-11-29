---
title: Multi-message conversations when scaling out
summary: How messages belong to the same conversation are routed depending on the scaling out approach used
component: Core
tags:
- Distributor
- Conversation
- Session
related:
- nservicebus/msmq/distributor
- samples/scaleout/distributor
- samples/scaleout/senderside
---

The sample demonstrates differences in behavior of some of NServiceBus APIs that depend on the scaling out method.

## Prerequisites

Make sure MSMQ is set up as described in the [MSMQ Transport - NServiceBus Configuration](/nservicebus/msmq/#nservicebus-configuration) section.

## Running the sample

 1. Start the solution
 2. Press `A` in Sender.V5 console.
 3. Wait until `Confirming order {orderId}` message shows up in one of the worker console.
 4. Notice that the messages that took part in the order flow conversation were processed by both workers in the round-robin way, each time going through the Distributor.
 5. Press `A` in Sender.V6 console.
 6. Wait until `Confirming order {orderId}` message shows up in one of the worker console.
 7. Notice that all the messages that took part in the order flow conversation were processed by a single worker.
 8. Press `A` in Sender.V6 console.
 9. Wait until `Confirming order {orderId}` message shows up in one of the worker console.
 10. Notice that all the messages that took part in the order flow conversation were processed by a single worker (different than previously).
 11. Press `B` in the Sender.V5 console.
 12. Notice the message fails twice until it ultimately succeeds. Each retry is routed to different worker.
 13. Press `B` in the Sender.V6 console
 14. Notice the message fails twice until it ultimately succeeds. Each retry is routed to different worker.

## Code walk-through
 
This sample contains five NServiceBus processes

 * Sender.V5 sends commands to the scaled out endpoint via Distributor.
 * Sender.V6 sends commands to the sacled out endpoint directly using Sender-Side Distribution
 * Distributor
 * Worker.1 and Worker.2 process commands simulating order flow

## APIs that depend on scaling out approach

The behavior of following APIs depend on the scaling out method. Whenever possible the behavior is based on the runtime context (if the message being processed has been sent via a Distributor). In case of delayed retries the behavior is static and based on configuration only.

### SendLocal

[Sending to the local endpoint](/nservicebus/messaging/send-a-message.md#sending-to-self), when in context of processing a message sent via a Distributor, routes the message back to the Distributor which routes it to the first available worker.

snippet:SendLocal

When in context of processing a message sent directly or when called from outside of a message handler, it routes the message to the local queue of processing instance.

### Defer

[Deferring a message](/nservicebus/messaging/delayed-delivery) to the local endpoint, when in context of processing a message sent via a Distributor, creates a pending timeout with destination set to the Distributor's queue. When that timeout is due, the message is sent back to the Distributor which routes it to the fist available worker.

snippet:Defer

### ReplyTo

Sending a message, when in context of processing a message sent via a Distributor, sets the [reply to](/nservicebus/messaging/routing#reply-routing) header to the Distributor's queue. When other endpoint replies to such message, the reply is routed to the Distributor which routes it to the first available worker.

### Delayed retries

Moving a message to delayed retries in MSMQ uses the same mechanism as message deferrals mentioned above. In this case, however, the runtime context (if the messages has been sent via Distributor) is ignored. The message is always deferred to the Distributor queue if the endpoint is configured to enlist with the Distributor.

snippet:Enlisting

Otherwise, the message is deferred to the local instance queue.
