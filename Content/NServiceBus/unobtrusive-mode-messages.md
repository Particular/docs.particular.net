---
title: Unobtrusive Mode Messages
summary: You do not need to reference any NServiceBus assemblies from your own message assemblies.
tags: []
---

When using NServiceBus you define your message contracts using plain C\# classes or interfaces. For NServiceBus to find those classes when scanning your assemblies you need to mark them with the special `IMessage` interface, which essentially says, "Hey, this is a message definition, please use it." This might seem like a small thing but now you're coupling your message contracts to a NServiceBus assembly since you need to reference the NServiceBus.dll to get access to the interface.

This dependency can cause problems if you have different services that run different versions of NServiceBus. Jonathan Oliver has a [great write up on this very subject](http://blog.jonathanoliver.com/nservicebus-distributing-event-schemacontract/).

This is not a big deal for commands because they are always used with in the boundary of a single service and it's fair to require a service to use the same version of NServiceBus. But when it comes to events, this becomes more of a problem since it requires your services to all use the same version of NServiceBus, thereby forcing them to upgrade NServiceBus all at once.

The solution
------------

There are a couple of ways you can solve this. NServiceBus V3 has a few changes that help:

-   NServiceBus follows the [semver.org](http://semver.org/) semantics, only changing the assembly version when changes are not backwards compatible or introduce substantial new functionality or improvements. This mean that V3.0.1 and V3.0.X have the same assembly version (3.0.0), and file version of course changes for every release/build. This means that as long as you do a NuGet update with the -safe flag your service contracts will stay compatible.
-   Support for running in "Unobtrusive" mode means you do not need to reference any NServiceBus assemblies from your own message assemblies, thereby removing the problem altogether.

Unobtrusive mode
----------------

This new feature in NServiceBus V3 allows you to pass in your own conventions to determine which types are message definitions, instead of using the `IMessage`, `ICommand` or `IEvent` interfaces. The following snippet shows how to define those conventions:

```
Configure.With().UseTransport<Msmq>() //Configure.With().MsmqTransport() in V 3
    .DefaultBuilder()
    .FileShareDataBus(@"\\MyDataBusShare\")
    .DefiningCommandsAs(t => t.Namespace != null && t.Namespace.EndsWith("Commands"))
    .DefiningEventsAs(t => t.Namespace != null && t.Namespace.EndsWith("Events"))
    .DefiningMessagesAs(t => t.Namespace == "Messages")
    .DefiningEncryptedPropertiesAs(p => p.Name.StartsWith("Encrypted"))
    .DefiningDataBusPropertiesAs(p => p.Name.EndsWith("DataBus"))
    .DefiningExpressMessagesAs(t => t.Name.EndsWith("Express"))
    .DefiningTimeToBeReceivedAs(t => t.Name.EndsWith("Expires")
 ? TimeSpan.FromSeconds(30)
 : TimeSpan.MaxValue);
```

This code tells NServiceBus to treat all types with a namespace that ends with "Messages" as messages. You can also specify conventions for the [ICommand and IEvent feature](introducing-ievent-and-icommand.md) .

NServiceBus supports property level encryption with a special WireEncryptedString property. The example above shows the unobtrusive way to tell NServiceBus which properties you want encrypted. These properties need to be of type String.

The example above also shows the unobtrusive way to tell NServiceBus which properties to deliver on a separate channel from the message itself using the Data Bus feature, and which messages are express or/and have a time to be received.

**NOTE** : When you're self hosting, `.DefiningXXXAs()` has to be before
`.UnicastBus()`, otherwise you get 

    System.InvalidOperationException: "No destination specified for message(s): <message type name>"

