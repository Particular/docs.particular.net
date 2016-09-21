To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `NServiceBus.Subscriptions`. In order to specify a different queue to store the subscriptions, add the following `configSections` and subsequent config entry:

snippet:MsmqSubscriptionAppConfig

WARNING: When using MSMQ Subscription Persistence on multiple endpoints running on the same machine, every endpoint requires it's own dedicated subscription storage queue.