---
title: Native Integration with Azure Service Bus Transport
summary: Consuming messages published by non NServiceBus endpoints.
component: ASB
reviewed: 2018-02-23
related:
 - transports/azure-service-bus
---

## Prerequisites

include: asb-connectionstring

include: asb-transport


## Code walk-through

This sample shows how to send a message from non-NServiceBus code using the Azure Service Bus API and process it with an NServiceBus endpoint using the Azure Service Bus transport.

The sample contains two executable projects:

 * `NativeSender` - sends a native `BrokeredMessage` messages to a queue.
 * `Receiver` - NServiceBus endpoint that processes messages sent by `NativeSender`.


## Sending messages with native Azure Service Bus API

Configuring the native sender to send messages to the queue used by the receiving endpoint is required when integrating the Azure Service Bus sender with NServiceBus endpoints. By default, the input queue for an NServiceBus endpoint is its endpoint name.

snippet: EndpointAndSingleQueue

The native sender is using queue client to send a `BrokeredMessage`.


## Message serialization

The Azure Service Bus transport is using the JSON serializer by default. Therefore, the message sent by a native sender needs to be valid JSON.

snippet: SerializedMessage

To generate a serialized message, the `MessageGenerator` project can be used with the unit test named `Generate` under the `SerializedMessageGenerator` test fixture.


## BrokeredMessage body format

The Azure Service Bus API allows the construction of a `BrokeredMessage` body from a stream or an object that will get serialized by the internals of `BrokeredMessage`.

NOTE: Both the sender (native or NServiceBus) and the receiver must agree on the convention used for sending the message body.


## Required headers

For a native message to be processed, NServiceBus endpoints using the Azure Service Bus transport require the following headers:

 1. Message type
 1. BrokeredMessage body type
 1. Message intent

These headers need to be stored as `BrokeredMessage` properties.

snippet: NecessaryHeaders

NOTE: The `NServiceBus.EnclosedMessageTypes` property must contain the message type expected by the NServiceBus endpoint. Message type should include namespace it's contained in.

The message itself is defined as an `IMessage` in the `Shared` project.

NOTE: To specify a message ID different from the underlying transport message ID (`BrokeredMessage.MessageId`), set the `NServiceBus.MessageId` header on the native message with the desired message ID.

snippet: NativeMessage


## NServiceBus receiving endpoint

The receiver is defining how to get the Azure Service Bus transport message body by specifying a strategy using `BrokeredMessageBodyConversion`.

snippet: BrokeredMessageConvention

NOTE: Both the sender (native or NServiceBus) and the receiver have to agree on the convention used for sending the message body.


## Handling messages from native sender in an NServiceBus endpoint

Once the message is received by the NServiceBus endpoint, its contents will be presented.

snippet: NativeMessageHandler


## Things to note

 * The use of the `AzureServiceBus.ConnectionString` environment variable mentioned above.
 * The use of `UseSingleBrokerQueue` prevents the Azure Service Bus transport individualizing queue names by appending the machine name.
 * Execute `Receiver` first to create destination queue `NativeSender` will need to send native messages.
