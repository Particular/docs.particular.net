---
title: Gateway Upgrade Version 3 to 4
summary: How to upgrade the Gateway from version 3 to 4.
component: Gateway
reviewed: 2022-09-22
related:
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Migration from BinaryFormatter

WARNING: When migrating from the `BinaryFormatter` ensure that all endpoints on all sites are updated to use the newly selected databus deserializer.

The `BinaryFormatter` is being phased out by Microsoft. The new data bus configuration API now requires a serializer.
Refer to the [Migration from BinaryFormatter](/nservicebus/upgrades/7to8/databus.md#migration-from-binaryformatter) for details on this.
