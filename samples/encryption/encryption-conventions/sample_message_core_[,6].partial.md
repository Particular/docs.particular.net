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