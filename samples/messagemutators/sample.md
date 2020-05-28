---
title: Message Mutators
summary: Change messages by plugging custom logic in to a couple of interfaces, encrypting as required.
reviewed: 2019-09-16
component: Core
redirects:
- nservicebus/nservicebus-message-mutators-sample
related:
- nservicebus/pipeline/message-mutators
- samples/header-manipulation
---


This sample shows how to create a custom [message mutator](/nservicebus/pipeline/message-mutators.md).

### Executing the sample

 1. Run the solution.
 1. Press 's' in the window to send a valid message. Then press 'e' to send an invalid message. (The exception is expected.) The console output will look something like this:

```
Press 's' to send a valid message, press 'e' to send a failed message. To exit, 'q'

s
INFO  ValidationMessageMutator Validation succeeded for message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032
INFO  TransportMessageCompressionMutator transportMessage.Body size before compression: 9787013
INFO  TransportMessageCompressionMutator transportMessage.Body size after compression: 9761
INFO  ValidationMessageMutator Validation succeeded for message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032
INFO  Handler Received a CreateProductCommand message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032

e
ERROR ValidationMessageMutator Validation failed for message CreateProductCommand: ProductId=XJ128, ProductName=Milk Milk Milk Milk Milk, ListPrice=15 Image (length)=7340032, with the following error/s:
The Product Name value cannot exceed 20 characters.
The field ListPrice must be between 1 and 5.
```

## Code walk-through

### Logical message mutators

The `IMutateIncomingMessages` and `IMutateOutgoingMessages` interfaces give access to the message so that the inbound and/or outbound message can be modified.

This sample implements two mutators, which validate all DataAnnotations attributes on both incoming or outgoing messages and throw an exception if the validation fails.

snippet: ValidationMessageMutator

partial: imessagemutator

Although not shown here, it is possible to mutate only certain types of messages by checking the type of the message object received as a parameter to the method.

### Transport message mutators

Similar interfaces exist for transport messages, i.e. `IMutateIncomingTransportMessages` and `IMutateOutgoingTransportMessages`. The main difference from the logical message mutators is that the transport message may have several messages in a single transport message.

This transport mutator compresses the outgoing transport message and decompresses the incoming message.

snippet: TransportMessageCompressionMutator

partial: imutatetransportmessages

The `TransportMessageCompressionMutator` is a transport message mutator, meaning that NServiceBus allows the mutation of the outgoing and incoming transport message.

The compression code uses the .NET framework [GZipStream](https://msdn.microsoft.com/en-us/library/system.io.compression.gzipstream.aspx) class to do the compression. After the compression is done, the compressed array is placed back in the transport message Body property. The sample then signals to the receiver that the transport message was mutated (compressed) by setting the header key `IWasCompressed` to "true". The incoming mutator uses this key to determine whether or not to mutate (decompress) the message.

## Configuring NServiceBus to use the message mutators

To hook the sample message mutators into NServiceBus messaging flow:

snippet: ComponentRegistration


## Sending a valid message

The message sent in the sample includes a 7MB image:

snippet: SendingSmall

partial: msmq

## Sending an invalid message

The sample sends a similar message but with data that fails the logical message mutator's validation:

snippet: SendingLarge

The message is invalid for several reasons: the product name is over the 20 character limit, the list price is too high, and the sell end date is not in the valid range. The exception logs those invalid values.

## Receiving a message

The receiver is straightforward. There is no special code to handle validation or compression since this is done by the logical and transport message mutators.

snippet: Handler