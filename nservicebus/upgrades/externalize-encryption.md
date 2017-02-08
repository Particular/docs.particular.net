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

The [Message Property Encryption feature](/nservicebus/security/property-encryption.md), has been moved from the NServiceBus core to the separate NuGet package [NServiceBus.Encryption.MessageProperty](https://www.nuget.org/packages/NServiceBus.Encryption.MessageProperty/). That package should be used in order to encryption message properties when using versions NServiceBus Versions 6.2 and above.

The API was also modified.


## Removed APIs

Configuring encryption [Via app.config](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-app-config) and [Via IProvideConfiguration](/nservicebus/security/property-encryption.md?version=core_6#configuration-via-iprovideconfiguration) have been removed. Instead use [Configuration via code](/nservicebus/security/property-encryption.md#configuration-via-code).


## Enabling RijndaelEncryptionService

snippet: SplitEncryptionFromCode


## Using WireEncryptedString

snippet: SplitMessageWithEncryptedProperty


## Encrypted Property Convention

snippet: SplitDefiningEncryptedPropertiesAs


## Custom encryption Service

snippet: SplitEncryptionFromIEncryptionService


## Compatibility

The NServiceBus.Encryption.MessageProperty package is partially compatible with endpoints using NServiceBus core's encryption functionality:


### Sending and receiving using NServiceBus.Encryption.MessageProperty
* NServiceBus.Encryption.MessageProperty can de- and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
* NServiceBus.Encryption.MessageProperty can de- and encrypt all messages with message properties of type `NServiceBus.Encryption.MessageProperty.WireEncryptedString`.
* NServiceBus.Encryption.MessageProperty can de- and encrypt all messages using an encrypted property convention.


### Sending and receiving using NServiceBus core
* NServiceBus can de- and encrypt all messages with message properties of type `NServiceBus.WireEncryptedString`.
* NServiceBus can de- and encrypt all messages using an encrypted property convention.
* NServiceBus.Encryption.MessageProperty **cannot** de- and encrypt messages with message properties of type `NServiceBus.Encryption.MessageProperty.WireEncryptedString`.

When migrating endpoints to use the NServiceBus.Encryption.MessageProperty package, message contracts should only be updated to use the `NServiceBus.Encryption.MessageProperty.WireEncryptedString` property type when all endpoints are migrated.
