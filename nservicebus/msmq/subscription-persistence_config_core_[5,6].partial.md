WARNING: Storing subscriptions in MSMQ is not suitable when endpoints are scaled-out, because the subscription queue cannot be shared among multiple endpoints.

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `NServiceBus.Subscriptions`. If not specified otherwise, all endpoints will share that queue to store subscriptions. In order to specify a different queue, add the following `configSections` and subsequent config entry:

snippet:MsmqSubscriptionAppConfig

WARNING: When using MSMQ Subscription Persistence on multiple endpoints running on the same machine, every endpoint requires a dedicated subscription storage queue.
