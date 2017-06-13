---
title: Brokered Message Creation
summary: Details on brokered message creation with Azure Service Bus
component: ASB
versions: '[6,)'
reviewed: 2017-06-13
tags:
 - Azure
related:
 - nservicebus/messaging/third-party-integration
 - nservicebus/messaging/headers
---

This document describes how to modify brokered message creation logic in order to set up a native integration between NServiceBus and non-NServiceBus endpoints.


## BrokeredMessage conventions


### Brokered Message body format

The Azure Service Bus API allows the construction of a [`BrokeredMessage`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.brokeredmessage) body from a stream or an object that will then be serialized into a byte array.

NOTE: Both the sender (native or NServiceBus) and the receiver must have a consistent convention for the communication the message body.

By default, the `BrokeredMessage` body is assumed to be a byte array to remain backward compatible with previous versions of the transport. It is recommended that the body can be retrieved using `Stream` for new implementations, as it incurs less of a memory overhead during serialization. It is especially useful for scenarios, such as native integration, as most non .NET ASB SDK's only support the `Stream` body format.

To specify how the `BrokeredMessage` body is stored and retrieved, override the default conventions.

partial: converter


#### Outgoing message:

snippet: ASB-outgoing-message-convention


#### Incoming message:

snippet: ASB-incoming-message-convention


### Brokered Message content serialization

The Azure Service Bus transport is using the JSON serializer by default. Therefore, the message sent by a native sender needs to be valid JSON.

```json
{
    "Content": "Hello from native sender",
    "SentOnUtc": "2015-10-27T20:47:27.4682716Z" 
}
```

If the application uses a different serialization for message content, then it can be configured as follows.

snippet: asb-serializer

If the message content is in an unsupported or proprietary format, then the application will have to provide a [custom serializer](/nservicebus/serialization/custom-serializer.md)


### Required Headers

For a native message to be processed, NServiceBus endpoints using the Azure Service Bus transport require the following headers. These headers need to be stored as `BrokeredMessage` properties.

partial: header

In native integration scenarios it is not always possible, or desirable, to modify the headers of the brokered message at the sending end. If this is the case, the receiving end can also add the required headers to the message by registering an incoming [Message Mutators](/nservicebus/pipeline/message-mutators.md) or via a [Pipeline Behavior](/nservicebus/pipeline/manipulate-with-behaviors.md).
