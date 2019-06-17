---
title: MSMQ Subscription Persistence
summary: How to configure the MQMQ persistence for subscriptions
component: MsmqPersistence
reviewed: 2018-04-06
related:
 - samples/msmq/persistence
---

Note: The subscription queue can contain duplicate entries. This is by design and does not result in events being published multiple times. Subscription entries are added for each subscription request received. After a publisher restarts, the subscription queue state will be rewritten and deduplicated.

partial: config


partial: timeouts
