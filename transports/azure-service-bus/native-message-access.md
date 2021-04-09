---
title: Native message access
summary: Access native message information with the Azure Service Bus transport
component: ASBS
versions: '[1.4.0,)'
reviewed: 2020-11-23
---

partial: supported-versions


## Access to the native Azure Service Bus incoming message

It can sometimes be useful to access the native Service Bus incoming message from behaviors and handlers. When a message is received, the transport adds the native Service Bus [`Message`](https://docs.microsoft.com/en-us/dotnet/api/microsoft.azure.servicebus.message) to the message processing context. Use the code below to access the message details from a [pipeline behavior](/nservicebus/pipeline/manipulate-with-behaviors.md):

snippet: access-native-incoming-message

The behavior above uses the native message's `LockedUntilUtc` system property to determine where the message lost its lock as a result of aggressive prefetching and slow processing. If desired, a [custom recoverability policy](/nservicebus/recoverability/custom-recoverability-policy.md) can be used so that the message will skip attempted retry processing that otherwise would be guaranteed to fail due to the message's lost lock.

## Access to the native Azure Service Bus outgoing message

It can also be useful to access the native Service Bus outgoing message from behaviors and handlers for customizations.

partial: snippets

Note: Native outgoing messages cannot be customized when using the [outbox](/nservicebus/outbox/) as customizations are not persistent.
