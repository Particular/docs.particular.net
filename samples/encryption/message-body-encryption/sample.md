---
title: Message body encryption
summary: Encrypting the full message body using a mutator.
reviewed: 2020-08-24
component: Core
related:
- nservicebus/security
- nservicebus/security/property-encryption
- nservicebus/pipeline/message-mutators
- serviceinsight/custom-message-viewers
---


This sample demonstrates how to use `IMutateTransportMessages` to encrypt and decrypt the binary representation of a message as it passes through the pipeline.

Running the solution starts two console applications. `Endpoint1` encrypts a message and sends it and `Endpoint2` receives the encrypted message and decrypts it.

### Endpoint1 output

```
Message sent.
```

### Endpoint2 output

```
CompleteOrderHandler Received CompleteOrder with credit card number 123-456-789
```


## Code walk-through


### Message contract

The `Shared` project contains `CompleteOrder.cs`, which defines the message contract:

snippet: Message

Note that a custom property type is not required.


### Encryption configuration

Encryption is enabled by calling an extension method in `Program.cs` in both `Endpoint1` and `Endpoint2`:

snippet: RegisterMessageEncryptor

The extension method is in `Shared/EncryptionExtensions.cs`:

snippet: MessageEncryptorExtension

The mutator is in `Shared/MessageEncryptor.cs`:

WARNING: This is for demonstration purposes and is not true encryption. It is only doing a byte array reversal to illustrate the API. In a production system, encryption should be performed used the [.NET Framework Cryptography Model](https://docs.microsoft.com/en-us/dotnet/standard/security/cryptography-model) or some other secure mechanism.

snippet: Mutator
