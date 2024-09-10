---
title: Conventions
summary: Custom conventions for defining how certain things are detected and to support unobtrusive mode
component: Core
reviewed: 2024-08-22
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/unobtrusive-mode
---

*Conventions* can be used to identify which types are messages, commands, and events, instead of using [marker interfaces](/nservicebus/messaging/messages-events-commands.md#identifying-messages-marker-interfaces). This can be done to avoid references to the NServiceBus assembly, referred to as *[unobtrusive mode](unobtrusive-mode.md)*. This is ideal for use in cross-platform environments.

Currently *conventions* exist to identify:

 * [Commands](/nservicebus/messaging/messages-events-commands.md)
 * [Events](/nservicebus/messaging/messages-events-commands.md)
 * [Messages](/nservicebus/messaging/messages-events-commands.md)
 * [Message Property Encryption](/nservicebus/security/property-encryption.md)
 * [Data Bus](/nservicebus/messaging/claimcheck/)
 * [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md)

Message types can be defined in a *Portable Class Library* (PCL) and shared across multiple platforms, even if the platform does not use NServiceBus for message processing.

snippet: MessageConventions

> [!NOTE]
> In .NET, the namespace is optional and can be null. If any conventions do partial string checks, for example using `EndsWith` or `StartsWith`, then a null check should be used. Include `.Namespace != null` at the start of the convention to avoid a null reference exception during type scanning.

## Using both default and custom conventions

Defining a custom convention will overwrite the default convention. If both default and custom conventions are needed, the default conventions must be specified along with the custom conventions. 

snippet: MessageConventionsDual

partial: encapsulated-conventions

## Attributes

If attributes are preferred over marker interfaces then this can be achieved using [NServiceBus.AttributeConventions](https://github.com/mauroservienti/NServiceBus.AttributeConventions), a [community package](/nservicebus/community/) that allows using attributes instead of interfaces.
