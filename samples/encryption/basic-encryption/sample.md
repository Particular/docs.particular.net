---
title: Encryption
summary: Encrypting message data.
reviewed: 2016-03-21
component: Core
tags:
- Encryption
redirects:
- nservicebus/encryption-sample
related:
- nservicebus/security/encryption
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

snippet:Message


### How is encryption configured.

Open either one of the `Program.cs`. Notice the line

snippet:enableEncryption

This code indicates that encryption should be enabled.

The key is then configured using

snippet: ConfigureEncryption


### The message on the wire

Now run `Endpoint1` on its own (i.e. don't start `Endpoint2`).

Go to the server queue (called `EncryptionSampleEndpoint1`) and examine the message in it. Read how to do this in the
[FAQ](/nservicebus/msmq/viewing-message-content-in-msmq.md).

The message will look like this:

```JSON
"MessageWithSecretData": {
  "Secret": {
    "EncryptedValue": {
      "EncryptedBase64Value": "+eeBont5Lzlre4cxDi8QT/M6EbAGxTerniqywbpLBVA=",
      "Base64Iv": "u8n8ds0Ssf/AdJCxpOG7AQ=="
  }
}
```