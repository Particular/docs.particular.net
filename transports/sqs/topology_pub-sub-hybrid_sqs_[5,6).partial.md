#### Backwards compatibility

Starting with [version 5](/transports/upgrades/amazonsqs-4to5.md#native-publish-subscribe) the SQS Transport uses SNS for publish-subscribe messaging. Communication with endpoints running on older versions is still possible via publish-subscribe compatibility mode. A publisher running in this mode will publish events both through SNS topics and unicast messaging destined to input queues of the older endpoints.

The compatibility mode requires SNS topology information that is queried by the endpoint using Amazon SQS client SDK. The API can be throttled by AWS - mostly in high-throughput scenarios that occur when many different types of events are being published at the same time. As a result, version 5.4 of the NServiceBus SQS Transport, introduced a caching mechanism that prevents API throttling from interfering with message processing in most of the scenarios. 

The default values of cache invalidation and message visibility timeouts can be changed using publish-subscribe compatibility mode [settings](/transports/sqs/configuration-options.md#message-driven-pubsub-compatibility-mode).