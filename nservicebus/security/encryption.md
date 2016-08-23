---
title: Encryption
tags:
- Encryption
- Security
redirects:
- nservicebus/encryption
related:
- samples/encryption
---


## Property Encryption

Property encryption operates on specific properties of a message. The data in the property is encrypted, but the rest of the message is clear text. This keeps the performance impact of encryption as low as possible.

The encryption algorithm used is [Rijndael](https://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndael.aspx).

Keep in mind that the security is only as strong as the keys; if the key is exposed, then an attacker can decipher the information. As such avoid encryption keys stored on the client (if deployed remotely) or even on a web server in the DMZ.

NOTE: In NServiceBus Versions 5 and below encryption is enabled by default and disables itself when there are no messages that require encryption. In NServiceBus Versions 6 and above encryption is only enabled when explicitly configured with either `endpointConfiguration.RijndaelEncryptionService` or `endpointConfiguration.RegisterEncryptionService`.

### Defining encrypted properties

There are two ways of telling NServiceBus what properties to encrypt.


#### Convention

Given a message of this convention

snippet:MessageForEncryptionConvention

Encrypt `MyEncryptedProperty` using `DefiningEncryptedPropertiesAs`.

snippet:DefiningEncryptedPropertiesAs


#### Property type

Use the `WireEncryptedString` type to flag that a property should be encrypted.

snippet: MessageWithEncryptedProperty


### Enabling property encryption

Property encryption is enabled via the configuration API.

snippet:EncryptionServiceSimple


#### Key ID

Each key needs an unique key ID (`KeyIdentifier`). The key ID is communicated in the message header meta data and provides the receiving endpoint information on which key to use for decryption.

> Error: It is required to set the Rijndael key ID.

NOTE: If a key ID is not set then no encrypted messages can be send but received messages without a key ID header will be decrypted using all keys in the configuration.

> Error: Encrypted message has no 'NServiceBus.RijndaelKeyIdentifier' header. Possibility of data corruption. Upgrade endpoints that send messages with encrypted properties.

{{NOTE:
Key IDs are only supported in the following versions

 * 3.3.16+
 * 4.7.8+
 * 5.0.7+
 * 5.1.5+
 * 5.2.9 and newer

All previous versions support decrypting messages that have encrypted fragments but no key ID header.
}}


#### Key ID naming strategy

A key ID identifies which key is used, it does not expose anything about the key itself.

Good strategies

 * Incrementing (1, 2, 3, 4, etc.)
 * Timestamp (2015-w01, 2015-m08, 2015-q03
 * Random (ab4b7a6e71833798),

Bad strategies

 * Full hash of key (MD5, SHA-1, etc.)


NOTE: Random named key IDs DO NOT improve security as the key ID is not encrypted.

NOTE: Timestamping does not weaken encryption. Messages already contain a timestamp that can be used to search for messages within a certain time range.


### Using the same key with and without a key ID

If the `KeyIdentifier` attribute is set then this key will be used to decrypt message with a matching key ID but it will also be used to try decrypting messages without a key ID.


partial: keyformat


### Defining the encryption key

In conjunction with enabling encryption, configure the encryption and decryption keys is required.

Note: The key specified must be the same in the configuration of all processes that are communicating encrypted information, both on the sending and on the receiving sides.


### Key strength

Description        | Calculation| Combinations
-------------------|------------|-------
ASCII 16 characters| 125^16     |  3.55e+33 (7 bits minus control characters)
ASCII 24 characters| 125^24     |  2.11e+50 (7 bits minus control characters)
ASCII 32 characters| 125^32     |  1.26e+67 (7 bits minus control characters)
Base64 16 bytes:   | 256^16     |  3.40e+38
Base64 24 bytes:   | 256^24     |  6.27e+57
Base64 32 bytes:   | 256^32     |  1.16e+77


This means that a 16 character ASCII key is almost 100.000 times weaker then a 16 byte Base64 key.

NOTE: Use Base64 if possible and to use ASCII 32 character keys for backward compatibility and not to use ASCII 16 character keys.


partial: code


#### Via App.config

The encryption key can be defined in the `app.config`.

snippet: EncryptionFromAppConfig


#### Via IProvideConfiguration

snippet:EncryptionFromIProvideConfiguration

For more info on `IProvideConfiguration` see [Customizing NServiceBus Configuration](/nservicebus/hosting/custom-configuration-providers.md)


### Multi-Key decryption

Note in several of the above examples that both an encryption key and **multiple** decryption keys were defined.

This feature allows a phased approach of managing encryption keys. So that different endpoints can update to a new encryption key while still maintaining wire compatibility with endpoints using the old key.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will fail decryption unless the original encryption key is added to a list of expired keys.


### Custom handling of property encryption

To take full control over how properties are encrypted replace the `IEncryptionService` instance.

This allows explicit handling of the encryption and decryption of each value. So for example to use an algorithm other than Rijndael.

snippet:EncryptionFromIEncryptionService


## Message Encryption

Message encryption leverages the pipeline to apply encryption to the whole message body.

Once way of achieving this is using a `IMutateTransportMessages`.

snippet:MessageBodyEncryptor

The this class can be then injected into the container using the following

snippet:UsingMessageBodyEncryptor
