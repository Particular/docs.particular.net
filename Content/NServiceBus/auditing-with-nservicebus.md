---
title: "Auditing With NServiceBus"
tags: ""
summary: "The scalability inherent in parallel message-driven systems makes them more difficult to debug than their sequential, synchronous, and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Just tell NServiceBus where to send those messages."
---

The scalability inherent in parallel message-driven systems makes them more difficult to debug than their sequential, synchronous, and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Just tell NServiceBus where to send those messages.

Configuring auditing
--------------------

To turn on auditing, add the attribute "ForwardReceivedMessagesTo" to the UnicastBusConfig section of an endpoint's configuration file, as shown:


```XML
<UnicastBusConfig 
      ForwardReceivedMessagesTo="The address to which messages received will be forwarded."
      TimeToBeReceivedOnForwardedMessages="The time to be received set on forwarded messages, specified as a timespan see http://msdn.microsoft.com/en-us/library/vstudio/se73z7b9.aspx">
  <MessageEndpointMappings>
    <!-- rest of your configuration here -->
  </MessageEndpointMappings>
</UnicastBusConfig>
```

 This configuration causes all messages arriving at the given endpoint to be forwarded to the queue called "AuditQueue" on the machine called
"AdminMachine". You can specify any queue on any machine, though only one is supported. Of course, you can forward on from those machines as well.

What you choose to do with those messages is now up to you: save them in a database, do custom logging, etc. The important thing is that you now have a centralized record of everything that is happening in your system while maintaining all the benefits of keep things distributed.

Version 4.0 supports setting up the error queue and the audit queue at a machine level. And these are stored in HKLM in the registry. Use the
<span style="font-weight: 600;">[Set-NServiceBusLocalMachineSettings](managing-nservicebus-using-powershell.md)
</span>powershell commandlet to set these values at a machine level. When set at a machine level, the attribute "ForwardReceivedMessagesTo" in the UnicastBusConfig is not required for messages to be forwarded to the audit queue.

**To turn auditing off:**

If the auditing attributes are set in the app.config, then delete the attributes.

If the machine level auditing is turned on, clear out the value for the string value AuditQueue under:

HKEYLOCALMACHINE\\SOFTWARE\\ParticularSoftware\\ServiceBus

If running 64 bit, in addition to the above, also clear out the value for AuditQueue under:


HKEYLOCALMACHINE\\SOFTWARE\\Wow6432Node\\ParticularSoftware\\ServiceBus

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

Learn more about how [logging works in NServiceBus](logging-in-nservicebus.md) .

