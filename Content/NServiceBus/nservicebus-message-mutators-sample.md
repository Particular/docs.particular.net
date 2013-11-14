<!--
title: "NServiceBus Message Mutators Sample"
tags: ""
summary: "<p>In NServiceBus V2.6 it was tricky to change messages as they were sent to and from endpoints. From NServiceBus V3 you can change messages by plugging custom logic in to a couple of simple interfaces.</p>
<p>You can encrypt all or part of a message. The encryption message mutator is part of the NServiceBus library, and can be used at any time.</p>
"
-->

In NServiceBus V2.6 it was tricky to change messages as they were sent to and from endpoints. From NServiceBus V3 you can change messages by plugging custom logic in to a couple of simple interfaces.

You can encrypt all or part of a message. The encryption message mutator is part of the NServiceBus library, and can be used at any time.

To see MessageMutators in action, open the [MessageMutator sample](https://github.com/NServiceBus/NServiceBus/tree/3.3.8/Samples/MessageMutators)
.

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

<script src="https://gist.github.com/Particular/6144218.js?file=ValidationMessageMutator.cs"></script> ValidationMessageMutator implements the two interface methods: outgoing and incoming. As can be seen in the code, both incoming and outgoing mutaturs have the exact same code in them. The mutation is symmetrical.

Both call a private static method called ValidateDataAnnotations.

This means that both the outgoing message and incoming message will be validated. The mutator is working on all incoming/outgoing message types.

It is possible to examine the message type and mutate only certain types of messages by checking the type of the message object received as a parameter to the method.

Browse the code. It shows a standard way to test data annotations. In this sample, if one of the validation fails, an exception is thrown, detailing the 'broken' validation.

### TransportMessageCompressionMutator

Examine the other message mutator, the TransportMessageCompressionMutator:

<script src="https://gist.github.com/Particular/6144218.js?file=TransportMessageCompressionMutator.cs"></script> The TransportMessageCompressionMutator is a transport message mutator, meaning that NServiceBus allows you to mutate the outgoing or/and incoming transport message.

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

<script src="https://gist.github.com/Particular/6144218.js?file=MutatorInit.cs"></script> Implementing IWantCustomInitialization signals NServiceBus to call the Init method during the NServiceBus initialization phase.

The Init method configures, using NServiceBus builder ( [dependency injection mechanism](containers.md) ) to use ValidationMessageMutator and TransportMessageCompressionMutators. The NServiceBus framework uses them in its messaging flow.

Since the HookMyMessageMutators class is defined in the MessageMutators class library assembly, it means that you can drop it in the Client and the Server executable folder, and mutation will happen automatically.

Simply dropping the MessageMutators assembly in the executable folder means that that the client and server are agnostic to the fact that message mutation is being executed and to its nature. The message mutation can be replaced, updated, and removed without the client and server knowing about it.

Client and server code
----------------------

Since registration is done automatically by the framework, the server and client code are NServiceBus standard for sending and handling a message. There is nothing special here.

This is how the client sends a valid message:

<script src="https://gist.github.com/Particular/6144218.js?file=ClientSend1.cs"></script> Since the message buffer field is empty, GZipStreamer in the outgoing transport message mutator easily compresses it to a size under the MSMQ limit of 4MB and the message will get to the server.

See how the client sends an invalid message that will never reach the server since an exception will be thrown at the outgoing message mutator:

<script src="https://gist.github.com/Particular/6144218.js?file=ClientSend2.cs"></script> The message is invalid for several reasons: the product name is over the
20 character limit, the list price is too high, and the sell end date is not in the valid range. The thrown exception logs those invalid values. The server code is simple and straightforward:

<script src="https://gist.github.com/Particular/6144218.js?file=ClientMessageHandler.cs"></script> The server handler code does not need to change on account of the message mutation.

This article was based on an article written by [Adam Fyles](http://adamfyles.blogspot.com/) . See the [original blog post](http://adamfyles.blogspot.com/2011/02/nservicebus-30-message-mutators.html)
.

Where to next?
--------------

It might be a good idea now to cover the [unobtrusive mode](unobtrusive-sample.md) subject.

