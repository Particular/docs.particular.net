---
title: Upgrade Version 8 to 9
summary: Instructions on how to upgrade NServiceBus from version 8 to version 9.
reviewed: 2022-04-22
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
 - 9
---

## SendOptions changes

In NServiceBus version 8 and earlier, the information if given message was marked for immediate dispatch can be verified by the `sendOptions.RequiredImmediateDispatch()`  `SendOptions` extension methods. In version 9, this extension method have been marked as obsolete in favour of `sendOptions.IsImmediateDispatchSet()`.
