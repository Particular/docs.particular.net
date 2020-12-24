---
title: Retry behavior
summary: Describes the relationship between the NServiceBus retry behavior and the Azure Service Bus native retry behavior
component: ASB
versions: '[7,)'
reviewed: 2020-12-24
redirects:
 - nservicebus/azure-service-bus/retries
 - transports/azure-service-bus/retries
---

include: legacy-asb-warning

This article describes the relationship between [recoverability behavior](/nservicebus/recoverability/) and the Azure Service Bus native retry behavior.

Azure Service Bus supports a `MaxDeliveryCount` at the entity level, which defines how many times Azure Service Bus attempts to deliver a message before sending it to the dead letter queue. Refer to [the full configuration API](/transports/azure-service-bus/legacy/configuration/full.md#controlling-entities-queues) article to learn how to adjust this setting.


## Immediate Retries and MaxDeliveryCount

To implement Immediate Retries, the native Azure Service Bus `MaxDeliveryCount` property of the `BrokeredMessage` is used to track the processing attempts by the endpoint instances for a specific message. If the number of attempts exceeds the `MaxRetries` setting, then the message will be forwarded to the delayed retries if configured, and eventually to to the error queue in case error persists. If immediate retries are disabled for the endpoint, the transport will default `MaxDeliveryCount` to 10 attempts.

A scaled out endpoint will retry the message by any of its instances. If the endpoint has not successfully processed the message after all the configured number of Immediate Retry attempts, then the message will be forwarded to the Delayed Retries if configured and if the problem still persists, then the message will be forwarded to the configured error queue. So in effect, the number of immediate retries attempts will never exceed the defined number of immediate retries for an endpoint.

In versions 6 and below, the default value for `MaxDeliveryCount` is 6. Starting from Versions 7 and above, the new defaults for `MaxDeliveryCount` is one more than the configured value of Immediate Retries. The default value `MaxDeliveryCount` for system queues such as audit and error queues is 10. If the endpoint disables the `Immediate Retries` explicitly, then the `MaxDeliveryCount` will also be set to 10.


### Keeping MaxDeliveryCount in sync with immediate retries

Whenever an endpoint is redeployed with a new `Immediate Retries` value, endpoint specific queues, such as endpoint's input queue, will be updated with the new value.


For system queues, such as audit and error queues, if a custom value was configured for `MaxDeliveryCount` redeploying the endpoint will not overwrite the already configured value.


### Excessive prefetching and MaxDeliveryCount 

When a message is received for processing, the transport will also [prefetch](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-message-receivers) additional messages. If processing a message takes too long time, then the broker will consider that a failed processing attempt and increase `DeliveryCount` counter on the messages w/o processing attempt taking place.  

Endpoints with high `PrefetchCount` that take long time to process an incoming message can cause prefetched message to be sent to the `Delayed Retries` or the error queue without being retried the desired number of times. To prevent this issue, lower the `PrefetchCount`.

See [Message Receivers configuration](/transports/azure-service-bus/legacy/configuration/full.md#controlling-connectivity-message-receivers) for more details.


## Retries and dead letter queues

If a message could not be successfully processed even after retries, it will eventually end up in the error queue. If a message is poisonous or cannot be moved to an error queue, it will be moved by the broker to the Dead Letter Queue (DLQ). 

Each endpoint has its own DLQ, however monitoring multiple DLQs might be challenging. One way to solve this problem is to configure the entity to forward dead letter messages to the error queue:

snippet: forward-deadletter-conditional-queue


## Delayed Retries

Delayed Retries are not affected by `MaxDeliveryCount` since the message is deferred for a longer period, it will result in a resend operation inside the Azure Service Bus transport. That, in turn, will reset the delivery counter inside the brokered message.
