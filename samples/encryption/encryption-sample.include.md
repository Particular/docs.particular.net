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

```xml
<?xml version="1.0"?>
<MessageWithSecretData>
  <EncryptedSecret>zoksP3QrtMqMmnXyShnvaLEq3n/6DA2f/7d6DDtwzXo=@u5THG1mtftg6+QAEsRh21g==</EncryptedSecret>
  <SubProperty>
    <EncryptedSecret>bmWpBtnYu0ira0Ke6+4YEQ==@zhLAqIx+qjwLFD1VGg78Bw==</EncryptedSecret>
  </SubProperty>
  <CreditCards>
    <CreditCardDetails>
      <ValidTo>2018-07-28T13:52:10.9062784Z</ValidTo>
      <EncryptedNumber>FMApSVh9UEIYcE75VWvYUw==@7z6A1A/I/w5lACPbMwxoKg==</EncryptedNumber>
    </CreditCardDetails>
    <CreditCardDetails>
      <ValidTo>2019-07-28T13:52:10.9072791Z</ValidTo>
      <EncryptedNumber>KLWeyjogoNfZS1mblvcOMw==@St/nXNacedk5rW4GOwzg/A==</EncryptedNumber>
    </CreditCardDetails>
  </CreditCards>
</MessageWithSecretData>
```
