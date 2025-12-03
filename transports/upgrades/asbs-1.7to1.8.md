---
title: Azure Service Bus Transport Upgrade Version 1.7 to 1.8
reviewed: 2020-12-23
component: ASBS
related:
 - transports/azure-service-bus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
---

## Replacing shorteners with naming conventions

For existing shorteners, the logic has to be moved into appropriate naming conventions.
To retain the same behavior, a length check needs to be added to the naming conventions logic.

### RuleNameShortener

To shorten subscription rules without changing the naming convention, apply the shortening logic to `Type.FullName` of the passed event type.
