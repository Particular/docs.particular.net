### Topology

* `Topology`: The topology used to publish and subscribe to events between endpoints. The topology is shared by the endpoints that need to publish and subscribe to events from each other. The topology has to be explicitly passed into the constructor

Endpoints that do not require backward compatibility with the previous single-topic topology should be using `TopicTopology.Default` which represents the new default [topic-per-event topology](/transports/azure-service-bus/topology.md). For transports requiring compatibility during the migration towards the topic-per-event topology the [upgrade guide](/transports/upgrades/asbs-4to5.md) describes in more details the migration topology.

Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).

#### Mapping

#### Options

It is possible to configure a topology entirely from configuration by loading a serialized version of the options and using the `TopicTopology.FromOptions` to create the topology.

```json
{
  ...
  "QueueNameToSubscriptionNameMap": {
    "QueueName": "SubscriptionName"
  }
}
```