---
title: MSMQ Persistence
summary: MSMQ Subscription Persistence for NServiceBus
component: MSMQPersistence
reviewed: 2017-12-01
---

The MSMQ Subscription storage can be used to enable publish/subscribe with MSMQ without the need for any additional persister.

NOTE: This can only be used to store subscriptions. To use sagas, timeouts, deferred messages, or the Outbox, a different persister is required.
