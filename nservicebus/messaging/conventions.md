---
title: Conventions
summary: Custom conventions for defining how certain things are detected.
tags:
- Unobtrusive
- Conventions
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/unobtrusive-mode
---

A *convention* is a way of defining what a certain type is instead of using a marker interface or an attribute.

Currently conventions exist to identify:

 * [Commands](/nservicebus/messaging/messages-events-commands.md)
 * [Events](/nservicebus/messaging/messages-events-commands.md)
 * [Messages](/nservicebus/messaging/messages-events-commands.md)
 * [Encryption](/nservicebus/security/encryption.md)
 * [DataBus](/nservicebus/messaging/databus.md)
 * [Express messages](/nservicebus/messaging/non-durable-messaging.md)
 * [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md)

When Conventions are combined with avoiding an reference to any NServiceBus assemblies this is referred to as [Unobtrusive Mode](unobtrusive-mode.md). This makes it also ideal to use in cross platform environments. Messages can be defined in a *Portable Class Library* (PCL) and shared across multiple platform even though not all platforms use NServiceBus for message processing.


snippet: MessageConventions