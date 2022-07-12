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

Configuring encryption via app.config and via `IProvideConfiguration` have been removed. Instead, use configuration via code.


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

```csharp
// For NServiceBus version 6.x
var defaultKey = "2015-10";

var keys = new Dictionary<string, byte[]>
{
    {"2015-10", Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
    {"2015-09", Convert.FromBase64String("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
    {"2015-08", Convert.FromBase64String("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
};
endpointConfiguration.RijndaelEncryptionService(defaultKey, keys);

// For Message Property Encryption version 1.x
var defaultKey = "2015-10";

var keys = new Dictionary<string, byte[]>
{
    {"2015-10", Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6")},
    {"2015-09", Convert.FromBase64String("abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
    {"2015-08", Convert.FromBase64String("cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6")},
};
var encryptionService = new RijndaelEncryptionService(defaultKey, keys);

endpointConfiguration.EnableMessagePropertyEncryption(encryptionService);
```


## Using EncryptedString

```csharp
// For NServiceBus version 6.x
using NServiceBus;

public class MyMessage :
    IMessage
{
    public WireEncryptedString MyEncryptedProperty { get; set; }
}

// For Message Property Encryption version 1.x
using NServiceBus;
using NServiceBus.Encryption.MessageProperty;

public class MyMessage :
    IMessage
{
    public EncryptedString MyEncryptedProperty { get; set; }
}
```


## Encrypted property convention

```csharp
// For NServiceBus version 6.x
var conventions = endpointConfiguration.Conventions();
conventions.DefiningEncryptedPropertiesAs(
    definesEncryptedProperty: propertyInfo =>
    {
        return propertyInfo.Name.EndsWith("EncryptedProperty");
    });

// For Message Property Encryption version 1.x
var encryptionService = new RijndaelEncryptionService(
    encryptionKeyIdentifier: "2015-10",
    key: Convert.FromBase64String("gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6"));

endpointConfiguration.EnableMessagePropertyEncryption(
    encryptionService: encryptionService,
    encryptedPropertyConvention: propertyInfo =>
    {
        return propertyInfo.Name.EndsWith("EncryptedProperty");
    }
);
```


## Custom encryption service

```csharp
// For NServiceBus version 6.x
endpointConfiguration.RegisterEncryptionService(
    func: () =>
    {
        return new EncryptionService();
    });

// For Message Property Encryption version 1.x
endpointConfiguration.EnableMessagePropertyEncryption(new EncryptionService());
```

