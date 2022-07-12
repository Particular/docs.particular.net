---
title: Brokered Message Creation
summary: Details on brokered message creation with Azure Service Bus
component: ASB
versions: '[6,)'
reviewed: 2020-01-11
related:
 - nservicebus/messaging/third-party-integration
 - nservicebus/messaging/headers
redirects:
 - nservicebus/azure-service-bus/brokered-message-creation
 - transports/azure-service-bus/brokered-message-creation
---

include: legacy-asb-warning

This document describes how to modify brokered message creation logic in order to set up a native integration between NServiceBus and non-NServiceBus endpoints.


## BrokeredMessage conventions


### Brokered Message body format

The Azure Service Bus API allows the construction of a [`BrokeredMessage`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.servicebus.messaging.brokeredmessage) body from a stream or an object that will then be serialized into a byte array.

NOTE: Both the sender (native or NServiceBus) and the receiver must have a consistent convention for the communication the message body.

By default, the `BrokeredMessage` body is assumed to be a byte array to remain backward compatible with previous versions of the transport. It is recommended that the body can be retrieved using `Stream` for new implementations, as it incurs less of a memory overhead during serialization. It is especially useful for scenarios, such as native integration, as most non .NET ASB SDK's only support the `Stream` body format.

NOTE: The `SupportedBrokeredMessageBodyTypes.Stream` body format is recommended, but this mode is currently NOT compatible with ServiceControl. A [ServiceControl transport adapter](/servicecontrol/transport-adapter/) is required in order to leverage both.

partial: converter

partial: bodyformat

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


### Message type detection

The native message must allow NServiceBus to [detect the message type either via the headers or the message payload](/nservicebus/messaging/message-type-detection.md).