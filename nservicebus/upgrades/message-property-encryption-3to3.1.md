---
title: Upgrade version 3 to 3.1
reviewed: 2023-03-01
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
related:
 - nservicebus/security/property-encryption
---

As of .NET 7, the [`RijndaelManaged` class](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.rijndaelmanaged) is obsolete and not recommended. The replacement class, [`AesManaged`](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aesmanaged) is backward-compatible with the old class in that messages encrypted with `RijdaelManaged` can be decrypted by `AesManaged` and vice versa. However, `AesManaged` is limited to a block size of 128 bits (as defined by the AES standard) while the `RijndaelManaged` allows 128-, 192-, and 256-bit block sizes. The lower block size has no effect on the strength of the encryption.

## AesEncryptionService

`NServiceBus.Encryption.MessageProperty` version 3.1 includes the `AesEncryptionService` class that replaces the deprecated `RijndaelEncryptionService`. The new class does not allow overriding the initialization vector (IV) customization or changing the block size.

## Adjusting the block size

If a system uses custom block size value (either 192 or 256 bits) by subclassing the `RijndaelEncryptionService`, the block size must be adjusted. The steps to upgrade are:

1. Upgrade all endpoints in the system to version 3.1 of the `NServiceBus.Encryption.MessageProperty` package.
2. Remove the initialization vector (IV) customization code to switch to the default 128-bit block size required by AES. It's recommended to do this by switching all instances of `RijndaelEncryptionService` to `AesEncryptionService` to facilitate future upgrades. However, it can also be done by using the default settings of the `RijndaelEncryptionService` class.