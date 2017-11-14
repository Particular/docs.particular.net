---
title: Message Body Encryption
summary: Encrypting the full message body using a mutator.
reviewed: 2017-11-14
component: Core
tags:
- Encryption
related:
- nservicebus/security
- nservicebus/security/property-encryption
- nservicebus/pipeline/message-mutators
---


## Introduction

This sample shows how to use `IMutateTransportMessages` to encrypt/decrypt the binary data of a message as it passes through the pipeline.


## Run the solution.

Set both `Endpoint1` and `Endpoint2` as startup projects and run the solution. `Endpoint1` encrypts a message and sends it while `Endpoint2` receives the encrypted message and decrypts it.


## Code walk-through


### The message contract

Starting with the Shared project, open `CompleteOrder.cs`:

snippet: Message

Note that it does not need any custom property types to be encrypted.


### How is encryption configured.

Open either one of the `Program.cs` files. Notice the line:

snippet: RegisterMessageEncryptor

This is an extension method that adds an `IMutateTransportMessages` to the configuration.

snippet: MessageEncryptorExtension


#### The Mutator

WARNING: This is for demonstration purposes and is not true encryption. It is only doing a byte array reversal to illustrate the API. In a production system, encryption should be used via the [.NET Framework Cryptography Model](https://docs.microsoft.com/en-us/dotnet/standard/security/cryptography-model) or some other secure mechanism.

snippet: Mutator
