The encryption algorithm used is [AES](https://learn.microsoft.com/en-us/dotnet/api/system.security.cryptography.aes). This algorithm is based on a key shared between the sender and receiver which is known as symmetric encryption.

> [!WARNING]
> Prior to `NServiceBus.Encryption.MessageProperty` version 3.1, the Rijndael algorithm was used to encrypt data. Microsoft has made the `Rijndael` class obsolete in favor of AES and `NServiceBus.Encryption.MessageProperty` version 3.1 contains both `RijndaelEncryptionService` and `AesEncryptionService` classes to facilitate [migrating](/nservicebus/upgrades/message-property-encryption-3to3.1.md) from the former to the latter. For new development, use `AesEncryptionService` as the Rijndael service has been removed in later versions of the package.
