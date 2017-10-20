---
title: Newtonsoft Json Encryption
summary: Encrypting specific parts of a message using the message property encryption.
reviewed: 2017-09-27
component: NewtonsoftEncryption
tags:
- Encryption
---

Leverages the [NServiceBus.Newtonsoft.Encryption](https://github.com/SimonCropp/Newtonsoft.Json.Encryption) extension to Encrypting specific nodes of a serialized message. This is done using the extension points of [Json.NET](https://www.newtonsoft.com/json), and as such is more efficient (in terms of memory and CPU) than the [Message Property Encryption component](/nservicebus/security/property-encryption.md).


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


### Encryption configuration

Open either one of the `Program.cs`. Notice the line

snippet: enableEncryption

This code indicates that encryption should be enabled.

The key is then configured in the `EncryptionExtensions.cs` file using

snippet: ConfigureEncryption


### The message on the wire

Now run `Endpoint1` on its own (i.e. don't start `Endpoint2`).

Open the .learningtransport folder for `Samples.Encryption.Endpoint2` and [view the message content](/transports/learning/viewing-messages.md).

The message will look like this:

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