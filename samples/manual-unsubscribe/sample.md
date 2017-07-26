---
title: Manual unsubscribe
summary: Shows how to manually remove subscriptions when subscribers are decommissioned.
reviewed: 2017-07-10
component: Core
related:
- transports
- transports/msmq
- transports/sql
- transports/azure-storage-queues
- transports/sqs
---

This sample shows how to manually remove subscriptions when subscribers are decommissioned. The solution comprises of 4 projects.

NOTE: While this sample uses the [MSMQ transport](/transports/msmq), the concepts shown are valid for all transports based on message driven subscriptions and that don't support native pub/sub. For more information see [Publish-Subscribe](/nservicebus/messaging/publish-subscribe/)


## Subscriber

A sample endpoint subscribed to an event that will be published by `Publisher`:

snippet: event-handler


## Publisher


### The publisher configuration

snippet: publisher-config

This specific sample is configured to use [MSMQ Subscription Persistence](/persistence/msmq/subscription.md).


### Subscriber decommissioning

In such a configuration, when a subscriber endpoint is decommissioned it may happen that subscriptions remain stored at the publisher. Publishers have no way to detect that a subscriber is no longer available. They will continue to publish events even for subscribers that no longer exist. This behavior will eventually lead to storage and quota issues.

To remove a subscription a message like the following one can be sent to the publisher:

snippet: unsubscribe-message


### Unsubscribe process

A publisher handle the `ManualUnsubscribe` message and perform the following task:

snippet: unsubscribe-handling

The message handler relies on the `ISubscriptionStorage` NServiceBus abstraction to perform the unsubscribe request regardless of the subscription storage configured for the publisher.


## SubscriptionManager

`SubscriptionManager` is a sample endpoint instance that can be used by operations personnel to send unsubscribe requests to publishers whenever a subscriber is decommissioned:

snippet: SubscriptionManager-config


## Messages

Stores the messages and events used by this sample.
