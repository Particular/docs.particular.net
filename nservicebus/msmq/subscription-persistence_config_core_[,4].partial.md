To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `{Name of the endpoint}.Subscriptions`. In order to specify a different queue to store the subscriptions, add the following `configSections` and subsequent config entry:

snippet:MsmqSubscriptionAppConfig