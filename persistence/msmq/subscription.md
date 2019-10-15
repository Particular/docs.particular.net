---
title: MSMQ Subscription Persistence
summary: How to configure the MQMQ persistence for subscriptions
component: MsmqPersistence
reviewed: 2019-06-18
related:
 - samples/msmq/persistence
---

WARN: MSMQ Persistence does not support scaled-out publishers. This is because the MSMQ storage is local to the machine, and a subscription message will only be delivered to one endpoint instance of a given logical endpoint group, and only that instance will be able to update its information, while other instances remain unaware of the new subscriber. 

Note: The subscription queue can contain duplicate entries. This is by design and does not result in events being published multiple times. Subscription entries are added for each subscription request received. After a publisher restarts, the subscription queue state will be rewritten and deduplicated.

partial: config


partial: timeouts
