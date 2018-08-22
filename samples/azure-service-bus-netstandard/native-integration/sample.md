---
title: Native Integration with Azure Service Bus .NET Standard Transport
summary: Consuming messages published by non-NServiceBus endpoints.
component: ASBS
reviewed: 2018-08-21
related:
 - transports/azure-service-bus-netstandard
---

## Prerequisites

include: asb-connectionstring-xplat


## Code walk-through

This sample shows how to send a message from non-NServiceBus code using the Azure Service Bus API and process it with an NServiceBus endpoint using the Azure Service Bus transport.

The sample contains two executable projects:

 * `NativeSender` - sends native messages to the `Receiver`'s queue.
 * `Receiver` - NServiceBus endpoint that processes messages sent by `NativeSender`.


## Sending messages with the Azure Service Bus API

Configuring the native sender to send messages to the queue used by the receiving endpoint is required when integrating the Azure Service Bus sender with NServiceBus endpoints. By default, the input queue for an NServiceBus endpoint is its endpoint name.

snippet: EndpointName

The native sender is using `QueueClient` to send a single `Message`.


## Message serialization

NServiceBus endpoint is using JSON serialization (NServiceBus.Newtonsoft.Json serializer). Therefore, the message sent by a native sender needs to be a valid JSON.

snippet: SerializedMessage

To generate a serialized message, the `MessageGenerator` project can be used with the unit test named `Generate` under the `SerializedMessageGenerator` test fixture.


## Required headers

For a native message to be processed, NServiceBus endpoints using the Azure Service Bus .NET Standard transport needs the message type to be included as a header. This header needs to be stored as an Azure Service Bus `Message` user property.

snippet: NecessaryHeaders

NOTE: The `NServiceBus.EnclosedMessageTypes` property must contain the the fully qualified name of the type expected by the NServiceBus endpoint.

The message itself is defined as an `IMessage` in the `Shared` project.

NOTE: To specify a message ID different from the underlying transport message ID (`Message.MessageId`), set the `NServiceBus.MessageId` header on the native message with the desired message ID.

snippet: NativeMessage


## Handling messages from a native sender in an NServiceBus endpoint

Once the message is received by the NServiceBus endpoint, its contents will be presented.

snippet: NativeMessageHandler


## Things to note

 * The use of the `AzureServiceBus_ConnectionString` environment variable mentioned above.
 * Execute `Receiver` first to create destination queue `NativeSender` will need to send native messages.
