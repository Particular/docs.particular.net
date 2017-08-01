
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
