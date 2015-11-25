---
title: Message Body Encryption
summary: How to encrypt the full message body using a mutator.
tags:
- Encryption
related:
- nservicebus/security/encryption
- nservicebus/pipeline/message-mutators
---


## Introduction

This Sample shows how to use `IMutateTransportMessages` to (fake) encrypt/decrypt the binary data of a message as it passed through the pipeline.


## Run the solution.

Set both `Endpoint1` and `Endpoint2` as projects and run the solution.


### Endpoint1 

Which encrypts and sends the message.


### Endpoint2 

Which receives and decrypts the message.


## Code walk-through


### The message contract

Starting with the Shared project, open the `CompleteOrder.cs` file and look at the following code:

<!-- import Message --> 

Note that is does not need any custom property types to be encrypted.
 

### How is encryption configured. 

Open either one of the `Program.cs`. You will notice the line 

    busConfiguration.RegisterMessageEncryptor();

This is a extension method that adds a `IMutateTransportMessages` to the configuration.

<!-- import MessageEncryptorExtension --> 


#### The Mutator

WARNING: Clearly this it not true encryption. It is only doing a byte array reversal to illustrate the API. In your production system you should do true encryption via the [.NET Framework Cryptography Model](https://msdn.microsoft.com/en-us/library/0ss79b2x.aspx). 

snippet:Mutator
