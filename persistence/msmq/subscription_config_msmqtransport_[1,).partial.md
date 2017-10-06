To configure MSMQ as the subscription persistence:

snippet: ConfiguringMsmqPersistence

If the subscription storage is not specified, NServiceBus uses a queue called `[Endpoint].Subscriptions`. This is to avoid all endpoints deployed on the same server from having to share the same queue to store subscriptions. When a subscription queue is not specified and the default queue is being used, the following message will get logged:

>The queue used to store subscriptions has not been configured, the default '{EndpointName}.Subscriptions' will be used.

WARNING: When using MSMQ Subscription Persistence on multiple endpoints running on the same machine, every endpoint **must have** a dedicated subscription storage queue. 

As some older versions of NServiceBus, specifically Versions 5.x and Versions 6.x) used a default queue called `NServiceBus.Subscriptions`, in order to avoid message loss, an exception will be thrown on startup asking to either specify the subscription queue or to [migrate the subscription messages to the new default queue]().

In order to specify a different queue, use the code api or specify a configuration section.


### Via Code

Call the following code API passing the subscription queue to use:

snippet: MsmqSubscriptionCode


### Via App.config

Add the following `configSections` and subsequent config entry:

snippet: MsmqSubscriptionAppConfig