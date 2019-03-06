---
title: MSMQ Subscription Persistence
summary: How to configure the MQMQ persistence for subscriptions
component: MsmqPersistence
reviewed: 2018-04-06
related:
 - samples/msmq/persistence
---

Note: The subscription queue can contain duplicate entries and is by design and does not result in events to be published multiple times. Subscriptrion entries are added for each subscription request received and after a publisher restarts the subscription queue state will be rewritten and deduplicated.

partial: config


partial: timeouts
