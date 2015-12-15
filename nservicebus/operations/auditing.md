---
title: Auditing Messages
summary: 'Tell NServiceBus where to send messages and it provides built-in message auditing for every endpoint'
tags:
- Auditing
- Forwarding Messages
redirects:
- nservicebus/auditing-with-nservicebus
---

The distributed nature of parallel message-driven systems makes them more difficult to debug than their sequential, synchronous, and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Just tell NServiceBus that you want auditing and it will capture a copy of every received message and forward it to a specified audit queue.

It is recommended that you specify a central auditing queue for all related endpoints (i.e. endpoints that belong to the same system). By doing so you can take advantage of central auditing within a distributed system. This is also required by the Particular Service Platform and especially [ServiceControl](/servicecontrol), which consumes messages from these auditing queues. For more information, see [ServicePulse documentation](/servicepulse/).


## Handling Audit messages

What you choose to do with those messages is now up to you: save them in a database, do custom logging, etc. The important thing is that you now have a centralized record of everything that is happening in your system while maintaining all the benefits of keep things distributed.


## Audit configuration options

There two settings that control Auditing.


### Queue Name

The queue name to forward audit messages to


### OverrideTimeToBeReceived

You can force a [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md) on audit messages by setting `OverrideTimeToBeReceived` using the configuration syntax below.

Note that while the phrasing is "forwarding a message" in the implementation it is actually "cloning and sending a new message". This is important when considering TimeToBeReceived since the time taken to receive and process the original message is not part of the TimeToBeReceived of the new audit message. So in effect the audit message receives the full time allotment of whatever TimeToBeReceived is used.


#### Default Value

If no OverrideTimeToBeReceived is defined then:

**Version 5 and lower**: TimeToBeReceived of the original message will be used.

**Version 6 and higher**: No TimeToBeReceived will be set.


## Configuring auditing


### Using Code

You can configure the target audit queue using the configuration API.

snippet:AuditWithCode


### Using app.config

snippet:configureAuditUsingXml

Note: `OverrideTimeToBeReceived` needs to be a valid [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx).


### Using IProvideConfiguration

The audit settings can also be configured using code via a  [custom configuration provider](/nservicebus/hosting/custom-configuration-providers.md)

snippet:AuditProvideConfiguration


## Machine level configuration

Version 4 and higher support setting the error queue and the audit queue at the machine level in the registry. Use the [Set-NServiceBusLocalMachineSettings](management-using-powershell.md) PowerShell commandlet to set these values at a machine level. When set at machine level, the setting is not required in the endpoint configuration file for messages to be forwarded to the audit queue.


## Turning off auditing

If the auditing settings are in `app.config`, remove them or comment them out.

If the machine level auditing is turned on, clear the value for the string value `AuditQueue` under

    HKEYLOCALMACHINE\\SOFTWARE\\ParticularSoftware\\ServiceBus

If running 64 bit, in addition to the above, also clear the value for `AuditQueue` under

    HKEYLOCALMACHINE\\SOFTWARE\\Wow6432Node\\ParticularSoftware\\ServiceBus


## Message headers

All headers, from the message being forwarded, will be included in the message sent to the audit queue. There are also some custom audit headers appended. See [Message Headers](/nservicebus/operations/auditing.md) for more information.
