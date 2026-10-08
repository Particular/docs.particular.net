---
title: Unobtrusive Mode Messages
summary: How to avoid referencing NServiceBus assemblies from message assemblies.
reviewed: 2026-05-22
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/conventions
 - samples/message-assembly-sharing
redirects:
- nservicebus/unobtrusive-mode-messages
- nservicebus/how-do-i-centralize-all-unobtrusive-declarations
- nservicebus/invalidoperationexception-in-unobtrusive-mode
---

Unobtrusive mode uses custom [message conventions](conventions.md) to identify messages, commands, and events instead of marker interfaces. Conventions can also configure related features, including time to be received, data bus, and property encryption.

For new message contracts with NServiceBus 8 or later, prefer the marker interfaces in the [NServiceBus.MessageInterfaces package](https://www.nuget.org/packages/NServiceBus.MessageInterfaces). The package avoids a dependency on the NServiceBus assembly while retaining explicit message markers, and supports sharing message assemblies across major NServiceBus versions and .NET target frameworks. See the [sharing message assemblies sample](/samples/message-assembly-sharing/).

Use unobtrusive message conventions when existing message types cannot implement marker interfaces or when conventions are needed for other features. See [conventions](conventions.md) for configuration details.
