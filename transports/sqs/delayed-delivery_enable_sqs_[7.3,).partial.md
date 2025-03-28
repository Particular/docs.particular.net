The unrestricted delayed delivery feature requires both senders and receivers to enable it. In NServiceBus.AmazonSQS version 7.3 and higher, this feature is enabled by default. It can be disabled by creating the transport instance using the corresponding constructor, accepting a boolean `disableDelayedDelivery`, and setting it to `false`.

> [!NOTE]
> When unrestricted delayed delivery is disabled, delayed delivery continues to work up to a delay of 15 minutes.
