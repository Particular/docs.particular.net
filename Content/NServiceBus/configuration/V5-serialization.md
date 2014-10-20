---
title: Configuration API serialization in V5
summary: Configuration API serialization in V5
tags:
- NServiceBus
- BusConfiguration
- V5
---

Serialization can be controlled via the `UseSerialization` method of the `BusConfiguration` class, using one of the following supported serializers:

* `BinarySerializer`: binary serializer;
* `BsonSerializer`: BSON serializer;
* `JsonSerializer`: JSON serializer;
* `XmlSerializer`: XML serializer;

Some serializers have specific `SerializationExtensions` that allows to customize the serializer behavior:

* `JsonSerializer`:
	* `Encoding`: defines the necoding of the serialized stream;
* `XmlSerializer`:
	* `DontWrapRawXml`: Tells the serializer to not wrap properties which have either XDocument or XElement with a "PropertyName" element;
	* `Namespace`: Configures the serializer to use a custom namespace. `http://tempuri.net` is the default. If the provided namespace ends with trailing forward slashes, those will be removed on the fly;
	* `SanitizeInput`: Tells the serializer to sanitize the input data from illegal characters;