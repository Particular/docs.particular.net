---
title: MSMQ Subscription Persistence
summary: How to configure the MQMQ persistence for subscriptions
component: MSMQPersistence
reviewed: 2019-09-26
related:
 - samples/msmq/persistence
redirects:
 - persistence/msmq/subscription
---

The MSMQ Subscription storage can be used to enable publish/subscribe with MSMQ without the need for any additional persister.

NOTE: This can only be used to store subscriptions. To use sagas, timeouts, deferred messages, or the Outbox, a different persister is required.

## Persistence at a glance

For a description of each feature, see the [persistence at a glance legend](/persistence/#persistence-at-a-glance).

|Feature                    |   |
|:---                       |---
|Supported storage types    |Subscriptions only
|Transactions               |Does not apply to subscriptions.
|Concurrency control        |Does not apply to subscriptions.
|Scripted deployment        |Not supported
|Installers                 |Subscription queues are created by installers.

## Configuration

WARN: MSMQ Persistence does not support scaled-out publishers. This is because the MSMQ storage is local to the machine, and a subscription message will only be delivered to one endpoint instance of a given logical endpoint, and only that instance will be able to update its information, while other instances remain unaware of the new subscriber.

Note: The subscription queue can contain duplicate entries. This is by design and does not result in events being published multiple times. Subscription entries are added for each subscription request received. After a publisher restarts, the subscription queue state will be rewritten and deduplicated.

partial: config


partial: timeouts