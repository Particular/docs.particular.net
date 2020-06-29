---
title: Message Property Encryption
summary: Encrypt message fragments using property encryption
component: PropertyEncryption
reviewed: 2020-06-29
redirects:
- nservicebus/encryption
related:
- samples/encryption
---

partial: obsolete

Property encryption operates on specific properties of a message. The data in the property is encrypted, but the rest of the message is clear text. This keeps the performance impact of encryption as low as possible.

The encryption algorithm used is [Rijndael](https://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndael.aspx) and is based on a key shared between the sender and receiver which is known as symmetric encryption.

Keep in mind that the security is only as strong as the keys; if the key is exposed, then an attacker can decrypt the information. Encryption keys should not be stored on the client (if deployed remotely) or even on a web server in the DMZ.

partial: default


## Defining encrypted properties

There are two ways of telling NServiceBus what properties to encrypt.


### Convention

Given a message of this convention

snippet: MessageForEncryptionConvention

Encrypt `MyEncryptedProperty`.

snippet: DefiningEncryptedPropertiesAs


### Property type

Use the `EncryptedString` type to flag that a property should be encrypted.

snippet: MessageWithEncryptedProperty


## Enabling property encryption

Property encryption is enabled via the configuration API.

snippet: EncryptionServiceSimple


### Key ID

Each key needs an unique key ID (`KeyIdentifier`). The key ID is communicated in the message header and allows the receiving endpoint to use the correct decryption key.

NOTE: If a key ID is not set then no messages with encrypted properties can be sent. When NServiceBus receives a message without a key ID header, it will attempt decryption using all keys in the configuration. If all decryption attempts fail, the message will be moved to the error queue.


#### Troubleshooting

> Error: Encrypted message has no 'NServiceBus.RijndaelKeyIdentifier' header. Possibility of data corruption. Upgrade endpoints that send messages with encrypted properties.

Key IDs are only supported in the following versions

 * NServiceBus 3.3.16+
 * NServiceBus 4.7.8+
 * NServiceBus 5.0.7+
 * NServiceBus 5.1.5+
 * NServiceBus 5.2.9 and newer
 * NServiceBus.Encryption.MessageProperty all versions

All previous versions support decrypting messages that have encrypted fragments but no key ID header.


### Key ID naming strategy

A key ID identifies which key is used and it must not expose anything about the key itself.

Good strategies

 * Incremental (1, 2, 3, 4, etc.)
 * Time-based (`2015-w01`, `2015-m08`, `2015-q03`)
 * Random (`ab4b7a6e71833798`),

Bad strategies

 * Full hash of key (MD5, SHA-1, etc.)
 * Key fragment


NOTE: Random named key IDs DO NOT improve security as the key ID is not encrypted.

NOTE: Using a time-based key name does not weaken encryption. Messages already contain a timestamp that can be used to search for messages within a certain time range.


## Using the same key with and without a key ID

If the `KeyIdentifier` attribute is set for a key then it will be used to decrypt message properties with a matching key ID but it will also be used to attempt decryption for messages without a key ID.


partial: keyformat


## Defining the encryption key

When encryption is enabled, the encryption and decryption keys must be configured.

Note: Both the sending and the receiving side must use the same key to communicate encrypted information.


## Key strength

Description        | Calculation| Combinations
-------------------|------------|-------
ASCII 16 characters| 125^16     |  3.55e+33 (7 bits minus control characters)
ASCII 24 characters| 125^24     |  2.11e+50 (7 bits minus control characters)
ASCII 32 characters| 125^32     |  1.26e+67 (7 bits minus control characters)
Base64 16 bytes:   | 256^16     |  3.40e+38
Base64 24 bytes:   | 256^24     |  6.27e+57
Base64 32 bytes:   | 256^32     |  1.16e+77


This means that a 16 character ASCII key is almost 100.000 times weaker then a 16 byte Base64 key.

NOTE: Use Base64 key format if possible and ASCII 32 key format for backward compatibility only. ASCII 16 key format is no longer recommended.


## Configuration

partial: code


partial: config


## Multi-Key decryption

Several of the examples above used both an encryption key and **multiple** decryption keys.

This feature allows a phased approach to key rotation. Endpoints can update to a new encryption key and maintain ability to decrypt information sent by endpoints using old keys.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will not be decrypted unless the original encryption key is added to a list of decryption keys.


## Custom handling of property encryption

To take full control over how properties are encrypted replace the `IEncryptionService` instance.

This allows explicit handling of the encryption and decryption of each value. So for example to use an algorithm other than Rijndael.

snippet: EncryptionFromIEncryptionService


snippet: EncryptionService
