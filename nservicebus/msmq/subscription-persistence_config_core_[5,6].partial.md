WARNING: Storing subscriptions in MSMQ is not suitable when endpoints are scaled-out, because the subscription queue cannot be shared among multiple endpoints, especially if the endpoints are running on different machines.

To configure MSMQ as the subscription persistence:

snippet:ConfiguringMsmqPersistence

When using MSMQ Subscription persistence, the default queue to store the subscription messages is `NServiceBus.Subscriptions`. It is important to override this queue name for each endpoint to avoid sharing the same subscription queue for different endpoints running on the same machine. 

To specify a different subscriptions queue, add the following configuration as shown below:

snippet:MsmqSubscriptionAppConfig
