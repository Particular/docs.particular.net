---
title: NServiceBus Host Upgrade Version 4 to 5
summary: Instructions on how to upgrade NServiceBus Host Version 4 to 5.
component: Host
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 4
 - 5
---

## Roles are obsolete

In NServiceBus.Host version 5 and above, roles are obsolete and should not be used. The functionality of `AsA_Server`, and `AsA_Publisher` is now the default in core and can be safely removed. Read the [Host guidance on alternatives for using roles](/nservicebus/hosting/nservicebus-host/?version=host_5#roles-built-in-configurations).

