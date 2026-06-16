## Message-driven publish/subscribe compatibility mode

To gradually migrate an existing system from message-driven publish/subscribe to native publish/subscribe using SNS, it's possible to enable message-driven publish/subscribe compatibility mode.

Message-driven pub/sub compatibility mode must be enabled on publisher endpoints. When enabled, publishers will still consume subscription messages sent by endpoints via message-driven publish/subscribe, and when publishing an event, it will be sent to both legacy subscribers and SNS. Publishers deduplicate published events.

To enable message-driven publish/subscribe compatibility mode, configure the endpoint as follows:

snippet: EnableMessageDrivenPubSubCompatibilityMode

### Subscription cache configuration

The default value for SNS topic subscription cache invalidation (5 seconds) can be changed using:

snippet: SubscriptionsCacheTTL

### Topic cache configuration

The default value for SNS topic cache invalidation (5 seconds) can be changed using:

snippet: TopicCacheTTL

### Message visibility timeout

The default value for the message visibility timeout setting (30 seconds) can be changed using:

snippet: MessageVisibilityTimeout