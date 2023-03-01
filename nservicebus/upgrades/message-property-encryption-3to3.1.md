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

.NET 8 has depracated the `RijndaelManaged` class used by this package. The replacement class, `AesManaged` is backwards compatible with the old class in the sense that messages encrypted with `RijdaelManaged` can be decrypted by `AesManaged` and vice versa. The `AesManaged` is, however, limited to 128 bit block size (as the AES standard defines) while the `RijndaelManaged` allos 128, 192 and 256 bit blocks. The lower block size has no effect on the strenght of the encryption.

## AesEncryptionService

The version 3.1 includes the `AesEncryptionService` class that replaces the now deprecated `RijndaelEncryptionService`. The new class does not allow overriding the initialization vector (IV) customization and changing the block size.

## Adjusting the block size

If the system used custom block size value (either 192 or 256 bits) by subclassing the `RijndaelEncryptionService`, the block size needs to be adjusted. The first step is to upgrade all endpoints in the system to version 3.1 of the `NServiceBus.Encryption.MessageProperty` package.

When all endpoints are upgraded, the second step is to remove the initialization vector (IV) customization code to switch to the default 128 bits (compatible wit the AES) either by using the default settings of `RijndaelEncryptionService` or by switching to `AesEncryptionService`.