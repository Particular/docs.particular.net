---
title: Serialization Changes in NServiceBus Version 6
reviewed: 2020-05-11
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---



## BinarySerializer deprecated

The BinarySerializer is deprecated. Use one of the supported serializers or an external serializer.


## No dependency injection for IMessageSerializer

The `IMessageSerializer` instances are now produced by a factory (as described in [this article](/nservicebus/serialization/custom-serializer.md)) instead of being resolved through [dependency injection](/nservicebus/dependency-injection/).


## Built-in serializers are internal

Built-in JSON and XML serializers are internal starting with NServiceBus version 6. If a custom serializer depends on one of these serializers, the code will need to be copied.


## Standardized XML Serialization

Handling of null types within the XML serializer now conforms to the [W3C Specification](https://www.w3.org/TR/xmlschema-1/#xsi_nil) by using the `xsi:nil="true"` attribute.

This change is backward compatible and will have no impact on communication between older versions of endpoints and newer versions. Older versions will be able to communicate with newer versions and vice versa.

Given the following class:

```cs
public class MessageWithNullable : IMessage
{
    public string FirstName { get; set; }
    public DateTime? BirthDate { get; set; } //Nullable DateTime property
}
```

A null `BirthDate` would result in a message in the following:

snippet: 5to6nullXml

WARNING: External integration systems need to ensure compatibility when receiving messages in the new format.


## BSON serializer deprecated

The BSON serializer built into the core has been removed. Use the [Newtonsoft serializer](/nservicebus/serialization/newtonsoft.md) as a replacement. Also see the [Newtonsoft BSON sample](/samples/serializers/newtonsoft-bson/).
