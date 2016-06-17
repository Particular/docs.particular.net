---
title: MSMQ Subscription Persistence
summary: Using MSMQ as a subscription persistence
---

WARNING: Storing subscriptions in MSMQ is not suitable for scenarios where scaling out the endpoint is required. The reason is that the subscription queue cannot be shared among multiple endpoints.

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

No configuration changes are required to enable this, NServiceBus automatically uses a queue called `{Name of the endpoint}.Subscriptions`. However to specify the queue used to store the subscriptions, add the following config section and subsequent config entry:

snippet:MsmqSubscriptionAppConfig


## Using MSMQ Subscription Persistence as the only persister

`MsmqPersistence` only provides persistence for subscriptions. NServiceBus also requires a timeout persistence by default which used for Second Level Retries, Saga timeouts and deferred messages. If none of these features are used, it is possible to disable the timeouts which removes the need for a timeout persistence:

snippet:DisablingTimeoutManagerForMsmqPersistence


## Using MSMQ Subscription Persistence with other persisters

To use features which require persistence (e.g. timeouts, Outbox, Sagas and more) together with MSMQ Subscription Persistence it is possible to configure a persistence option for each category:

snippet:MsmqPersistenceWithOtherPersisters


