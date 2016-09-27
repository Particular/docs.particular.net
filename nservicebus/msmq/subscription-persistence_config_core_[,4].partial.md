WARNING: Storing subscriptions in MSMQ is not suitable when endpoints are scaled-out, because the subscription queue cannot be shared among multiple endpoints.s

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `{Name of the endpoint}.Subscriptions`. In order to specify a different queue to store the subscriptions, add the following `configSections` and subsequent config entry:

snippet:MsmqSubscriptionAppConfig