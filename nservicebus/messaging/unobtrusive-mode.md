---
title: Unobtrusive Mode Messages
summary: You do not need to reference any NServiceBus assemblies from your own message assemblies.
tags: []
redirects:
- nservicebus/unobtrusive-mode-messages
---

When using NServiceBus you define your message contracts using plain classes or interfaces. For NServiceBus to find those classes when scanning your assemblies you need to mark them with the special `IMessage` interface, which essentially says, "Hey, this is a message definition, please use it." This might seem like a small thing but now you're coupling your message contracts to a NServiceBus assembly since you need to reference the NServiceBus.dll to get access to the interface.

This dependency can cause problems if you have different services that run different versions of NServiceBus. Jonathan Oliver has a [great write up on this very subject](http://blog.jonathanoliver.com/nservicebus-distributing-event-schemacontract/).

This is not a big deal for commands because they are always used with in the boundary of a single service and it's fair to require a service to use the same version of NServiceBus. But when it comes to events, this becomes more of a problem since it requires your services to all use the same version of NServiceBus, thereby forcing them to upgrade NServiceBus all at once.


## The solution

There are a couple of ways you can solve this.

 * NServiceBus follows the [semver.org](http://semver.org/) semantics, only changing the assembly version when changes are not backwards compatible or introduce substantial new functionality or improvements. This mean that Version 3.0.1 and Version 3.0.X have the same assembly version (3.0.0), and file version of course changes for every release/build. This means that as long as you do a NuGet update with the -safe flag your service contracts will stay compatible.
 * Support for running in "Unobtrusive" mode means you do not need to reference any NServiceBus assemblies from your own message assemblies, thereby removing the problem altogether.


## Unobtrusive mode

NServiceBus allows you to define your own [message conventions](messages-events-commands.md) instead of using the `IMessage`, `ICommand` or `IEvent` interfaces and attributes like `TimeToBeReceivedAttribute` and `ExpressAttribute`. NServiceBus also supports conventions for encrypted properties, express messages, databus properties and time to be received. So with these conventions combined you can avoid referencing NServiceBus in your messages assembly.

snippet: MessageConventions

Note: It is important to note that in .NET namespace is optional and hence can be null. So if any conventions do partial string checks, for example using `EndsWith` or `StartsWith`, then a null check should be used. So include `.Namespace != null` at the start of the convention. Otherwise a null reference exception will occur during the type scanning.