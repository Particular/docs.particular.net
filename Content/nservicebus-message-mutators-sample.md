---
layout:
title: "NServiceBus Message Mutators Sample"
tags: 
origin: http://www.particular.net/Articles/nservicebus-message-mutators-sample
---
In NServiceBus V2.6 it was tricky to change messages as they were sent to and from endpoints. From NServiceBus V3 you can change messages by plugging custom logic in to a couple of simple interfaces.

You can encrypt all or part of a message. The encryption message mutator is part of the NServiceBus library, and can be used at any time.

To see MessageMutators in action, open the MessageMutator sample.

1.  Run the solution.

     Two console applications start up.
2.  Find the client application by looking for the one with "Client" in
    its path and pressing 's' and 'Enter' in the window. Then press 'e'
    followed by 'Enter'.

     Your screen should look something like this (the exception message
    is expected):

![Message Mutator sample Running](https://particular.blob.core.windows.net/media/Default/images/MessageMutatorsRunning.png "Message Mutator sample Running")

Now let's look at the code.

![Message mutaturs sample](https://particular.blob.core.windows.net/media/Default/images/MessageMutatorSolutionExplorer.png "Message mutaturs sample")

Code walk-through
-----------------

This sample shows how to create a custom message mutator.

Take a quick look at the interfaces involved. ![Message Mutators](https://particular.blob.core.windows.net/media/Default/images/MessageMutators.png "Message Mutators")

Each interface gives access to the message so that you can mutate on the inbound and/or outbound message.

All you have to do as a consumer is implement the desired interface and load it into the NServiceBus container.

Similar interfaces exist for IMessageMutator, i.e., IMutateTransportMessages, which mutates transport messages. The main difference from IMessageMutator is that the transport message may have several messages in a single transport message.

This sample implements two mutators:

-   ValidationMessageMutator: This message mutator validates all
    DataAnnotations attributes that exist in the message.

-   TransportMessageCompressionMutator: This transport mutator
    compresses the whole transport message.

Let's look at the MessageMutators Assembly.

Message mutators assembly
-------------------------

Both interfaces are implemented in the MessageMutators project.

### ValidationMessageMutator

Examine the implementation of ValidationMessageMutator:

    public class ValidationMessageMutator : IMessageMutator
    {
    private static readonly ILog Logger = LogManager.GetLogger("ValidationMessageMutator");
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
    private static void ValidateDataAnnotations(Object message)
    {
    var context = new ValidationContext(message, null, null);
    var results = new List();
    var isValid = Validator.TryValidateObject(message, context, results, true);
    if (isValid)
    {
    Logger.Info("Validation succeeded for message: " + message.ToString());
    return;
    }
    var errorMessage = new StringBuilder();
    errorMessage.Append(
    string.
    Format("Validation failed for message {0}, with the following error/s: " + 
    Environment.NewLine, message.ToString()));
    foreach (var validationResult in results)
    errorMessage.Append(validationResult.ErrorMessage + Environment.NewLine);
    Logger.Error(errorMessage.ToString());
    throw new Exception(errorMessage.ToString());
    }
    }

ValidationMessageMutator implements the two interface methods: outgoing and incoming. As can be seen in the code, both incoming and outgoing mutaturs have the exact same code in them. The mutation is symmetrical.

Both call a private static method called ValidateDataAnnotations.

This means that both the outgoing message and incoming message will be validated. The mutator is working on all incoming/outgoing message types.

It is possible to examine the message type and mutate only certain types of messages by checking the type of the message object received as a parameter to the method.

Browse the code. It shows a standard way to test data annotations. In this sample, if one of the validation fails, an exception is thrown, detailing the 'broken' validation.

### TransportMessageCompressionMutator

Examine the other message mutator, the TransportMessageCompressionMutator:

    public class TransportMessageCompressionMutator : IMutateTransportMessages
    {
    private static readonly ILog Logger = 
    LogManager.GetLogger("TransportMessageCompressionMutator");
    public void MutateOutgoing(object[] messages, TransportMessage transportMessage)
    {
    Logger.Info("transportMessage.Body size before compression: " 
    + transportMessage.Body.Length);            
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
    Logger.Info("transportMessage.Body size after compression: " 
    + transportMessage.Body.Length);
    }
    public void MutateIncoming(TransportMessage transportMessage)
    {
    if (!transportMessage.Headers.ContainsKey("IWasCompressed")) 
    return;
    using (var bigStream = 
    new GZipStream(new MemoryStream(transportMessage.Body), CompressionMode.Decompress))
    {
    var bigStreamOut = new MemoryStream();
    bigStream.CopyTo(bigStreamOut);
    transportMessage.Body = bigStreamOut.ToArray();
    }
    }
    }

The TransportMessageCompressionMutator is a transport message mutator, meaning that NServiceBus allows you to mutate the outgoing or/and incoming transport message.

In the TransportMessageCompressionMutator class, both the incoming and outgoing methods are implemented.

In this sample, the outgoing method (compression) is executed by the client (sender) AppDomain, while the incoming method (Decompress) is executed by the Server (receiver) AppDomain.

This mutator is acting on all transport messages, regardless of what message types the transport message carries.

The compression code is straightforward and utilizes the .NET framework
[GZipStream](http://msdn.microsoft.com/en-us/library/system.io.compression.gzipstream.aspx) class to do the compression.

After the compression is done, the compressed array is placed back in the transport message Body property.

This sample signals to the receiving end that this transport message was mutated (compressed) by placing a "true" string in the header key
"IWasCompressed".

Decompression is done in the incoming method if the key "IWasCompressed" exists.

If the key is missing, the message is returned, unmutated.

Otherwise, the incoming method is replacing the transport message Body compressed content an uncompressed one.

Now all we have to do it hook those two mutators into the NServiceBus message flow.

Configuring NServiceBus to use the message mutators
---------------------------------------------------

To hook the sample message mutators into NServiceBus messaging flow:

    public class HookMyMessageMutators : IWantCustomInitialization
    {
    public void Init()
    {
    Configure.Instance.Configurer.ConfigureComponent(
    DependencyLifecycle.InstancePerCall);
    Configure.Instance.Configurer.ConfigureComponent(
    DependencyLifecycle.InstancePerCall);
    }
    }

Implementing IWantCustomInitialization signals NServiceBus to call the Init method during the NServiceBus initialization phase.

The Init method configures, using NServiceBus builder ([dependency injection mechanism](containers)) to use ValidationMessageMutator and TransportMessageCompressionMutators. The NServiceBus framework uses them in its messaging flow.

Since the HookMyMessageMutators class is defined in the MessageMutators class library assembly, it means that you can drop it in the Client and the Server executable folder, and mutation will happen automatically.

Simply dropping the MessageMutators assembly in the executable folder means that that the client and server are agnostic to the fact that message mutation is being executed and to its nature. The message mutation can be replaced, updated, and removed without the client and server knowing about it.

Client and server code
----------------------

Since registration is done automatically by the framework, the server and client code are NServiceBus standard for sending and handling a message. There is nothing special here.

This is how the client sends a valid message:

    Bus.Send(m =>
    {
    m.ProductId = "XJ128";
    m.ProductName= "Milk";
    m.ListPrice = 4;
    m.SellEndDate = new DateTime(2012, 1, 3);
    // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
    // before it reaches MSMQ.
    m.Image = new byte[1024 * 1024 * 7];
    });

Since the message buffer field is empty, GZipStreamer in the outgoing transport message mutator easily compresses it to a size under the MSMQ limit of 4MB and the message will get to the server.

See how the client sends an invalid message that will never reach the server since an exception will be thrown at the outgoing message mutator:

    Bus.Send(m =>
    {
    m.ProductId = "XJ128";
    m.ProductName = "Milk Milk Milk Milk Milk";
    m.ListPrice = 15;
    m.SellEndDate = new DateTime(2011, 1, 3);
    // 7MB. MSMQ should throw an exception, but it will not since the buffer will be compressed 
    // before it reaches MSMQ.
    m.Image = new byte[1024 * 1024 * 7];
    });

The message is invalid for several reasons: the product name is over the
20 character limit, the list price is too high, and the sell end date is not in the valid range. The thrown exception logs those invalid values. The server code is simple and straightforward:

    public class Handler : IHandleMessages
    {
    public void Handle(CreateProductCommand createProductCommand)
    {
    Console.WriteLine("Received a CreateProductCommand message: " + createProductCommand);
    }
    }

The server handler code does not need to change on account of the message mutation.

This article was based on an article written by [Adam Fyles](http://adamfyles.blogspot.com/). See the [original blog post](http://adamfyles.blogspot.com/2011/02/nservicebus-30-message-mutators.html).

Where to next?
--------------

It might be a good idea now to cover the [unobtrusive mode](unobtrusive-sample) subject.

