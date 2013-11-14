<!--
title: "Auditing With NServiceBus"
tags: 
-->

The scalability inherent in parallel message-driven systems makes them more difficult to debug than their sequential, synchronous, and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Just tell NServiceBus where to send those messages.

Configuring auditing
--------------------

To turn on auditing, add the attribute "ForwardReceivedMessagesTo" to the UnicastBusConfig section of an endpoint's configuration file, as shown:










This configuration causes all messages arriving at the given endpoint to be forwarded to the queue called "AuditQueue" on the machine called
"AdminMachine". You can specify any queue on any machine, though only one is supported. Of course, you can forward on from those machines as well.

What you choose to do with those messages is now up to you: save them in a database, do custom logging, etc. The important thing is that you now have a centralized record of everything that is happening in your system while maintaining all the benefits of keep things distributed.

Message headers
---------------

Custom headers are attached to each message, which you can examine using a third-party tool for MSMQ.

  Key                                Description
  ---------------------------------- ---------------------------------------------
  NServiceBus.Version                Version of NServiceBus
  NServiceBus.TimeSent               When was the message sent
  NServiceBus.EnclosedMessageTypes   Massage type(s) within the envelope
  NServiceBus.ProcessingStarted      Time when processing of the message started
  NServiceBus.ProcessingEnded        Time when the processing finished
  NServiceBus.OriginatingAddress     From where did the envelope originate

Next steps
----------

Learn more about how [logging works in NServiceBus](logging-in-nservicebus.md).

