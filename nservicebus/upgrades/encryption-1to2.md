---
title:  Upgrade Version 1 to 2
reviewed: 2017-09-18
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## Internal methods in `RijndaelEncryptionService`

The `AddKeyIdentifierHeader`, `ConfigureIV`, and `TryGetKeyIdentifierHeader` methods in `RijndaelEncryptionService` have been marked `internal`. These methods are not required to use the package and were not meant to be made public.

