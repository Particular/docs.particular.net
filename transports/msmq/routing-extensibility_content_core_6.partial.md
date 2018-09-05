## Instance mapping extensibility

To customize how logical endpoints are mapped to a physical instance when routing messages, use the following code in a [Feature](/nservicebus/pipeline/features.md):

snippet: RoutingExtensibility-Instances

`sourceKey` uniquely identifies the mapping source so that it can be modified and must be unique.

Note: The entire source will be replaced by the code above so all known instance mappings must be added.

The instances collection is thread-safe. It allows registering multiple instance of a given endpoint. In case there is more than one mapping for a given logical endpoint, messages will be distributed based on the active [distribution strategy](/transports/msmq/sender-side-distribution.md#mapping-physical-endpoint-instances-message-distribution).