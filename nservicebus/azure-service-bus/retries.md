---
title: Retry behavior
summary: Describes the relationship between the NServiceBus retry behavior and the Azure Service Bus native retry behavior
tags:
- Cloud
- Azure
- Transport
reviewed: 2016-09-22
---

This article describes the relationship between [recoverability behavior](/nservicebus/recoverability/) and the Azure Service Bus native retry behavior.

Azure Service Bus supports a `MaxDeliveryCount` at the entity level, which defines how many times Azure Service Bus attempts to deliver a message before sending it to the dead letter queue. Refer to [the full configuration API](/nservicebus/azure-service-bus/configuration/full.md#controlling-entities-queues) article to learn how to adjust this setting.


## Immediate Retries and MaxDeliveryCount

To implement Immediate Retries, the native Azure Service Bus `MaxDeliveryCount` property of the `BrokeredMessage` is used to track the processing attempts by the endpoint instances for a specific message. If the number of attempts exceeds the `MaxRetries` setting, then the message will be forwarded to the delayed retries if configured, and eventually to at the error queue in case error persists. In case Immediate Retries are disabled, the transport will default `MaxDeliveryCount` to 10 attempts.

A scaled out endpoint will retry the message by any of its instances. Once all Immediate Retries where exhausted, the message will be delegated to the Delayed Retries, if configured, or sent the error queue. So in effect, the number of immediate retries attempts will never exceed the defined number of immediate retries for an endpoint.

Versions 6 and below the default value for `MaxDeliveryCount` is 6. Versions 7 and higher it has been changed to be equal to a number of immediate retries for an endpoint plus one. For system queues, such as audit and error queues, the default was changed to 10. Should endpoint disable Immediate Retries, `MaxDeliverCount` will be set to 10.


### Keeping MaxDeliveryCount in sync with immediate retries

Whenever an endpoint is redeployed with a new Immediate Retries value, endpoint specific queues, such as endpoint`s input queue, will be updated with the new value.


For system queues, such as audit and error queues, a newly deployed or a re-deployed endpoint will not modify system queues `MaxDeliveryCount` set by end users.


### Excessive prefetching and MaxDeliveryCount 

When a message is received for processing, the transport will also [prefetch](/nservicebus/azure-service-bus/configuration/full.md#controlling-connectivity-message-receivers) additional messages. "If processing a message takes too long time, then the broker will consider that a failed processing attempt and increase `DeliveryCount` counter on the messages w/o processing attempt taking place.  

Endpoints with high `PrefetchCount` that take long time to process an incoming message can cause prefetched message to be sent to the Delayed Retries or the error queue without being retried the desired number of times. To prevent this issue, lower the `PrefetchCount`.

See [Message Receivers configuration](/nservicebus/azure-service-bus/configuration/full.md#controlling-connectivity-message-receivers) for more details.


## Retries and dead letter queues

A valid message failing processing will be retried and eventually end up in the error queue. If a message is poisonous or cannot be moved to an error queue, it will be moved by the broker to a dead letter queue. Each endpoint has its own DLQ. Monitoring multiple DLQs might be challenging. One way to solve this problem is to configure the entity to forward dead letter messages to the error queue:

snippet:forward-deadletter-conditional-queue


## Delayed Retries

Delayed Retries are not affected by `MaxDeliveryCount` since the message is deferred for a longer period, it will result in a resend operation inside the Azure Service Bus transport. That, in turn, will reset the delivery counter inside the brokered message.
