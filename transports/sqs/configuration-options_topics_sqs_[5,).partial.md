## TopicNamePrefix

**Optional**

**Default**: None

This string value is prepended to the name of every SNS topic subscribed by the endpoint. This is useful when deploying many instances of the same application in the same AWS region (e.g. a development instance, a QA instance, and a production instance), and the topic names must be distinguished from each other.

**Example**: For a development instance, specify:

snippet: TopicNamePrefix

For example, topic names for the topic called "MyNameSpace.MyEvent" might be:

```
DEV-MyNameSpace-MyEvent
```

## TopicNameGenerator

**Optional**

**Default**: `$"{topicNamePrefix}{eventType.FullName}` with unsupported characters like `.` being replaced with a hyphen `-`

Provides the ability to override the topic name generation with a custom function that allows creating topics in alignment with custom conventions.

snippet: TopicNameGenerator

Be aware that ServiceControl doesn't allow customization of this convention when publishing ServiceControl events. ServiceControl events will be published using the default naming convention.

## Custom topics mappings

The [transport topology](topology.md#sqs-publishsubscribe) describes in depth how the topology is determined by subscribers. There are scenarios in which a custom mapping is needed.

The `MapEvent` transport configuration API can be used to customize the way subscribers determine the topic to subscribe to. If the subscribers have knowledge of both the published event type and the subscribed one, the following API can be used:

snippet: CustomTopicsMappingsTypeToType

NOTE: The types are only used to determine the topic name; subscribers can define dummy empty types to use the strongly typed API shown above.

If the published type is not known at compilation time, the following API can be used:

snippet: CustomTopicsMappingsTypeToTopic