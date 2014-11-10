---
title: NServiceBus Message Mutators Sample
summary: 'Change messages by plugging custom logic in to a couple of interfaces, encrypting as required. '
tags:
- Mutator
- Changing message content
---

From NServiceBus V3 you can intercept and change messages by plugging custom logic into a couple of simple interfaces.

To see MessageMutators in action, open the [MessageMutator sample](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/MessageMutators).

1.  Run the solution.
2.  Press 's' and 'Enter' in the window. Then press 'e' followed by 'Enter'.
    The Console  output will look something like this (the exception message is expected):

```
Press 's' to send a valid message, press 'e' to send a failed message. To exit, 'q'

s
2014-11-04 16:49:41.338 INFO  ValidationMessageMutator Validation succeeded for message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032
2014-11-04 16:49:41.438 INFO  TransportMessageCompressionMutator transportMessage.Body size before compression: 9787013
2014-11-04 16:49:41.559 INFO  TransportMessageCompressionMutator transportMessage.Body size after compression: 9761
2014-11-04 16:49:43.879 INFO  ValidationMessageMutator Validation succeeded for message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032
2014-11-04 16:49:43.887 INFO  Handler Received a CreateProductCommand message: CreateProductCommand: ProductId=XJ128, ProductName=Milk, ListPrice=4 Image (length)=7340032

e
2014-11-04 16:55:17.997 ERROR ValidationMessageMutator Validation failed for message CreateProductCommand: ProductId=XJ128, ProductName=Milk Milk Milk Milk Milk, ListPrice=15 Image (length)=7340032, with the following error/s:
The Product Name value cannot exceed 20 characters.
The field ListPrice must be between 1 and 5.

```

Now let's look at the code.

## Code walk-through

This sample shows how to create a custom message mutator.

Take a quick look at the interfaces involved. 

![Message Mutators](MessageMutators.png "Message Mutators")

Each interface gives access to the message so that you can mutate on the inbound and/or outbound message.

All you have to do as a consumer is implement the desired interface and load it into the NServiceBus container.

Similar interfaces exist for `IMessageMutator`, i.e., `IMutateTransportMessages`, which mutates transport messages. The main difference from `IMessageMutator` is that the transport message may have several messages in a single transport message.

This sample implements two mutators:

### ValidationMessageMutator

This message mutator validates all DataAnnotations attributes that exist in the message.

```C#
public class ValidationMessageMutator : IMessageMutator
{
    static ILog Logger = LogManager.GetLogger("ValidationMessageMutator");

    public object MutateOutgoing(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    public object MutateIncoming(object message)
    {
        ValidateDataAnnotations(message);
        return message;
    }

    static void ValidateDataAnnotations(object message)
    {
        var context = new ValidationContext(message, null, null);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(message, context, results, true);

        if (isValid)
        {
            Logger.Info("Validation succeeded for message: " + message);
            return;
        }

        var errorMessage = new StringBuilder();
        var error = string.Format("Validation failed for message {0}, with the following error/s: " + Environment.NewLine,message);
        errorMessage.Append(error);

        foreach (var validationResult in results)
        {
            errorMessage.Append(validationResult.ErrorMessage + Environment.NewLine);
        }

        Logger.Error(errorMessage.ToString());
        throw new Exception(errorMessage.ToString());
    }
}
```

`ValidationMessageMutator` implements the two interface methods: outgoing and incoming. As can be seen in the code, both incoming and outgoing mutators have the exact same code in them. The mutation is symmetrical.

Both call a private static method called `ValidateDataAnnotations`.

This means that both the outgoing message and incoming message will be validated. The mutator is working on all incoming/outgoing message types.

It is possible to examine the message type and mutate only certain types of messages by checking the type of the message object received as a parameter to the method.

Browse the code. It shows a standard way to test data annotations. In this sample, if one of the validation fails, an exception is thrown, detailing the 'broken' validation.

### TransportMessageCompressionMutator

This transport mutator compresses the whole transport message.

