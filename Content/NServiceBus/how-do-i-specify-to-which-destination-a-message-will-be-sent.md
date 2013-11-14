<!--
title: "How to Specify to Which Destination a Message Is Sent?"
tags: ""
summary: "<p>You configure the destination for message types in <unicastbusconfig>, under <messageendpointmappings>.</p>
<p>Add one of the following:</p>
"
-->

You configure the destination for message types in <unicastbusconfig>, under <messageendpointmappings>.

Add one of the following:

<script src="https://gist.github.com/Particular/6106874.js?file=MessageMappings.xml"></script> For more information, see the [PubSub sample](https://github.com/NServiceBus/NServiceBus/tree/master/Samples/PubSub) config file.

Destinations can be QueueName@ServerName, or just QueueName if the destination is the local machine.

You can also call the following, even though it is not recommended for application-level code:

    Bus.Send(string destination, params IMessage[] msgs);

