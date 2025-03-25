In NServiceBus.AmazonSQS version 7.3 and higher, the unrestricted delayed delivery feature is enabled by default. It can be disabled by creating the transport instance using the corresponding contructor accepting a boolean `disableDelayedDelivery` and setting it to `false`.

> [!NOTE]
> When unrestricted delayed delivery is disabled, delayed deliveries will still work up to a delay of 15 minutes.
