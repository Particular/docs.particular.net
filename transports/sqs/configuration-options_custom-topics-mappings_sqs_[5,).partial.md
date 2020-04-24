### Custom topics mappings

The [transport topology](topology.md#sqs-publishsubscribe) describes in depth how the topology is determined by subscribers. There are scenarios in which a custom mapping is needed.

The `MapEvent` transport configuration API can be used to customize the way subscribers determine the topic to subscribe to. If the subscribers has knowledge of both the published event type and the subscribed one, the following API can be used:

snippet: CustomTopicsMappingsTypeToType

NOTE: The types are only used to determine the topic name, subscribers can define dummy empty types to use the strongly typed API shown above.

If the published type is not known at compilation time the following API can be used:

snippet: CustomTopicsMappingsTypeToTopic