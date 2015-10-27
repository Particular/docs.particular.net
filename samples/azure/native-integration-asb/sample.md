---
title: Native Integration with Azure Service Bus Transport
summary: Shows how to consume messages published by non NServiceBus endpoints
tags:
related:
- nservicebus/azure/azure-servicebus-transport
---

## Prerequisites 

An environment variable named `AzureServiceBus.ConnectionString` that contains the connection string for the Azure Service Bus namespace.


## Azure Service Bus Transport

This sample utilizes the [Azure Service Bus Transport](/nservicebus/azure/azure-servicebus-transport.md).


## Code walk-through

This sample shows how to send a message from non-NServicebus code using Azure Service Bus API and process it with NServiceBus endpoint using Azure Service Bus transport.

Sample contains 2 executable projects:

- NativeSender - sends a native `BrokeredMessage` message to a queue
- Receiver - NServiceBus endpoint that processes messages

## Sending message with native Azure Service Bus API

To integrate native Azure Service Bus sender with NServiceBus endpoints you need to configure native sender to send messages to the queue used by receiving endpoint. By default, input queue for NServiceBus endpoint is its endpoint name.

<!-- import EndpointAndSingleQueue -->

Native sender is using queue client to send a `BrokeredMessage`.

## Message serialization

Azure Service Bus transport is using JSON serializer by default. Therefore message sent by native sender needs to be a valid JSON.

<!-- import SerializedMessage -->

To generate a serialized message, `MessageGenerator` project can be used with unit test `Generate` under `SerializedMessageGenerator` test fixture.

## BrokeredMessage body format

Azure Service Bus API allows to construct `BrokeredMessage` body from a stream or an object that will get serialized by internals of `BrokeredMessage`. 

NOTE: both sender (native or NServiceBus) and receiver have to agree on the convention used for sending the body.

## Message properties

In order for native message to be processed, NServiceBus endpoint using Azure Service Bus transport requires 2 pieces of information

1. Message type
2. Message intent

Message properties applied on `BrokeredMessage` instance directly before it's sent out.

<!-- import NecessaryHeaders -->

NOTE: `NServiceBus.EnclosedMessageTypes` property must contain the message expected by NServiceBus endpoint

Message itself is define as an `IMessage` under `Shared` project


<!-- import NativeMessage -->

## NServiceBus receiving endpoint

Receiver is defining how to get Azure Service Bus transport message body by specifying strategy `BrokeredMessageBodyConversion`

<!-- import BrokeredMessageConvention -->

NOTE: both sender (native or NServiceBus) and receiver have to agree on the convention used for sending the body.

## Handling message from native sender in NServiceBus endpoint

Once message is received by NServiceBus endpoint, it will write the contents of the message to the screen.

<!-- import NativeMessageHandler -->


Things to note:

 * The use of the `AzureServiceBus.ConnectionString` environment variable mention above.
 * The use of `UseSingleBrokerQueue` prevents the Azure transport individualizing queue names by appending the machine name.  
 * Execute `Receiver` first to create destination queue `NativeSender` will need