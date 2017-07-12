To configure MSMQ as the subscription persistence:

snippet: ConfiguringMsmqPersistence

By default NServiceBus uses a queue called `NServiceBus.Subscriptions`. If not specified otherwise, all endpoints will share that queue to store subscriptions. The following warning message will get logged:

> NServiceBus.Features.MsmqSubscriptionPersistence Could not find configuration section for Msmq Subscription Storage and no name was specified for this endpoint. Going to default the subscription queue

WARNING: When using MSMQ Subscription Persistence on multiple endpoints running on the same machine, every endpoint MUST HAVE  a dedicated subscription storage queue.

In order to specify a different queue, use the code api or specify a configuration section.

### Via App.config

Add the following `configSections` and subsequent config entry:

snippet: MsmqSubscriptionAppConfig

### Via Code

Call the following code API passing the subscription queue to use:

snippet: MsmqSubscriptionCode