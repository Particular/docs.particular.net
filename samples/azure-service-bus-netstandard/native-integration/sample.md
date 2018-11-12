---
title: Azure Service Bus native integration
summary: Consuming messages published by non-NServiceBus endpoints.
component: ASBS
reviewed: 2018-08-21
related:
 - transports/azure-service-bus-netstandard
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


## Required headers

For a native message to be processed, NServiceBus endpoints using the Azure Service Bus transport need the message type to be included as a header. This header must be stored as an Azure Service Bus `Message` user property.

snippet: NecessaryHeaders

NOTE: The `NServiceBus.EnclosedMessageTypes` property must contain the the fully-qualified name of the type expected by the NServiceBus endpoint.

The message itself is defined using [conventions](/nservicebus/messaging/conventions.md) in the `Receiver` project.

NOTE: To specify a message ID different from the underlying transport message ID (`Message.MessageId`), set the `NServiceBus.MessageId` header on the native message with the desired message ID.

snippet: NativeMessage


## Handling messages from a native sender in an NServiceBus endpoint

Once the message is received by the NServiceBus endpoint, its content will be presented.

snippet: NativeMessageHandler


## Things to note

 * The use of the `AzureServiceBus_ConnectionString` environment variable mentioned above.
 * Execute `Receiver` first to create the destination queue that `NativeSender` will need to send native messages.
