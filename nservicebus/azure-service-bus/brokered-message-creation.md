---
title: Azure Service Bus Transport Brokered Message Creation
tags:
- Azure
- Cloud
---

This document describes how to modify NServiceBus endpoints in order to set up a native integration between these endpoints and non-NServiceBus endpoints that share an Azure Service Bus namespaces as mutual communication channel.

### BrokeredMessage conventions

#### Brokered Message body format

The Azure Service Bus API allows the construction of a `BrokeredMessage` body from a stream or an object that will get serialized into a byte array by the internals of `BrokeredMessage`.

NOTE: Both the sender (native or NServiceBus) and the receiver must agree on the convention used for sending the message body.

By default the `BrokeredMessage` body is assumed to be a byte array. But for scenarios such as native integration it is usually more appropriate that the body can be retrieved using `Stream` as most non .NET ASB SDK's only support the `Stream` body format. `Stream` does not perform any serialization on the body, so it stays in its raw form on the wire.

To specify how the `BrokeredMessage` body is stored and retrieved, override the default conventions by using `BrokeredMessageBodyConversion` class.

Outgoing message:

snippet: ASB-outgoing-message-convention

Incoming message:

snippet: ASB-incoming-message-convention

#### Brokered Message content serialization

The Azure Service Bus transport is using the JSON serializer by default. Therefore, the message sent by a native sender needs to be valid JSON.

``` json
{ Content: "Hello from native sender", SentOnUtc: "2015-10-27T20:47:27.4682716Z" }
```

If the application usages a different serialization for message content, then the serializer can be configured using the following configuration API.

snippet: asb-serializer

#### Required Headers

For a native message to be processed, NServiceBus endpoints using the Azure Service Bus transport require the following headers. These headers need to be stored as `BrokeredMessage` properties.

partial: headers

NOTE: The `NServiceBus.EnclosedMessageTypes` property must contain the message type expected by the NServiceBus endpoint. Message type should include namespace it's contained in.

In native integration scenarios it is not always possible, or desireable, to modify the headers of the brokered message at the sending end. If this is the case, the receiving end can also add the required headers to the message by registering a behavior in the pipeline.

TODO: add snippet that shows this
