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

Configuring encryption [via app.config](/nservicebus/security/property-encryption.md#configuration-via-app-config) and [via IProvideConfiguration](/nservicebus/security/property-encryption.md#configuration-via-iprovideconfiguration) have been removed. Instead use [configuration via code](/nservicebus/security/property-encryption.md#configuration-via-code).


## Migration example

For a system with two or more endpoints, there are the steps to migrate to the `NServiceBus.Encryption.MessageProperty` package:

 1. Install the `NServiceBus.Encryption.MessageProperty` NuGet package into all endpoints.
 1. Update the configuration for all endpoints to use either [RijndaelEncryptionService](#enabling-rijndaelencryptionservice) or [a custom encryption service](#custom-encryption-service).
 1. Deploy all endpoints.
 1. Update message contracts in all endpoints to use the `NServiceBus.Encryption.MessageProperty.WireEncryptedString` property type.
 1. Deploy both endpoints.

Note: All endpoints must be updated to the `NServiceBus.Encryption.MessageProperty` package _before_ updating any message contracts to use `NServiceBus.Encryption.MessageProperty.WireEncryptedString`. This is to prevent issues with [compatibility](#compatibility).


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
