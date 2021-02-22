---
title: Externalize Message Property Encryption
reviewed: 2021-02-22
component: PropertyEncryption
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
related: 
 - nservicebus/security/property-encryption
---

include: externalize-encryption

The API was also modified.


## Removed APIs

Configuring encryption [via app.config](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-app-config) and [via IProvideConfiguration](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-iprovideconfiguration) have been removed. Instead, use [configuration via code](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-code).


## Compatibility

The NServiceBus.Encryption.MessageProperty package is not fully compatible with endpoints that use the NServiceBus package's encryption functionality. The core implementation is not aware of the existence of the external package, so it is unable to decrypt message that use `NServiceBus.Encryption.MessageProperty.EncryptedString`. Here are details of the specific cases.


### Encrypting and decrypting using NServiceBus.Encryption.MessageProperty

 * NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
 * NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages with message properties of type `NServiceBus.Encryption.MessageProperty.EncryptedString`.
 * NServiceBus.Encryption.MessageProperty can decrypt and encrypt all messages using an encrypted property convention.


### Encrypting and decrypting using NServiceBus

 * NServiceBus.Encryption.MessageProperty **cannot** decrypt and encrypt messages with message properties of type `NServiceBus.Encryption.MessageProperty.EncryptedString`.
 * NServiceBus can decrypt and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
 * NServiceBus can decrypt and encrypt all messages using an encrypted property convention.


## Migration example

For a system with two or more endpoints, these are the steps to migrate to the `NServiceBus.Encryption.MessageProperty` package:

 1. Install the `NServiceBus.Encryption.MessageProperty` NuGet package into all endpoints.
 1. Update the configuration for all endpoints to use either [RijndaelEncryptionService](#enabling-rijndaelencryptionservice) or [a custom encryption service](#custom-encryption-service).
 1. Deploy all endpoints.
 1. After all endpoints are deployed, update message contracts in all endpoints to use the `NServiceBus.Encryption.MessageProperty.EncryptedString` property type.
 1. Deploy all endpoints again.

Note: All endpoints must be updated to the `NServiceBus.Encryption.MessageProperty` package _and deployed_ before updating any message contracts to use `NServiceBus.Encryption.MessageProperty.EncryptedString`. This is to prevent issues with [compatibility](#compatibility).


## Enabling RijndaelEncryptionService

snippet: SplitEncryptionFromCode


## Using EncryptedString

snippet: SplitMessageWithEncryptedProperty


## Encrypted property convention

snippet: SplitDefiningEncryptedPropertiesAs


## Custom encryption service

snippet: SplitEncryptionFromIEncryptionService

