---
title: Azure Service Bus Transport Retry behavior
summary: Describes the relationship between NServiceBus' retry behavior and Azure Service Bus' native retry behavior
tags:
- Cloud
- Azure
- Transport
---

This article describes the relationship between NServiceBus retry behavior and Azure Service Bus native retry behavior.

NServiceBus supports First Level Retries and Second Level Retries features at the endpoint instance level as described in the [recoverability](/nservicebus/recoverability/) article.

Azure Service Bus supports a `MaxDeliveryCount` at the entity level, which defines how many times Azure Service Bus attempts to deliver a message before sending it to the dead letter queue. Refer to [the full configuration API](/nservicebus/azure-service-bus/configuration/full.md#controlling-entities-queues) article to learn how to adjust this setting.

## First Level Retries vs MaxDeliveryCount

In order to implement First Level Retries, NServiceBus maintains an internal counter to track the processing attempts by the endpoint instance for a specific message. If the number of attempts exceeds the `MaxRetries` setting, then the message will be forwarded to the error queue.

Each endpoint's instance maintains its own internal counter for retries. If an endpoint is scaled out then the message can be retried a few times by each instance, before one of them decides to send the message to the error queue. So in effect the global retry count, as seen by the Azure Service Bus entity, will in the worst case scenario be equal to `MaxRetries * number of instances`.

In Azure Service Bus Versions 6 and below the default value for `MaxDeliveryCount` is 6. In Azure Service Bus Versions 7 and higher it has been changed to 10, based on the assumption that by default there are 2 instances of each endpoint and `MaxRetries` has the default value of 5.

### Elastic Scale

The `MaxDeliveryCount` setting becomes a property on the Azure Service Bus entity. It can't be easily modified after the entity has been created. At the same time the number of instances can be changed just by dragging a slider in the Azure administration portal.

This becomes a problem when the number of instances exceeds what was originally planned. The messages can be dead lettered when the total retry value exceeds the value configured in `MaxDeliveryCount`, which was adjusted for a smaller number of instances.

One way to solve this problem is to configure the entity to forward dead letter messages to the error queue:

snippet:forward-deadletter-conditional-queue

## Second Level Retries

The problem with `MaxDeliveryCount` doesn't impact Second Level Retries. Since the message is deferred for a longer period of time, it will result in a resend operation inside the Azure Service Bus transport. That in turn will reset the delivery counter inside the brokered message.