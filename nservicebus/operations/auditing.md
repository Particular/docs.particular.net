---
title: Auditing With NServiceBus
summary: 'Tell NServiceBus where to send messages and it provides built-in message auditing for every endpoint'
tags:
- Auditing
- Forwarding Messages
redirects:
- nservicebus/auditing-with-nservicebus
---

The distributed nature of parallel message-driven systems makes them more difficult to debug than their sequential, synchronous, and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Just tell NServiceBus that you want auditing and it will capture a copy of every received message and forward it to a specified audit queue. 

It is recommended that you specify a central auditing queue for all related endpoints (i.e. endpoints that belong to the same system). By doing so you can take advantage of central auditing within a distributed system. This is also required by the Particular Service Platform and especially [ServiceControl](/servicecontrol), which consumes messages from these auditing queues. For more information, see [ServicePulse documentation](/servicepulse/).

## Configuring auditing

<!-- import ConfigureAudit -->

* `OverrideTimeToBeReceived`: Supported in Version 4 and higher. It is optional and defaults to the value of the TimeToBeReceived on the original message message.  

Note: For backwards compatibility, Version 4 still supports the attribute `ForwardReceivedMessagesTo` on `UnicastBusConfig` section, but you will receive a warning recommending that you upgrade your configuration to use `AuditConfig` section.

## Machine level configuration

Version 4 and higher support setting the error queue and the audit queue at the machine level in the registry. Use the [Set-NServiceBusLocalMachineSettings](management-using-powershell.md) PowerShell commandlet to set these values at a machine level. When set at machine level, the setting is not required in the endpoint configuration file for messages to be forwarded to the audit queue.

## Handling Audit messages

NServiceBus then forwards all messages arriving at the given endpoint to the queue called `AuditQueue` on the machine called `AdminMachine`. You can specify any queue on any machine, although only one is supported.

What you choose to do with those messages is now up to you: save them in a database, do custom logging, etc. The important thing is that you now have a centralized record of everything that is happening in your system while maintaining all the benefits of keep things distributed.

## Turning off auditing

If the auditing settings are in `app.config`, remove them or comment them out.

If the machine level auditing is turned on, clear the value for the string value `AuditQueue` under

    HKEYLOCALMACHINE\\SOFTWARE\\ParticularSoftware\\ServiceBus

If running 64 bit, in addition to the above, also clear the value for `AuditQueue` under 

    HKEYLOCALMACHINE\\SOFTWARE\\Wow6432Node\\ParticularSoftware\\ServiceBus

## Message headers

Custom headers are attached to each message. You can examine them in the audit queue using a third-party tool for MSMQ.

| Key                               | Description
|-----------------------------------|------------------------------------------------
| NServiceBus.Version               | Version of NServiceBus 
| NServiceBus.TimeSent              | When was the message sent
| NServiceBus.EnclosedMessageTypes  | Message type(s) within the envelope
| NServiceBus.ProcessingStarted     | Time when processing of the message started
| NServiceBus.ProcessingEnded       | Time when the processing finished
| NServiceBus.OriginatingAddress    | From where did the envelope originate
| NServiceBus.ProcessingEndpoint    | The endpoint that processed the message
| NServiceBus.ProcessingMachine     | The machine on which the message was processed
| NServiceBus.OriginatingAddress    | The queue that the message originated from
