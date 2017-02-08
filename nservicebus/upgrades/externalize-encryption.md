---
title: Externalize Message Property Encryption
reviewed: 2017-02-08
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related: 
 - nservicebus/security/property-encryption
---

The [Message Property Encryption feature](/nservicebus/security/property-encryption.md), has been moved from the NServiceBus package to the separate [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty/) NuGet package. That package should be used to encrypt message properties when using versions NServiceBus Versions 6.2 and above.

The API was also modified.


## Removed APIs

Configuring encryption [Via app.config](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-app-config) and [Via IProvideConfiguration](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-iprovideconfiguration) have been removed. Instead use [Configuration via code](/nservicebus/security/property-encryption.md#configuration-via-code).


## Enabling RijndaelEncryptionService

snippet: SplitEncryptionFromCode


## Using WireEncryptedString

snippet: SplitMessageWithEncryptedProperty


## Encrypted property convention

snippet: SplitDefiningEncryptedPropertiesAs


## Custom encryption service

snippet: SplitEncryptionFromIEncryptionService


## Compatibility

The NServiceBus.Encryption.MessageProperty package is partially compatible with endpoints using NServiceBus package's encryption functionality:


### Encrypting and decrypting using NServiceBus.Encryption.MessageProperty
* NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
* NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages with message properties of type `NServiceBus.Encryption.MessageProperty.WireEncryptedString`.
* NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages using an encrypted property convention.


### Encrypting and decrypting using NServiceBus
* NServiceBus.Encryption.MessageProperty **cannot** decrypt and encrypt messages with message properties of type `NServiceBus.Encryption.MessageProperty.WireEncryptedString`.
* NServiceBus can decrypt and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
* NServiceBus can decrypt and encrypt all messages using an encrypted property convention.

When migrating endpoints to use the NServiceBus.Encryption.MessageProperty package:
1. Upgrade all the endpoints to use the new package. The endpoints will continue to process all messages the same way as they've done until then.
2. After all endpoints have been migrated, upgrade message contracts to use the `NServiceBus.Encryption.MessageProperty.WireEncryptedString` property type.
