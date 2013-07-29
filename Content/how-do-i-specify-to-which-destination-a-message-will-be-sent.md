---
layout:
title: "How to Specify to Which Destination a Message Is Sent?"
tags: 
origin: http://www.particular.net/Articles/how-do-i-specify-to-which-destination-a-message-will-be-sent
---
You configure the destination for message types in <unicastbusconfig>, under <messageendpointmappings>.

Add one of the following:

    To register all message types defined in an assembly use either of the following:

To specify a fully qualified type, use either of the following:



    You can also filter by namespace when defining a mapping like this. 


    NOTE: The mapping only subscribes to messages defined in the MyMessages.Other namespace":

For more information, see the [PubSub sample](https://github.com/NServiceBus/NServiceBus/tree/master/Samples/PubSub) config file.

Destinations can be QueueName@ServerName, or just QueueName if the destination is the local machine.

You can also call the following, even though it is not recommended for application-level code:

    Bus.Send(string destination, params IMessage[] msgs);

