---
title: Upgrade Version 9 to 9.1
summary: Instructions on how to upgrade NServiceBus from version 9 to version 9.1.
reviewed: 2024-07-24
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 9
---

## Meter source name change

In NServiceBus 9.0.x, the OpenTelemetry meter source name was `NServiceBus.Core`. In NServiceBus 9.1.0, it has been changed to `NServiceBus.Core.Pipeline.Incoming`.

Update endpoint configuration code to use the new meter source name or use a wildcard notation, e.g., `NServiceBus.Core*`.
