---
title: DataBus Binary Serializer
summary: BinaryFormatter-based DataBus serializer (deprecated and unsafe)
component: BinaryDataBusSerializer
reviewed: 2025-12-09
redirects:
 - nservicebus/messaging/databus/binary
related:
 - samples/databus/file-share-databus
 - samples/databus/custom-serializer
---

This DataBus serializer uses **`BinaryFormatter`** to serialize and deserialize DataBus properties.

snippet: BinaryDataBusUsage

> [!CAUTION]
> **`BinaryFormatter` is unsafe and deprecated.**  
> It cannot be secured and must not be used for processing data.  
> Stop using it as soon as possible, even if the data appears trustworthy.  
>
> Use a [safer serializer](/nservicebus/messaging/claimcheck/#serialization) instead.  
> For guidance, see the upgrade notes:  
> **[Migration from BinaryFormatter](/nservicebus/upgrades/7to8/databus.md#migration-from-binaryformatter).**
