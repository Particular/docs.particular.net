---
title: Azure Service Bus Transport Retry behaviour
summary: Describes the relationship between NServiceBus' retry behaviour and Azure Service Bus' native retry behaviour
component: ASB
tags:
- Cloud
- Azure
- Transports
---

This article describes the relationship between NServiceBus' retry behavior and Azure Service Bus' native retry behaviour.

NServiceBus supports First Level Retries and Second Level Retries features at the endpoint instance level as described in the [Automatic Retries](/nservicebus/errors/automatic-retries.md) article.

Azure Service Bus supports a `MaxDeliveryCount` at the entity level, which defines how many times Azure ServiceBus attempts to deliver a message before sending it to the dead letter queue. Refer to [the full configuration API](/nservicebus/azure-service-bus/configuration/configuration.md#controlling-entities-queues) to learn how to change this settings.

## First Level Retries vs MaxDeliveryCount

In order to implement First Level Retries, NServiceBus maintains an internal counter to track the processing attempts by the endpoint instance for a specific message. If the number of attempts exceeds the `MaxRetries` setting, then the message will be forwarded to the error queue.

If an endpoint is scaled out, this implies that each instance has it's own internal counter and they can, collectively, attempt to reprocess the message in an alternating way before one of the instances decides to send the message to the error queue. So in effect the global retry count, as seen by the Azure Service Bus entity, will potentially increment up to `MaxRetries * number of instances`.

As from version 7 of the transport, the default value for `MaxDeliveryCount` has been set to 10 (it used to be 6), assuming that there are 2 instances of each endpoint with a default `MaxRetries` setting of 5.

### Elastic Scale

The `MaxDeliveryCount` setting becomes a property on the Azure Service Bus entity and can therefore not easily be changed after creation of the entity. While the number of instances can often be changed just by dragging a slider in the Azure administration portal. This poses a problem when the number of instances is scaled further than planned, resulting in messages potentially being dead lettered when the total retry value exceeds the value configured in `MaxDeliveryCount`.

One way to deal with this problem is to configure the entity to forward dead letter messages to the error queue. This can be done the following way:

snippet:forward-deadletter-conditional-queue

## Second Level Retries

Second level retries does not pose this problem. As it actively defers a message for a longer period of time this will result in a resend operation inside the Azure Service Bus transport, effectively resetting the delivery counter inside the brokered message.