---
title:  Upgrade Version 1 to 2
reviewed: 2021-07-23
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## Internal methods in `RijndaelEncryptionService`

The `AddKeyIdentifierHeader`, `ConfigureIV`, and `TryGetKeyIdentifierHeader` methods in `RijndaelEncryptionService` have been made `internal`. These methods are not required to use the package.

