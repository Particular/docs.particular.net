---
title: Native message access
component: ASBS
versions: '[1.4.0,)'
reviewed: 2020-11-23
---

This document describes how to access native message information with Azure Service Bus transport.
- Incoming message access is available from version 1.4.0 and above.
- Outgoing message access is available from version 1.7.0 and above.

## Access to the native Azure Service Bus incoming message

It can sometimes be useful to access the native Service Bus incoming message from behaviors and handlers. When a message is received, the transport adds the native Service Bus [`Message`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.servicebus.message) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: access-native-incoming-message

The behavior above uses the native message's `LockedUntilUtc` system property to determine wherever the message has lost its lock as a result of aggressive prefetching and slow processing. The message with the lost lock will not go through recoverability, skipping attempted processing that otherwise would be guaranteed to fail due to the message's lost lock.

## Access to the native Azure Service Bus outgoing message

It can also be useful to access the native Service Bus outgoing message from behaviors and handlers for customizations. 

Customizing an outgoing message from a message handler:

snippet: access-native-outgoing-message-from-handler

Customizing an outgoing message using `IMessageSession`:

snippet: access-native-outgoing-message-with-messagesession

Customizing an outgoing message from a physical behavior:

snippet: access-native-outgoing-message-from-physical-behavior

Customizing an outgoing message from a logical behavior:

snippet: access-native-outgoing-message-from-logical-behavior