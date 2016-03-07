---
title: MSMQ Subscription Persistence
summary: Using MSMQ as a subscription persistence
tags: []
---

WARNING: Storing subscriptions in MSMQ is not suitable for scenarios where scaling out the endpoint is required. The reason is that the subscription queue cannot be shared among multiple endpoints.

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

No configuration changes are required to enable this, NServiceBus automatically uses a queue called `{Name of the endpoint}.Subscriptions`. However to specify the queue used to store the subscriptions, add the following config section and subsequent config entry:


snippet:MsmqSubscriptionAppConfig
