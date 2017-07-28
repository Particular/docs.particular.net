Open the .learningtransport folder for `Samples.Encryption.Endpoint2` and [view the message content](/transports/learning/viewing-messages.md).

The message will look like this:

```xml
<?xml version="1.0"?>
<MessageWithSecretData xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://tempuri.net/">
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