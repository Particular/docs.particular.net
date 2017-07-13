WARNING: Storing subscriptions in MSMQ **must not** be used when scaling out across multiple machines or running side-by-side on the same machine. The subscription queue cannot be shared among multiple endpoints instances for the same endpoint.

To configure MSMQ as the subscription persistence:

snippet: ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `{Name of the endpoint}.Subscriptions`. In order to specify a different queue to store the subscriptions, add the following `configSections` and subsequent config entry:

snippet: MsmqSubscriptionAppConfig