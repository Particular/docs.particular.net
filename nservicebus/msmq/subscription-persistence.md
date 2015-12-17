---
title: MSMQ Subscription Persistence
summary: Using MSMQ as a subscription persistence
tags: []
---

WARNING: Storing your subscriptions in MSMQ is not suitable for scenarios where you need to scale the endpoint out. The reason is that the subscription queue cannot be shared among multiple endpoints.

To configure MSMQ as your subscription persistence:

snippet:ConfiguringMsmqPersistence

You don't need any configuration changes for this to work, NServiceBus automatically uses a queue called `{Name of your endpoint}.Subscriptions`. However if you want specify the queue used to store the subscriptions yourself, add the following config section and subsequent config entry:


snippet:MsmqSubscriptionAppConfig
