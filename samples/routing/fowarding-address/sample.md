---
title: Forwarding address
summary: Implementing forwarding address for in-flight messages during handler migration
component: Core
reviewed: 2019-06-25
---

When a handler is moved to a new endpoint, there may still be "in-flight" messages, bound for the original endpoint. When those messages arrive at the original endpoint, they need to be re-routed to the new endpoint.

An endpoint may be configured to [forward a copy of every successfully processed message to another endpoint](/nservicebus/messaging/forwarding.md). This sample is different in a few key ways:

 1. This sample only forwards messages which have a configured forwarding address. Each message type can have a different forwarding address. The built-in forwarding forwards every message to a single destination address.
 1. This sample forwards messages to a logical endpoint, rather than a physical address. This allows it to interact with [sender-side distribution](/transports/msmq/sender-side-distribution.md) if required.


## Running the sample

1. Open the solution in Visual Studio.
1. Press F5.
1. Follow the instructions in sender's console window to send a message to the OriginalDestination endpoint.
1. The message will be processed by the OriginalDestination endpoint.
1. The message will also be processed by the NewDestination endpoint.

The sender is configured to send messages to OriginalDestination, and OriginalDestination is configured to forward a copy to NewDestination.

Remove the handler code from OriginalDestination and run the sample again. Note that the sender is still configured to send the message to OriginalDestination, which will forward a copy to NewDestination even though it no longer contains a handler for the message.


## Code walk-through

The sample contains four projects.


### Sender

Contains routing configuration to send `ImportantMessage` messages to the OriginalDestination endpoint.

snippet: route-message-to-original-destination


### OriginalDestination

This endpoint configures the forwarding address for `ImportantMessage` messages.

snippet: forward-message-to-new-destination

The endpoint also contains the original handler for `ImportantMessage` messages.

snippet: old-handler

NOTE: This handler will still be called, but can be safely removed, if no longer required.


### NewDestination

This endpoint contains the new handler for `ImportantMessage` messages.

snippet: new-handler


### NServiceBus.ForwardingAddress

Contains the implementation logic for specifying a forwarding address. This is done by adding two behaviors to the pipeline.

The first behavior forks the incoming physical context into the forwarding context after the message has been processed.

snippet: forward-matching-messages-behavior

The second behavior is installed in the incoming logical context and matches incoming messages to a forwarding address.

snippet: set-forwarding-address-behavior

NOTE: This behavior sets `context.MessageHandled` to `true`. This allows the message handler to be removed from the endpoint containing the forwarding address.

This project also contains a feature to wire up the two main behaviors.

snippet: forwarding-feature

The routing configuration extension enables the forwarding address feature and records the forwarding address.

snippet: forwarding-routing-extensions


### Messages

A shared assembly containing the `ImportantMessage` message type.