Running the solution starts two console applications. `Endpoint1` encrypts a message and sends it, while `Endpoint2` receives the encrypted message and decrypts it.


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

Encryption is enabled by calling an extension method in `Program.cs` for both `Endpoint1` and `Endpoint2`:

snippet: enableEncryption

The extension method is implemented in `Shared/EncryptionExtensions.cs`:

snippet: ConfigureEncryption


### The message on the wire

The serialized message content can be seen by running `Endpoint1` without running `Endpoint2`.

Messages are queued in the `.learningtransport` folder next to the solution. The message will be [contained in a file](/transports/learning/viewing-messages.md) in the `Samples.Encryption.Endpoint2` sub-folder with the following content (XML namespaces removed for clarity):

```xml
<?xml version="1.0" encoding="UTF-8"?>
<MessageWithSecretData>
   <Secret>
      <EncryptedValue>
         <EncryptedBase64Value>VOQk8pvlMdpdAgQiJldg2WZQCL86FxFMEd0VsTydOSw=</EncryptedBase64Value>
         <Base64Iv>4OnlFC1WyhTmkDLyfOdnYQ==</Base64Iv>
      </EncryptedValue>
   </Secret>
   <SubProperty>
      <Secret>
         <EncryptedValue>
            <EncryptedBase64Value>uEjQePtNlhkWEr5QHgiLbA==</EncryptedBase64Value>
            <Base64Iv>kh5C9W9picaOZ5dhz4adlA==</Base64Iv>
         </EncryptedValue>
      </Secret>
   </SubProperty>
   <CreditCards>
      <CreditCardDetails>
         <ValidTo>2024-03-08T21:08:34.091063Z</ValidTo>
         <Number>
            <EncryptedValue>
               <EncryptedBase64Value>Iv621YNDox3pd1zIbkeRrA==</EncryptedBase64Value>
               <Base64Iv>VPrVGB888YmKhi8lgkNFtg==</Base64Iv>
            </EncryptedValue>
         </Number>
      </CreditCardDetails>
      <CreditCardDetails>
         <ValidTo>2025-03-08T21:08:34.093907Z</ValidTo>
         <Number>
            <EncryptedValue>
               <EncryptedBase64Value>WY69+QzkqqKJ6UYCemShUg==</EncryptedBase64Value>
               <Base64Iv>BFLz5jz0DdhJNK01MFrMmA==</Base64Iv>
            </EncryptedValue>
         </Number>
      </CreditCardDetails>
   </CreditCards>
</MessageWithSecretData>
```
