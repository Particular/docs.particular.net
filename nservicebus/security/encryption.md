---
title: Encryption
summary: Message Encryption
tags:
- Encryption
redirects:
- nservicebus/encryption
related:
- samples/encryption
---

## Property Encryption

Property encryption operates on specific properties of a message. The data in the property is encrypted, but the rest of the message is clear text. This keeps the performance impact of encryption as low as possible.

The encryption algorithm used is [Rijndael](https://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndael.aspx).

Keep in mind that the security is only as strong as the keys; if the key is exposed, then an attacker can decipher the information. As such, you may not want to have your encryption keys stored on the client (if deployed remotely) or even on a web server in the DMZ.


### Defining encrypted properties

There are two ways of telling NServiceBus what properties to encrypt.


#### Convention

Given a message of this convention

snippet:MessageForEncryptionConvention

You can encrypt `MyEncryptedProperty` using `DefiningEncryptedPropertiesAs`.

snippet:DefiningEncryptedPropertiesAs


#### Property type

You can also use the `WireEncryptedString` type to flag that a property should be encrypted.

snippet: MessageWithEncryptedProperty


### Enabling property encryption

Property encryption is enabled via the configuration API.

snippet:EncryptionServiceSimple


#### Key identifier

Each key needs an unique key identifier (`KeyIdentifier`). The key identifier is communicated in the message header meta data and provides the receiving endpoint information on which key to use for decryption.

> Error: It is required to set the rijndael key identifier.

NOTE: If a key identifier is not set then no encrypted messages can be send but received messages without a key identifier header will be decrypted using all keys in the configuration.

> Error: Encrypted message has no 'NServiceBus.RijndaelKeyIdentifier' header. Possibility of data corruption. Please upgrade endpoints that send messages with encrypted properties.

NOTE: Key identifiers are only supported since v3.3.16+, v4.7.8+, v5.0.7, 5.1.5, 5.2.9 and newer. All previous versions support decrypting messages that have encrypted fragments but no key identifier header.


#### Key identifier naming strategy

A key identifier identifies which key is used, it does not expose anything about the key itself.

Good strategies

- Incrementing (1, 2, 3, 4, etc.)
- Timestamp (2015-w01, 2015-m08, 2015-q03
- Random (ab4b7a6e71833798),

Bad strategies

- Full hash of key (md5, sha-1, etc.)


NOTE: Random named key identifiers DO NOT improve security as the key identifier is not encrypted.

NOTE: Timestamping do not weaken encryption. Messages already contain a timestamp that can be used to search for messages within a certain time range.


### Using the same key with and without a key identifier

If the KeyIdentifier attribute is set then this key will be used to decrypt message with a matching key identifier but it will also be used to try decrypting messages without a key identifier.


#### Key format (v5+)

The key format can be specified in either *Base64* or *ASCII* format.


With ASCII its not possible to use the full 8bit range of a byte as its a 7bit encoding and even then some characters need to be escaped which is not done resulting in even less characters. Meaning per byte only about 100 values are used. When you use 16 byte / 128 bit keys this means only about 100^16 combinations are possible versus 256^16.

NOTE: Use Base64 whenever possible, ASCII 7bit keys are meant for backwards compatibility.


### Defining the encryption key

In conjunction with enabling encryption you need to configure the encryption and decryption keys.

Note: The key specified must be the same in the configuration of all processes that are communicating encrypted information, both on the sending and on the receiving sides.


### Key strength

Description        | Calculation| Combinations
-------------------|------------|-------
ASCII 16 characters| 125^16     |  3.55e+33 (7bits minus control characters)
ASCII 24 characters| 125^24     |  2.11e+50 (7bits minus control characters)
ASCII 32 characters| 125^32     |  1.26e+67 (7bits minus control characters)
Base64 16 bytes:   | 256^16     |  3.40e+38
Base64 24 bytes:   | 256^24     |  6.27e+57
Base64 32 bytes:   | 256^32     |  1.16e+77


This means that a 16 character ASCII key is almost 100.000 times weaker then a 16 byte Base64 key.

NOTE: Our advice is to use Base64 if possible and to use ASCII 32 character keys for backward compatibility and not to use ASCII 16 character keys.


#### App.config

The encryption key can be defined in the `app.config`.

snippet: EncryptionFromAppConfig


#### IProvideConfiguration

snippet:EncryptionFromIProvideConfiguration

For more info on `IProvideConfiguration` see [Customizing NServiceBus Configuration](/nservicebus/hosting/custom-configuration-providers.md)


#### Configuration API

NOTE: Defining encryption keys via the configuration API is only supported in Version 5 and above.

snippet:EncryptionFromCode


### Multi-Key decryption

You will note in several of the above examples that both an encryption key and **multiple** decryption keys were defined.

This feature allows a phased approach of managing encryption keys. So that different endpoints can update to a new encryption key while still maintaining wire compatibility with endpoints using the old key.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will fail decryption unless the original encryption key is added to a list of expired keys.


### Custom handling of property encryption

To take full control over how properties are encrypted you can replace the `IEncryptionService` instance.

This allows you to explicitly handled the encryption and decryption of each value. So for example if you want to use an algorithm other than Rijndael.

snippet:EncryptionFromIEncryptionService


## Message Encryption

Message encryption leverages the pipeline to apply encryption to the whole message body.

Once way of achieving this is using a `IMutateTransportMessages`.

snippet:MessageBodyEncryptor

The this class can be then injected into the container using the following

snippet:UsingMessageBodyEncryptor
