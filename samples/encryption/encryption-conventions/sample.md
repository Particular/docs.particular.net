---
title: Unobtrusive Property Encryption
summary: Encrypting specific parts of a message using conventions.
reviewed: 2017-07-28
component: PropertyEncryption
tags:
- Encryption
- Unobtrusive
---


## Run the solution.

Two console applications will start up.


### Endpoint1

Which outputs

```
MessageWithSecretData sent.
```


### Endpoint2

Which outputs

```
I know the secret - it's 'betcha can't guess my secret'
SubSecret: My sub secret
CreditCard: 312312312312312 is valid to 3/11/2015 5:21:59 AM
CreditCard: 543645546546456 is valid to 3/11/2016 5:21:59 AM
```


## Code walk-through


### The message contract

Starting with the Shared project, open the `MessageWithSecretData.cs` file and look at the following code:

snippet: Message

Note the properties that have names prefixed with with the word "Encrypted".

### How is encryption configured.

Open either one of the `Program.cs`. Notice the line

snippet: enableEncryption

This code indicates that encryption should be enabled.

The key is then configured in the `EncryptionExtensions.cs` file using

snippet: ConfigureEncryption


### The message on the wire

Now run `Endpoint1` on its own (i.e. don't start `Endpoint2`).

partial: message