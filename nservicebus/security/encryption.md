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

## Encryption options

Encryption can be applied via *property*, *message* or *transport* encryption.


## Property Encryption

Property encryption operates on specific properties of a message. The data in the property is encrypted, but the rest of the message is clear text. This keeps the performance impact of encryption as low as possible. 

The encryption algorithm used is [Rijndael](https://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndael.aspx).

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

Property encryption is enabled via the configuration API.

<!-- import EncryptionServiceSimple -->

### Generating encryption keys

In order to use the RijndaelEncryptionService a primary and secondary keys need to be generated. An option is to use `RijndaelManaged.GenerateKey`, please refer to the following article to generate new keys:

- https://msdn.microsoft.com/en-us/library/system.security.cryptography.rijndaelmanaged.generatekey(v=vs.110).aspx

Using a secure key generator makes sure that your encryption keys are really randomized.

### Configuring the encryption key

In conjunction with enabling encryption you need to configure the encryption and decryption keys.

Note: The key specified must be the same in the configuration of all processes that are communicating encrypted information, both on the sending and on the receiving sides.


#### App.config

The encryption key can be defined in the `app.config`.

<!-- import EncryptionFromAppConfig --> 
 

#### IProvideConfiguration

<!-- import EncryptionFromIProvideConfiguration -->

For more info on `IProvideConfiguration` see [Customizing NServiceBus Configuration](/nservicebus/hosting/custom-configuration-providers.md)


#### Configuration API

NOTE: Defining encryption keys via the configuration API is only supported in version 5 and up. 

<!-- import EncryptionFromCode -->


### Multi-Key decryption 

You will note in several of the above examples that both an encryption key and **multiple** decryption keys were defined.

This feature allows a phased approach of managing encryption keys. So that different endpoints can update to a new encryption key while still maintaining wire compatibility with endpoints using the old key.

When the original encryption key is replaced by a new encryption key, in-flight messages that were encrypted with the original key will fail decryption unless the original encryption key is added to a list of expired keys. 


### Custom handling of property encryption

To take full control over how properties are encrypted you can replace the `IEncryptionService` instance.

This allows you to explicitly handled the encryption and decryption of each value. So for example if you want to use an algorithm other than Rijndael.

<!-- import EncryptionFromIEncryptionService -->


## Message Encryption

Message encryption encrypts the complete message body. It can even include the message headers if these would contain sensitive data.

We currently do not offer this feature but it can be achieved by creating a transport message mutator using `IMutateTransportMessages`

<!-- import MessageBodyEncryptor -->

This class can be then injected into the container using the following:

<!-- import UsingMessageBodyEncryptor -->


## Transport Encryption

However, using message encryption is often used to prevent intercepting data during transport. If this is the requirement then you can maybe on the transport for encryption. RabbitMQ, SQL Server, Azure Storage Queues and Azure Service Bus support SSL. NServiceBus currently does not support MSMQ over HTTP/HTTPS, meaning transport encryption is unsupported.
