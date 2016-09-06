---
title: Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus Version 5 to 6.
tags:
 - upgrade
 - migration
related:
- nservicebus/upgrades/gateway-1to2
- nservicebus/upgrades/sqlserver-2to3
---


## Move to .NET 4.5.2

The minimum .NET version for NServiceBus Version 6 is .NET 4.5.2.

**Users must update all projects (that reference NServiceBus) to .NET 4.5.2 before updating to NServiceBus Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus Version 6.

For larger solutions the Visual Studio extension [Target Framework Migrator](https://visualstudiogallery.msdn.microsoft.com/47bded90-80d8-42af-bc35-4736fdd8cd13) can reduce the manual effort required in performing an upgrade.


## IBus, IStartableBus and the Bus Static class are now obsolete

In previous versions of NServiceBus, to send or publish messages within a message handler or other extension interfaces, the message session (`IBus` interface in Versions 5 and below) was accessed via container injection. In Versions 6 injecting the message session is no longer required. Message handlers and other extension interfaces now provide context parameters such as `IMessageHandlerContext` or `IEndpointInstance` which give access to the same functions that used to be available via the `IBus` interface.

INFO: For more details on the various scenarios when using IBus, see: [Migrating from IBus](moving-away-from-ibus.md).

