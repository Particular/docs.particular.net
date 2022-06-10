---
title: DataBus Binary Serializer
summary: A binary serializer for the data bus
component: BinaryDataBusSerializer
reviewed: 2022-05-27
related:
 - samples/file-share-databus
 - samples/databus/custom-serializer
---

This DataBus serializer uses the `BinaryFormatter` to serialize and deserialize data bus properties.

snippet: BinaryDataBusUsage

WARN: `BinaryFormatter` [is not supported in .NET 5](https://docs.microsoft.com/en-us/dotnet/core/compatibility/core-libraries/5.0/binaryformatter-serialization-obsolete). For projects that target .NET 5 and later, migrate to a [safer serializer](/nservicebus/messaging/databus/#serialization).