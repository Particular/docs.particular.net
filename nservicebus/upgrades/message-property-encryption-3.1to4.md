---
title: Upgrade version 3.1 to 4
reviewed: 2023-03-01
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
related:
 - nservicebus/security/property-encryption
---

As of .NET 7, the [`RijndaelManaged` class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged) is obsolete and not recommended. The replacement class, [`AesManaged`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged) is backward-compatible with the old class in that messages encrypted with `RijdaelManaged` can be decrypted by `AesManaged` and vice versa.

## RijdaelEncryptionService

The `RijdaelEncryptionService` has been removed. Use `AesEncryptionService` instead.
