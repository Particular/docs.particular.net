## Message driven pub/sub compatibility mode

To gradually move an existing system from message driven pub/sub to native pub/sub using SNS, it's possible to enable message-driven pub/sub compatibility mode.

Message-driven pub/sub compatibility mode must be enabled on publisher endpoints. When enabled, publishers will still consume subscription messages sent by endpoints using message-driven pub/sub, and when publishing an event, it will be published both to legacy subscribers and to SNS. Publishers deduplicate published events.

To enable message-driven Pub/Sub compatibility mode, configure the endpoint as follows:

snippet: EnableMessageDrivenPubSubCompatibilityMode

### Subscription cache configuration

The default (5 seconds) value of SNS topic subscription cache invalidation can be changed using:

snippet: SubscriptionsCacheTTL

### Topic cache configuration

The default (5 seconds) value of SNS topic cache invalidation can be changed using:

snippet: TopicCacheTTL

### Message visibility timeout

The default (30 seconds) message visibility timeout can be changed using:

snippet: MessageVisibilityTimemout