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

.NET 8 has depracated the `RijndaelManaged` class used by previous versions of this package. The replacement class, `AesManaged` is backwards compatible with the old class in the sense that messages encrypted with `RijdaelManaged` can be decrypted by `AesManaged` and vice versa. The `AesManaged` is, however, limited to 128 bit block size (as the AES standard defines) while the `RijndaelManaged` allos 128, 192 and 256 bit blocks. The lower block size has no effect on the strenght of the encryption.

NOTE: If the system used non default block size, make sure to follow the upgrade guide from version 3 to version 3.1 prior to upgrading to version 4.

## RijdaelEncryptionService

The `RijdaelEncryptionService` has been removed. Use `AesEncryptionService` instead.