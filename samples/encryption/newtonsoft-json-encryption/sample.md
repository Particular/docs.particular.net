---
title: Newtonsoft.Json encryption
summary: Encrypting specific parts of a message using the message property encryption.
reviewed: 2018-12-11
component: NewtonsoftEncryption
tags:
- Encryption
---

This sample demonstrates how to use the [NServiceBus.Newtonsoft.Encryption](https://www.nuget.org/packages/NServiceBus.Newtonsoft.Encryption) package to encrypt and decrypt specific properties of a message as it passes through the pipeline. This method of encryption uses extension points in [Json.NET](https://www.newtonsoft.com/json) and requires less memory and CPU processing than [message property encryption](/nservicebus/security/property-encryption.md).

Running the solution starts two console applications. `Endpoint1` encrypts a message and sends it and `Endpoint2` receives the encrypted message and decrypts it.

### Endpoint1 output

```
MessageWithSecretData sent.
```


### Endpoint2 output

```
I know the secret - it's 'betcha can't guess my secret'
SubSecret: My sub secret
CreditCard: 312312312312312 is valid to 3/11/2015 5:21:59 AM
CreditCard: 543645546546456 is valid to 3/11/2016 5:21:59 AM
```


## Code walk-through


### Message contract

The `Shared` project contains `MessageWithSecretData.cs`, which defines the message contract:

snippet: Message


### Encryption configuration

Encryption is enabled by calling an extension method in `Program.cs` in both `Endpoint1` and `Endpoint2`:

snippet: enableEncryption

The extension method is in `Shared/EncryptionExtensions.cs`:

snippet: ConfigureEncryption


### The message on the wire

The serialized message content can be seen by running `Endpoint1` without running `Endpoint2`.

Messages are queued in the `.learningtransport` folder next to the solution. The message will be [contained in a file](/transports/learning/viewing-messages.md) in the `Samples.Encryption.Endpoint2` sub-folder with the following content:

```json
{
   "Secret":"L6sv2UjRckKcO5sYgeLTtOZSM9XEVzjKMgL8HkAqp4s=",
   "SubProperty":{
      "Secret":"QyQCkTOYtpFYVOo7XH8cEw=="
   },
   "CreditCards":[
      {
         "ValidTo":"2018-09-27T13:23:40.0659704Z",
         "Number":"Ne3i4KW+1o99XqowLpy8fw=="
      },
      {
         "ValidTo":"2019-09-27T13:23:40.0659704Z",
         "Number":"GBHwR51fV/56ez2b9ZRfwg=="
      }
   ]
}
```
