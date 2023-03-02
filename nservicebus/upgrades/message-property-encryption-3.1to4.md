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

As of .NET 7, the [`RijndaelManaged` class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged) is obsolete and not recommended. The replacement class, [`AesManaged`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged) is backward-compatible with the old class in that messages encrypted with `RijdaelManaged` can be decrypted by `AesManaged` and vice versa. However, `AesManaged` is limited to a block size of 128 bits (as defined by the AES standard) while the `RijndaelManaged` allows 128-, 192-, and 256-bit block sizes. The lower block size has no effect on the strength of the encryption.

NOTE: If a system uses non-default block size, follow the upgrade guide from [version 3 to version 3.1](./message-property-encryption-3to3.1) prior to upgrading to version 4.

## RijdaelEncryptionService

The `RijdaelEncryptionService` has been removed. Use `AesEncryptionService` instead.
