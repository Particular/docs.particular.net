---
title: Auditing Messages
summary: Configure where to send messages and it provides built-in message auditing for every endpoint.
reviewed: 2016-03-17
tags:
 - Audit
 - Forwarding Messages
related:
 - nservicebus/messaging/headers
 - nservicebus/messaging/discard-old-messages
redirects:
 - nservicebus/auditing-with-nservicebus
---

The distributed nature of parallel, message-driven systems make them more difficult to debug than their sequential, synchronous and centralized counterparts. For these reasons, NServiceBus provides built-in message auditing for every endpoint. Configure NServiceBus to audit and it will capture a copy of every received message and forward it to a specified audit queue.

It is recommended to specify a central auditing queue for all related endpoints (i.e. endpoints that belong to the same system). This gives an overview of the entire system in one place and see how messages correlate. One can look at the audit queue as a central record of everything that is happening in the system. A central audit queue is also required by the Particular Service Platform and especially [ServiceControl](/servicecontrol), which consumes messages from these auditing queues. For more information, see [ServicePulse documentation](/servicepulse/).


## Message headers

The audited message is enriched with additional headers, which contain information related to processing the message:

 * Processing start and end times.
 * Processing host id and name.
 * Processing machine address.
 * Processing endpoint.


## Handling Audit messages

Those audit messages can then be handled as needed: Save them in a database, do custom logging, etc. It is important not to leave the messages in the audit queue however, as most queuing technologies have upper-bound limits on their queue sizes and depth. By not processing these messages, the limits of the underlying queue technology may be reached.


## Audit configuration options

There two settings that control Auditing:


### Queue Name

The queue name to forward audit messages to


### OverrideTimeToBeReceived

To force a [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md) on audit messages by setting `OverrideTimeToBeReceived` use the configuration syntax below.

Note that while the phrasing is "forwarding a message" in the implementation it is actually "cloning and sending a new message". This is important when considering TimeToBeReceived since the time taken to receive and process the original message is not part of the TimeToBeReceived of the new audit message. So in effect the audit message receives the full time allotment of whatever TimeToBeReceived is used.

{{Warning: Since MSMQ forces the same TimeToBeReceived on all messages in the a transaction, OverrideTimeToBeReceived is not supported when using the [MSMQ Transport](/nservicebus/msmq/). If OverrideTimeToBeReceived is detected when using MSMQ an exception, with the following text, will be thrown:

```no-highlight
Setting a custom OverrideTimeToBeReceived for audits is not supported on transactional MSMQ
```
}}


#### Default Value

If no OverrideTimeToBeReceived is defined then:

**Versions 5 and below**: TimeToBeReceived of the original message will be used.

**Versions 6 and above**: No TimeToBeReceived will be set.


## Configuring auditing


### Using Code

Configure the target audit queue using the configuration API.

snippet:AuditWithCode


### Using app.config

include: configurationWarning

snippet:configureAuditUsingXml

Note: `OverrideTimeToBeReceived` needs to be a valid [TimeSpan](https://msdn.microsoft.com/en-us/library/system.timespan.aspx).


### Using IProvideConfiguration

The audit settings can also be configured using code via a [custom configuration provider](/nservicebus/hosting/custom-configuration-providers.md).

snippet:AuditProvideConfiguration


## Machine level configuration

Versions 4 and above support setting the error queue and the audit queue at the machine level in the registry. Use the [Set-NServiceBusLocalMachineSettings](management-using-powershell.md) PowerShell commandlet to set these values at a machine level. When set at machine level, the setting is not required in the endpoint configuration file for messages to be forwarded to the audit queue.


## Turning off auditing

If the auditing settings are in `app.config`, remove them or comment them out.

If the machine level auditing is turned on, clear the value for the string value `AuditQueue` under

```no-highlight
HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceBus
```

If running 64 bit, in addition to the above, also clear the value for `AuditQueue` under

```no-highlight
HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ParticularSoftware\ServiceBus
```
