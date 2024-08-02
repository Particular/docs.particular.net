---
title: DataBus Binary Serializer
summary: A binary serializer for the data bus
component: BinaryDataBusSerializer
reviewed: 2024-02-16
redirects:
 - nservicebus/messaging/databus/binary
related:
 - samples/databus/file-share-databus
 - samples/databus/custom-serializer
---

This DataBus serializer uses the `BinaryFormatter` to serialize and deserialize data bus properties.

snippet: BinaryDataBusUsage

> [!WARNING]
> `BinaryFormatter` [is dangerous and is not recommended for data processing](https://aka.ms/binaryformatter). Applications should stop using BinaryFormatter as soon as possible, even if they believe the data they're processing to be trustworthy. BinaryFormatter is insecure and can't be made secure. Switch to a [safer serializer](/nservicebus/messaging/claimcheck/#serialization). Refer to the [Migration from BinaryFormatter](/nservicebus/upgrades/7to8/databus.md#migration-from-binaryformatter) for details.
