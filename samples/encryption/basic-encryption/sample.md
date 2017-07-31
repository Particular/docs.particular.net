---
title: Message Property Encryption
summary: Encrypting specific parts of a message using the message property encryption feature.
reviewed: 2017-02-08
component: PropertyEncryption
tags:
- Encryption
redirects:
- nservicebus/encryption-sample
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


### How is encryption configured.

Open either one of the `Program.cs`. Notice the line

snippet: enableEncryption

This code indicates that encryption should be enabled.

The key is then configured using the `encryptionService` variable. This allows for property encryption using the special `WireEncryptedString` property seen in the `MessageWithSecretData.cs` file.

snippet: ConfigureEncryption

### The message on the wire

Now run `Endpoint1` on its own (i.e. don't start `Endpoint2`).

Go to the server queue (called `EncryptionSampleEndpoint1`) and [view the message content](/transports/msmq/viewing-message-content-in-msmq.md).

The message will look like this:

```json
"MessageWithSecretData": {
  "Secret": {
    "EncryptedValue": {
      "EncryptedBase64Value": "+eeBont5Lzlre4cxDi8QT/M6EbAGxTerniqywbpLBVA=",
      "Base64Iv": "u8n8ds0Ssf/AdJCxpOG7AQ=="
  }
}
```
