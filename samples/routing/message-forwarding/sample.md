---
title: Message forwarding
summary: Forwarding a copy of every message to another destination after it is processed
component: Core
reviewed: 2020-07-22
---

In complex upgrade scenarios it can be useful to forward a copy of processed messages to another destination. This allows an old version of an endpoint to run side-by-side with a new version, both processing the message until the old version can be fully retired.

## Running the sample

1. Open the solution in Visual Studio.
1. Press F5.
1. Follow the instructions in sender's console window to send a message to the OriginalDestination endpoint.
1. The message will be processed by the OriginalDestination endpoint.
1. The message will also be processed by the UpgradedDestination endpoint.

The sender is configured to sends messages to OriginalDestination, and OriginalDestination is configured to forward a copy of every message it proceses to UpgradedDestination.


## Code walk-through

The sample contains four projects.


### Sender

Contains routing configuration to send `ImportantMessage` messages to the OriginalDestination endpoint.

snippet: route-message-to-original-destination


### OriginalDestination

This endpoint configures the forwarding address for `ImportantMessage` messages.

snippet: forward-messages-after-processing

The endpoint also contains the original handler for `ImportantMessage` messages.

snippet: old-handler


### UpgradedDestination

This endpoint contains the new handler for `ImportantMessage` messages.

snippet: new-handler


### NServiceBus.MessageForwarding

Contains the implementation logic for the message forwarding behavior. This behavior forks the incoming physical context into the routing context after the message has been processed.

The first behavior forks the incoming physical context into the forwarding context after the message has been processed. Note that the behavior copies the headers and body before the message is processed so that an exact copy of the received message is forwarded.

snippet: forward-processed-messages-behavior

This project also contains a configuration extension to specify the forwarding address and wire up the behavior.

snippet: message-forwarding-configuration


### Messages

A shared assembly containing the `ImportantMessage` message type.