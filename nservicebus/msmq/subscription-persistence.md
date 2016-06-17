---
title: MSMQ Subscription Persistence
summary: Using MSMQ as a subscription persistence
---

WARNING: Storing subscriptions in MSMQ is not suitable when endpoints are scaled-out, becasue the subscription queue cannot be shared among multiple endpoints.

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `{Name of the endpoint}.Subscriptions`. In order to specify a different queue to store the subscriptions, add the following config section and subsequent config entry:

snippet:MsmqSubscriptionAppConfig


## Timeouts persistenace

`MsmqPersistence` provides persistence only for storing event subscriptions. By default NServiceBus also requires a timeout persistence, which is used by Second Level Retries, Saga timeouts and for deferring messages.

If none of these features are used then timeouts can be disabled:

snippet:DisablingTimeoutManagerForMsmqPersistence

NOTE: If timeouts are disabled then features such as Second Level Retries or Saga timeotus can't be used.

Another solutions is to use other available persistences for features different than event subscriptions:

snippet:MsmqPersistenceWithOtherPersisters
