### Publish/Subscribe

The transport is a [Multicast-enabled transport](/transports/types#multicast-enabled-transports) and provides built-in support for [publish-subscribe messaging](/nservicebus/messaging/publish-subscribe/) using Amazon Simple Notification Service (SNS). Publishing events to multiple endpoints is achieved by publishing a single message to an SNS topic to which multiple destination queues are subscribed.

The topology (topics and subscriptions) is created automatically by destination endpoints if [installers](/nservicebus/operations/installers) are enabled. Topology deployment can be automated, or manually created, using the transport CLI tool. Refer to the [transport operations section](/transports/sqs/operations-scripting) for more information.

#### Message inheritance support

By default topic names are generated using the message full type name and replacing characters that are not allowed in SNS. This has an impact on the way inheritance is supported by the transport. By default a subscriber will subscribe only to the most concrete type it knows about and a publisher will always publish the most concrete type it knows about. Inheritance at the subscriber level is not supported when using the automatically created topology.

In case a subscriber needs to subscribe to a message type that is not the most concrete type as seen by the publisher a custom mapping is needed. For example, if a subscriber is subscribed to the `IOrderAccepted` event defined in the `Contracts` assembly it will create and subscribe to a topic named `namespace-IOrderAccepted`. If in the same system the publisher, however, publishes the `OrderAccepted` message, that implements `IOrderAccepted`, from the `Messages` assembly it'll try to publish to the `namespace-OrderAccepted` topic and the message won't be delivered to the desired destination.

For the described inheritance scenario to work properly, a custom mapping must be defined at the subscriber:

snippet: CustomTopicsMappingsTypeToTopicForTopology

The above snippet instructs the subscriber's subscription manager to create and subscribe to a topic named `namespace-OrderAccepted` when subscribing to the `IOrderAccepted` event.

More information about the custom mapping API can be found in the [configuration options](/transports/sqs/configuration-options#custom-topics-mappings?version=sqs_5) documentation.
