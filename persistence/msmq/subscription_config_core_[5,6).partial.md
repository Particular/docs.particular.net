WARNING: Storing subscriptions in MSMQ **must not** be used when scaling out across multiple machines or running side-by-side on the same machine. The subscription queue cannot be shared among multiple endpoints instances for the same endpoint.

To configure MSMQ as the subscription persistence:

snippet: ConfiguringMsmqPersistence

When using MSMQ subscription persistence, the default queue to store the subscription messages is `NServiceBus.Subscriptions`. It is important to override this queue name for each endpoint to avoid sharing the same subscription queue for different endpoints running on the same machine. 

To specify a different subscriptions queue, add the following configuration as shown below:

snippet: MsmqSubscriptionAppConfig