```C#
public class TransportMessageCompressionMutator : IMutateTransportMessages
{
    static ILog Logger = LogManager.GetLogger("TransportMessageCompressionMutator");

    public void MutateOutgoing(LogicalMessage message, TransportMessage transportMessage)
    {
        Logger.Info("transportMessage.Body size before compression: " + transportMessage.Body.Length);

        var mStream = new MemoryStream(transportMessage.Body);
        var outStream = new MemoryStream();

        using (var tinyStream = new GZipStream(outStream, CompressionMode.Compress))
        {
            mStream.CopyTo(tinyStream);
        }
        // copy the compressed buffer only after the GZipStream is disposed, 
        // otherwise, not all the compressed message will be copied.
        transportMessage.Body = outStream.ToArray();
        transportMessage.Headers["IWasCompressed"] = "true";
        Logger.Info("transportMessage.Body size after compression: " + transportMessage.Body.Length);
    }

    public void MutateIncoming(TransportMessage transportMessage)
    {
        if (!transportMessage.Headers.ContainsKey("IWasCompressed"))
        {
            return;
        }
        using (var bigStream = new GZipStream(new MemoryStream(transportMessage.Body), CompressionMode.Decompress))
        {
            var bigStreamOut = new MemoryStream();
            bigStream.CopyTo(bigStreamOut);
            transportMessage.Body = bigStreamOut.ToArray();
        }
    }
}
```

The `TransportMessageCompressionMutator` is a transport message mutator, meaning that NServiceBus allows you to mutate the outgoing or/and incoming transport message.

In the TransportMessageCompressionMutator class, both the incoming and outgoing methods are implemented.

This mutator is acting on all transport messages, regardless of what message types the transport message carries.

The compression code is straightforward and utilizes the .NET framework [GZipStream](http://msdn.microsoft.com/en-us/library/system.io.compression.gzipstream.aspx) class to do the compression.

After the compression is done, the compressed array is placed back in the transport message Body property.

This sample signals to the receiving end that this transport message was mutated (compressed) by placing a "true" string in the header key `IWasCompressed`.

Decompression is done in the incoming method if the key `IWasCompressed` exists.

If the key is missing, the message is returned, unmutated.

Otherwise, the incoming method is replacing the transport message Body compressed content an uncompressed one.

Now all we have to do it hook those two mutators into the NServiceBus message flow.

## Configuring NServiceBus to use the message mutators

To hook the sample message mutators into NServiceBus messaging flow:

```C#
var busConfiguration = new BusConfiguration();
busConfiguration.RegisterComponents(components =>
{
    components.ConfigureComponent<ValidationMessageMutator>(DependencyLifecycle.InstancePerCall);
    components.ConfigureComponent<TransportMessageCompressionMutator>(DependencyLifecycle.InstancePerCall);
});
```

## The Sending code

```C#
bus.SendLocal(new CreateProductCommand
              {
                  ProductId = "XJ128",
                  ProductName = "Milk",
                  ListPrice = 4,
                  // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
                  // before it reaches MSMQ.
                  Image = new byte[1024*1024*7]
              });
```

Since the message buffer field is empty, `GZipStreamer` in the outgoing transport message mutator easily compresses it to a size under the MSMQ limit of 4MB and the message will get to the server.

See how an invalid message is sent that will never be received since an exception will be thrown at the outgoing message mutator:

```C#
bus.SendLocal(new CreateProductCommand
              {
                  ProductId = "XJ128",
                  ProductName = "Milk Milk Milk Milk Milk",
                  ListPrice = 15,
                  // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
                  // before it reaches MSMQ.
                  Image = new byte[1024*1024*7]
              });
```

The message is invalid for several reasons: the product name is over the 20 character limit, the list price is too high, and the sell end date is not in the valid range. The thrown exception logs those invalid values. The server code is simple and straightforward:

```C#
public class Handler : IHandleMessages<CreateProductCommand>
{
    static ILog Log = LogManager.GetLogger<Handler>();

    public void Handle(CreateProductCommand createProductCommand)
    {
        Log.Info("Received a CreateProductCommand message: " + createProductCommand);
    }
}
```

The handler code does not need to change on account of the message mutation.