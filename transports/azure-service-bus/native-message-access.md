---
title: Native message access
component: ASBS
versions: '[1.4.0,]'
reviewed: 2019-12-02
---

This document describes how to access native message information with Azure Service Bus transport version 1.4.0 and above.

## Access to the native Azure Service Bus message details

It can sometimes be useful to access the native Service Bus message from behaviors and handlers. When a message is received, the transport adds the native Service Bus [`Message`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.servicebus.message) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: access-native-message

The behavior above uses the native message's `LockedUntilUtc` system property to determine wherever the message has lost its lock as a result of aggressive prefetching and slow processing. The message with the lost lock will not go through recoverability, skipping attempted processing that otherwise would be guaranteed to fail due to the message's lost lock.