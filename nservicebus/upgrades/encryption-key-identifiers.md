---
title: Upgrade encryption configuration
summary: Instructions on how to upgrade the Rijndael property encryption configuration to use key IDs.
component: PropertyEncryption
reviewed: 2017-06-30
related:
 - nservicebus/security/generating-encryption-keys
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 3
 - 4
 - 5
---

## Summary

This upgrade document explains what steps to take when upgrading NServiceBus endpoints to make use of the following hotfix releases:

 * [NServiceBus 3.3.17](https://github.com/Particular/NServiceBus/releases/tag/3.3.17)
 * [NServiceBus 4.7.8](https://github.com/Particular/NServiceBus/releases/tag/4.7.8)
 * [NServiceBus 5.0.7](https://github.com/Particular/NServiceBus/releases/tag/5.0.7)
 * [NServiceBus 5.1.5](https://github.com/Particular/NServiceBus/releases/tag/5.1.5)
 * [NServiceBus 5.2.9](https://github.com/Particular/NServiceBus/releases/tag/5.2.9)

To deploy this fix throughout a system, first upgrade endpoints with the latest versions of NServiceBus, then update endpoint configurations. Optionally, also generate new encryption keys.


### Upgrade endpoints

All endpoints using encryption need to be upgraded to the latest patch release.

 * Version 3.x.x requires at least 3.3.17
 * Version 4.x.x requires at least 4.7.8
 * Version 5.x.x requires at least 5.0.7, 5.1.5 or 5.2.9

For example, if currently using NServiceBus 4.7.x, an update to the latest patch release 4.7.8 is required.


## Compatibility

It is not required to upgrade all endpoints simultaneously. There has not been a change in encryption or decryption. If a key ID header is present in a message received by an endpoint that does not know this header, it will still decrypt the message.


## Upgrade steps

Steps:

 * Stop the endpoint.
 * Deploy the new version.
 * Edit the endpoint configuration, adding key identifiers to at least the current encryption key, if not the backup keys as well.
 * Start the endpoint.

NOTE: If only the new version is deployed without updating the configuration, then the endpoint will start but will not be able to encrypt messages or decrypt messages that have a key ID.


### Update endpoint configurations

The following configuration examples demonstrate how to update existing endpoints while still using the same encryption key to not require all endpoints to be down at once and to allow keys to be backward compatible.


#### XML Configuration

It is possible to keep the current encryption key and still be able to decrypt messages in flight:

```xml
<RijndaelEncryptionServiceConfig Key="do-not-use-this-encryption-key!!"
                                 KeyIdentifier="2015-10">
  <ExpiredKeys>
    <add Key="an-expired-encryption-key-here-!" />
    <add Key="another-expired-key-!-----------" />
  </ExpiredKeys>
</RijndaelEncryptionServiceConfig>
```


#### Code API (Versions 5 and above)

When recompiling an obsolete warning will occur. Change the current method to the new one that allows passing a dictionary to lookup keys and an optional list of keys that will be used to decrypt messages with no key ID header.

NOTE: Keys need to be available in the expired keys list to be backward compatible.

**From**

```cs
busConfiguration.RijndaelEncryptionService("do-not-use-this-encryption-key!!")
```

**To**

Send encrypted messages with the existing key.

Does not require all endpoints to be stopped, updated, configured and started simultaneously.

```cs
var key = System.Convert.FromBase64String("do-not-use-this-encryption-key!!");

busConfiguration.RijndaelEncryptionService(
    "2015-10",
    new Dictionary<string,byte[]>{{ "2015-10", key}},
    // Required for decrypting message without a key identifier
    new[]{ key }
    );
```


## Generating new keys

Create a new encryption key if using ASCII keys with a key length of 16 characters to overcome the 7-bit limit that weakens the encryption key.

Switch to Base64 256 bits keys if possible or to at least ASCII 24 character keys to be backward compatible with pre Version 5 endpoints and have stronger encryption.

NOTE: Base64 keys can only be configured for NServiceBus Versions 5 and above, and are not compatible with earlier versions.


## Locating possible corrupted data

Property encryption is often used for the following types of data:

 * Credit card numbers
 * Social security numbers
 * Bank accounts

Investigate locations where such values are stored and check if these values are in an expected format by verifying that these values have a:

 * correct length
 * correct character set (credit card should only contain numbers)

Search in log files that indicate data validation issues. These could suggest that properties have been decrypted with an incorrect key.


## Recovering data

Recovering corrupted data is *only* possible when the original encrypted messages are moved to an audit queue.

There is no tool that helps in recovery. [Contact support](https://particular.net/support) if data corruption is suspected.