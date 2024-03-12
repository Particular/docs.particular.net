## Configuring a serializer

A serializer can be configured using the `endpointConfiguration.UseSerialization` API. Refer to the dedicated documentation pages for each available serializer for more information about the specific configuration.

The same serializer must be used by the sending endpoint to serialize messages and by the receiving endpoint to deserialize them, unless additional deserializers are specified.

## Using the default serializer

The default serializer used in NServiceBus projects is the custom [XmlSerializer](xml.md). Unless explicitly configured otherwise, NServiceBus will use [XmlSerializer](xml.md) for serializing and deserializing all messages.

WARN: In NServiceBus 8.1 and above, a runtime warning will encourage explicitly selecting a serializer. In a future version of NServiceBus, the XmlSerializer will no longer be selected by default.
