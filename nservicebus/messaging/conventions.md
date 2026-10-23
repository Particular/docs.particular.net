---
title: Conventions
summary: Custom conventions for defining how certain types are detected and to support unobtrusive mode
component: Core
reviewed: 2026-06-01
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/unobtrusive-mode
---

*Conventions* identify message, command, and event types without [marker interfaces](/nservicebus/messaging/messages-events-commands.md#identifying-messages-marker-interfaces). Using conventions for message identification is called *[unobtrusive mode](unobtrusive-mode.md)*.

> [!NOTE]
> For new message contracts with NServiceBus 8 or later, prefer the marker interfaces in the [NServiceBus.MessageInterfaces package](https://www.nuget.org/packages/NServiceBus.MessageInterfaces). The package avoids a dependency on the NServiceBus assembly while supporting shared message assemblies across major NServiceBus versions and .NET target frameworks. Use conventions when existing message types cannot implement marker interfaces or when conventions are needed for other features. See the [sharing message assemblies sample](/samples/message-assembly-sharing/).

Currently, *conventions* exist to identify:

- [Commands](/nservicebus/messaging/messages-events-commands.md)
- [Events](/nservicebus/messaging/messages-events-commands.md)
- [Messages](/nservicebus/messaging/messages-events-commands.md)
- [Message Property Encryption](/nservicebus/security/property-encryption.md)
- [Data Bus](/nservicebus/messaging/claimcheck/)
- [TimeToBeReceived](/nservicebus/messaging/discard-old-messages.md)

Message types can be defined in a *Portable Class Library* (PCL) and shared across multiple platforms, even if the platform does not use NServiceBus for message processing.

snippet: MessageConventions

> [!NOTE]
> In .NET, the namespace is optional and can be null. If any conventions do partial string checks, for example using `EndsWith` or `StartsWith`, then a null check should be used. Include `.Namespace != null` at the start of the convention to avoid a null reference exception during type scanning.

## Using both default and custom conventions

Defining a custom convention will overwrite the default convention. If both default and custom conventions are needed, the default conventions must be specified along with the custom conventions.

snippet: MessageConventionsDual

partial: encapsulated-conventions

## Attributes

If attributes are preferred over marker interfaces, use [NServiceBus.AttributeConventions](https://github.com/mauroservienti/NServiceBus.AttributeConventions), a [community package](/nservicebus/community/) that allows using attributes instead of interfaces.
