---
layout:
title: "Pipeline Management Using Message Mutators"
tags: 
origin: http://www.particular.net/Articles/pipeline-management-using-message-mutators
---
The message pipeline in NServiceBus V2.X consisted of message modules. They served their purpose but didn’t quite give full control over the message pipeline for more advanced things, and there was no way to hook into the pipeline at the sending/client side of the message conversation.

The [DataBus feature](attachments-databus-sample) of NServiceBus V3.0 uses message mutators to change the content of a message before and after sending it on the wire and act as fine-grained hooks into the pipeline. See how to use them.

Two flavors of mutators
-----------------------

NServiceBus enables two types of message mutators:

-   Applicative Message Mutators

    Message mutators change/react to individual messages being sent or
    received. The
    [IMessageMutator](https://github.com/NServiceBus/NServiceBus/blob/master/src/messagemutator/NServiceBus.MessageMutator/IMessageMutator.cs)
    interface lets you implement hooks for the sending and receiving
    sides. If you only need one, use the finely grained
    [IMutateOutgoingMessages](https://github.com/NServiceBus/NServiceBus/blob/master/src/messagemutator/NServiceBus.MessageMutator/IMessageMutator.cs)
    or
    [IMutateIncomingMessages](https://github.com/NServiceBus/NServiceBus/blob/master/src/messagemutator/NServiceBus.MessageMutator/IMessageMutator.cs).

     You can use reactions to individual messages to perform actions
    such as validation of outgoing/incoming messages. The [Message
    Mutators Sample](nservicebus-message-mutators-sample) puts it into
    action.

     NServiceBus uses this type of mutator internally to do things like
    property encryption and serialization/deserialization of properties
    to and from the DataBus.

-   Transport Messages Mutators

    Create transport message mutators by implementing the
    [IMutateTransportMessages](https://github.com/NServiceBus/NServiceBus/blob/master/src/messagemutator/NServiceBus.MessageMutator/IMutateTransportMessages.cs)
    interface. This type of mutator works on the entire transport
    message and is useful for compression, header manipulation, etc. See
    a[full explanation of the
    syntax](nservicebus-message-mutators-sample).

     Remember that message mutators are NOT automatically registered in
    the container, so to invoke them, register them in the container
    yourself.

When should I use a message mutator?
------------------------------------

Just like the recommendation for headers, only use message mutators for infrastructure purposes.

As a rule of thumb, consider using message mutators only to solve technical requirements.

What happens if a mutator throws an exception?
----------------------------------------------

If a server side (incoming) mutator throws an exception, the message aborts, rolls back to the queue, and is retried.

If a client side (outgoing) message mutator throws an exception, the exception bubbles up to the method calling bus.Send or bus.Publish.

