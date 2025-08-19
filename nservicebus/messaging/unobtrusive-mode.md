---
title: Unobtrusive Mode Messages
summary: How to avoid referencing NServiceBus assemblies from message assemblies.
reviewed: 2024-09-11
related:
 - nservicebus/messaging/messages-events-commands
 - nservicebus/messaging/conventions
 - samples/message-assembly-sharing
redirects:
- nservicebus/unobtrusive-mode-messages
- nservicebus/how-do-i-centralize-all-unobtrusive-declarations
- nservicebus/invalidoperationexception-in-unobtrusive-mode
---

Message contracts can be defined using plain classes or interfaces. For NServiceBus to find those classes when scanning assemblies, they need to be marked with the `IMessage` interface, which essentially says, "this is a message definition". This allows decoupling message contracts from the NServiceBus assembly.

This dependency can cause problems when different services run different versions of NServiceBus. Jonathan Oliver has a [great write up on this very subject](https://blog.jonathanoliver.com/nservicebus-distributing-event-schemacontract/).

> [!NOTE]
> It's also possible to use the [NServiceBus.MessageInterfaces](https://www.nuget.org/packages/NServiceBus.MessageInterfaces) to build message assemblies targeting `netstandard2.0` that can be shared between different major versions of NServiceBus and different versions of .NET. See the details in the [sharing message assemblies sample](/samples/message-assembly-sharing/).

This is not a big deal for commands because they are always used within the boundary of a single service, and it's fair to require a service to use the same version of NServiceBus. However, when it comes to events, this becomes more of a problem since it requires the services to all use the same version of NServiceBus, thereby forcing them to upgrade NServiceBus all at once.


## The solution

There are a couple of ways this can be solved.

 * NServiceBus follows the [semver.org](https://semver.org/) semantics, changing the assembly version only when changes are not backward compatible or introduce substantial new functionality or improvements. This means that Version 3.0.1 and Version 3.0.X have the same assembly version (3.0.0), and file version changes for every release/build. As long as NuGet updates are done with the -safe flag, the service contracts will stay compatible.
 * Support for running in "Unobtrusive" mode means no reference to any NServiceBus assemblies is required from message assemblies, thereby removing the problem altogether.


## Unobtrusive mode

NServiceBus allows defining custom [message conventions](conventions.md) instead of using the `IMessage`, `ICommand` or `IEvent` interfaces and attributes like `TimeToBeReceivedAttribute`. NServiceBus also supports conventions for encrypted properties, databus properties and time to be received. The usage of these conventions can avoid a reference to NServiceBus.
