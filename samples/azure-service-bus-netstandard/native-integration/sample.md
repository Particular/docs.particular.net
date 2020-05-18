---
title: Azure Service Bus Native Integration
summary: How to consume messages published by non-NServiceBus endpoints.
component: ASBS
isLearningPath: true
reviewed: 2020-05-18
related:
 - transports/azure-service-bus
---

This sample shows how to send a message from non-NServiceBus code using the Azure Service Bus API and process it with an NServiceBus endpoint using the Azure Service Bus transport.

## Prerequisites

include: asb-connectionstring-xplat


## Code walk-through

The sample contains two executable projects:

 * `NativeSender` - sends native messages to the `Receiver`'s queue.
 * `Receiver` - NServiceBus endpoint that processes messages sent by `NativeSender`.


## Sending messages with the Azure Service Bus API

Configuring the native sender to send messages to the queue used by the receiving endpoint is required when integrating the Azure Service Bus sender with NServiceBus endpoints. By default, the input queue for an NServiceBus endpoint is its endpoint name.

snippet: EndpointName

The native sender is using `QueueClient` to send a single `Message`.


## Message serialization

The NServiceBus endpoint is using [JSON serialization](/nservicebus/serialization/newtonsoft.md). Therefore, the message sent by a native sender must be valid JSON.

snippet: SerializedMessage

To generate a serialized message, the `MessageGenerator` project can be used with the unit test named `Generate` under the `SerializedMessageGenerator` test fixture.

## Message type detection

The native message must allow NServiceBus to [detect the message type either via the headers or the message payload](/nservicebus/messaging/message-type-detection.md).

In this sample the header option is used by storing the `FullName` of the message as an Azure Service Bus `Message` user property.

snippet: NecessaryHeaders

## Message definition

The message itself is defined using [conventions](/nservicebus/messaging/conventions.md) in the `Receiver` project.

snippet: NativeMessage


## Handling messages from a native sender in an NServiceBus endpoint

Once the message is received by the NServiceBus endpoint, its content will be presented.

snippet: NativeMessageHandler


## Things to note

 * The use of the `AzureServiceBus_ConnectionString` environment variable mentioned above.
 * Execute `Receiver` first to create the destination queue that `NativeSender` will need to send native messages.
