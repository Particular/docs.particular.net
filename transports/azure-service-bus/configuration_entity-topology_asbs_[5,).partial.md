### Topology

* `Topology`: The topology used to publish and subscribe to events between endpoints. The topology is shared by the endpoints that need to publish and subscribe to events from each other. The topology has to be explicitly passed into the constructor.

Endpoints that do not require backward compatibility with the previous single-topic topology should be using `TopicTopology.Default` which represents the new default [topic-per-event topology](/transports/azure-service-bus/topology.md). For transports requiring compatibility during the migration towards the topic-per-event topology the [upgrade guide](/transports/upgrades/asbs-4to5.md) describes the migration topology in more depth.

Topic names must adhere to the limits outlined in [the Microsoft documentation on topic creation](https://docs.microsoft.com/en-us/rest/api/servicebus/create-topic).

#### Mapping

##### Options

It is possible to configure a topology entirely from configuration by loading a serialized version of the options and using the `TopicTopology.FromOptions` to create the topology.

This allows loading the topology configuration from Application configuration or any other source. The options layer also provides support for source-generated serializer options as part of `TopologyOptionsSerializationContext`.

The following snippet demonstrates raw deserialization of options and creating the topology from those options. Usage may vary depending on the usage cases. For more details how to load options in the generic host consolidate the [options sample](/samples/azure-service-bus-netstandard/options/).

snippet: asb-options-options-loading

The topology json document for the topic-per-event topology looks like:

snippet: topology-options

In order to support polymorphic event types, one event (base type) can be mapped to multiple topics (where the derived events are published):

snippet: topology-options-inheritance

Loading from json is also supported for the migration topology:

snippet: migration-options

##### Validation

During the start of the transport the topology configuration is validated against some of the well known limitations like entity, subscription or rule name lengths and some consistency validation is executed.

The default validator uses data validations and source-generated options validation. The default validator can be overriden or the validation can be entirely disabled.

snippet: asb-options-validation-disable

Disabling the validator might be desirable in generic hosting scenarios when the topology options are loaded from the Application configuration and the validator is registered to validate at startup to avoid double validating.
