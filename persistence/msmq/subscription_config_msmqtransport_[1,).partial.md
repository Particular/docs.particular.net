To configure MSMQ as the subscription persistence:

snippet: ConfiguringMsmqPersistence

If the subscription storage is not specified, NServiceBus uses a queue called `[EndpointName].Subscriptions`. This is to avoid all endpoints deployed on the same server from having to share the same queue to store subscriptions. When a subscription queue is not specified and the default queue is being used, the following message will get logged:

>The queue used to store subscriptions has not been configured, the default 'NServiceBus.Subscriptions' will be used.

WARNING: When using MSMQ subscription persistence on multiple endpoints running on the same machine, every endpoint **must have** a dedicated subscription storage queue. 

Versions 5.x and Versions 6.x of NServiceBus used a default queue called `NServiceBus.Subscriptions`. An exception is thrown on startup if this queue is detected. Either specify the subscription queue explicitly or [move the subscription messages to the new default queue](/nservicebus/upgrades/6to7/moving-msmq-subscriptions.md) to avoid message loss.

In order to specify a different queue, use the code API or specify a configuration section.


### Via code

Call the following code API passing the subscription queue to use:

snippet: MsmqSubscriptionCode


### Via app.config

Add the following `configSections` and subsequent config entry:

snippet: MsmqSubscriptionAppConfig