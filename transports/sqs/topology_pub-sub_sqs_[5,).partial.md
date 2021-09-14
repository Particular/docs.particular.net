### Publish/Subscribe

The transport is a [multicast-enabled transport](/transports/types.md#multicast-enabled-transports) and provides built-in support for [publish-subscribe messaging](/nservicebus/messaging/publish-subscribe/) using Amazon Simple Notification Service (SNS). Publishing events to multiple endpoints is achieved by publishing a single message to an SNS topic to which multiple destination queues are subscribed.

The topology (topics and subscriptions) is created automatically by the subscribing endpoints. Topology deployment can be automated, or manually created, using the transport CLI tool. Refer to the [transport operations section](/transports/sqs/operations-scripting.md) for more information.

#### Backwards compatibility

Native support for SNS was [introduced in version 5](/transports/upgrades/amazonsqs-4to5.md#native-publish-subscribe) of the Amazon SQS transport. In able to upgrade from message-driven publish-subscribe to SNS a backwards compatibility mode was introduced. This compatibility mode requires usage of the Amazon SQS client SDK which can be throttled by AWS in specific scenarios; mostly high-throughput scenarios with many different types of events that are being published at the same time. Since version 5.4 of the NServiceBus SQS Transport, it is by default able to deal with most of these scenarios. In the case NServiceBus reports warnings about excessive rate limiting, there is the option to modify [the default settings](/transports/sqs/configuration-options.md#message-driven-pubsub-compatibility-mode).

The topic cache and subscription cache can be extended to prevent additional rate limiting. The message visibility timeout can be extended if messages are returned to the queue before NServiceBus had the change to process the message and publish new messages.

#### Message inheritance support

By default topic names are generated using the message full type name and replacing characters that are not allowed in SNS. This has an impact on the way inheritance is supported by the transport. By default a subscriber will subscribe only to the most concrete type it knows about and a publisher will always publish the most concrete type it knows about. Inheritance at the subscriber level is not supported when using the automatically created topology.

In case a subscriber needs to subscribe to a message type that is not the most concrete type as seen by the publisher, a custom mapping is needed. For example, if a subscriber is subscribed to the `IOrderAccepted` event defined in the `Contracts` assembly it will create and subscribe to a topic named `namespace-IOrderAccepted`. However, if in the same system the publisher publishes the `OrderAccepted` message that implements `IOrderAccepted` from the `Messages` assembly it'll try to publish to the `namespace-OrderAccepted` topic and the message won't be delivered to the desired destination.

For the described inheritance scenario to work properly, a custom mapping must be defined at the subscriber:

snippet: CustomTopicsMappingsTypeToTopicForTopology

The above snippet instructs the subscriber's subscription manager to create and subscribe to a topic named `namespace-OrderAccepted` when subscribing to the `IOrderAccepted` event.

More information about the custom mapping API can be found in the [configuration options](/transports/sqs/configuration-options.md?version=sqs_5#custom-topics-mappings) documentation.

#### Evolving an existing topology

Evolving an existing topology needs to be handled with care. The SQS Transport never deletes any resource so it might happen that messages are routed to undesirable destinations if or when message handlers are moved to a different endpoint, or completely removed.

For instance, in the following scenario:

- The system is composed by a publisher and a scaled-out subscribers (two instances, for the sake of the sample)
- An event needs to be removed as not anymore needed
- The message handler, for the removed event, needs to be removed with no subscribers downtime

The following steps needs to be put in place:

1. Change the publisher first to stop publishing the event for which the handler needs to be removed
1. Deploy the publisher changes
1. Wait for in-flight messages to be consumed
1. Change the subscriber by removing the not anymore needed handler
1. Deploy subscribers instances one-by-one with no downtime
1. Remove the SNS to SQS subscription for the specific event in AWS
1. If not anymore needed, remove the SNS topic for the specific event
