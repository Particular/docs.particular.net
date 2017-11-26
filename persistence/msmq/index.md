---
title: MSMQ persistence
summary: MSMQ Subscription Persistence for NServiceBus
component: MSMQPersistence
reviewed: 2017-11-16
---

The MSMQ Subscription storage can be used to enable publish/subscribe with MSMQ without the need of any additional persister.

NOTE: This can only be used to store subscriptions. To be able to use sagas, timeouts, deferred messages a different persister is required.

- [How to configure MSMQ Subscription storage](/persistence/msmq/subscription.md).
