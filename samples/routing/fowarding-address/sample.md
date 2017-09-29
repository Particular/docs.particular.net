---
title: Forwarding address
summary: Implementing forwarding address for in-flight messages during handler migration
component: Core
reviewed: 2017-09-27
tags:
- Routing
---

When a handler is migrated to a new endpoint there can be outstanding messages in-flight. When these messages arrive at the original endpoint they need to be re-routed to the new endpoint.

NServiceBus can already be configured to [forward a copy of every message that it processes to another address](/nservicebus/messaging/forwarding.md). This sample is different in a few key ways:

 1. This sample only forwards messages which have a configured forwarding address. Each message type can have a different forwarding address. The built-in forwarding forwards every message to a single destination address.
 1. This sample forwards messages to a logical endpoint, rather than a physical address. This allows it to interact with [sender-side distribution](/transports/msmq/sender-side-distribution.md) if required.


## Running the project

 1. Start all the projects by hitting F5.
 1. The text `Endpoint Started. Press s to send a very important message. Any other key to exit` should be displayed in the Sender's console window.
 1. Press S to send a message.
 1. The message will be processed by OriginalDestination
 1. The message will also be processed by NewDestination

Sender is configured to send messages to OriginalDestination which is configured to forward a copy to NewDestination.

Remove the handler code from OriginalDestination and run the sample again. Note that Sender is still configured to send the message to OriginalDestination which will forward a copy to NewDestination even though it no longer contains a handler of the correct type.


## Code walk-through

This sample contains four projects.


### Sender

Contains routing configuration to send `ImportantMessage` to the _OriginalDestination_ endpoint.

snippet: route-message-to-original-destination


### OriginalDestination

Configures the forwarding address for messages of the `ImportantMessage` type.

snippet: forward-message-to-new-destination

Original handler for `ImportantMessage` messages.

snippet: old-handler

NOTE: This handler will still be called but can be safely removed if no longer required.


### NewDestination

Contains the new handler for `ImportantMessage` messages.

snippet: new-handler


### NServiceBus.ForwardingAddress

Contains the implementation logic for leaving a forwarding address. There are two behaviors in the pipeline to implement this behavior.

The first behavior forks from the incoming physical context into the forwarding context after the message has been processed.

snippet: forward-matching-messages-behavior

The second behavior is installed in the incoming logical context and matches incoming messages to a forwarding address.

snippet: set-forwarding-address-behavior

NOTE: This behavior sets the `context.MessageHandled` to `true`. This allows the message handler to be removed from the endpoint containing the forwarding address.

This project also contains a feature to wire up the two main behaviors.

snippet: forwarding-feature

The routing configuration extension enables the forwarding address feature and records the forwarding address.

snippet: forwarding-routing-extensions


### Messages

A shared assembly containing the `ImportantMessage` message type.