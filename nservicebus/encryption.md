---
title: Encryption
summary: Message Encryption
tags:
- Encryption
---

## Property Encryption

Property encryption operates on specific properties of a message. The data in the property is encrypted, but the rest of the message is clear text. This keeps the performance impact of encryption as low as possible. 

The encryption algorithm used is [Rijndael](http://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndael.aspx).

Keep in mind that the security is only as strong as the keys; if the key is exposed, then an attacker can decipher the information. As such, you may not want to have your encryption keys stored on the client (if deployed remotely) or even on a web server in the DMZ. 

### Defining encrypted properties

There are two ways of telling NServiceBus what properties to encrypt.

#### Convention 

Given a message of this convention 

<!-- import MessageForEncryptionConvention -->

You can encrypt `MyEncryptedProperty` using `DefiningEncryptedPropertiesAs`.

<!-- import DefiningEncryptedPropertiesAs -->

#### Property type

You can also use the `WireEncryptedString` type to flag that a property should be encrypted.

<!-- import MessageWithEncryptedProperty --> 

### Enabling property encryption

Property encryption is enabled via the fluent API.

<!-- import EncryptionServiceSimple -->

### Defining the encryption key

In conjunction with enabling encryption you need to configure the encryption and decryption keys.

#### App.config

The encryption key can be defined in the `app.config`.

<!-- import EncryptionFromAppConfig --> 
 
#### IProvideConfiguration

<!-- import EncryptionFromIProvideConfiguration -->

For more info on `IProvideConfiguration` see [Customizing NServiceBus Configuration](customizing-nservicebus-configuration.md)

#### Fluent API

NOTE: Defining encryption keys via the fluent API is only supported in V5 and up. 

<!-- import EncryptionFromFluentAPI -->

### Multi-Key decryption 

You will note in several of the above examples that both an encryption key and **multiple** decryption keys were defined.

This feature allows a phased approach of managing encryption keys. So that different endpoints can update to a new encryption key while still maintaining wire compatibility with endpoints using the old key.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will fail decryption unless the original encryption key is added to a list of expired keys. 

### Custom handling of property encryption

To take full control over how properties are encrypted you can replace the `IEncryptionService` instance.

This allows you to explicitly handled the encryption and decryption of each value. So for example if you want to use an algorithm other than Rijndael.

<!-- import EncryptionFromIEncryptionService -->

## Message Encryption

Message encryption leverages the pipeline to apply encryption to the whole message body.

Once way of achieving this is using a `IMutateTransportMessages`.

<!-- import MessageBodyEncryptor -->

The this class can be then injected into the container using the following

<!-- import UsingMessageBodyEncryptor -->

NOTE: `IMutateTransportMessages` are non-deterministic in terms of order of execution. If you want more fine grained control over the pipeline see [Pipeline Introduction](nservicebus-pipeline-intro.md).