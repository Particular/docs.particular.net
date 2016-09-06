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

For more details on the various scenarios when using IBus, see: [Migrating from IBus](moving-away-from-ibus.md).


include:5to6removePShelpers


## Outbox


### Outbox storage

`IOutboxStorage` introduced a new parameter `OutboxStorageOptions`. This parameter gives access to the pipeline context. This enables outbox storage methods to manipulate everything that exists in the context during message pipeline execution.


## MSMQ ReturnToSourceQueue.exe

The MSMQ ReturnToSourceQueue.exe tool is now deprecated. The code for this tool has been moved to [ParticularLabs/MsmqReturnToSourceQueue](https://github.com/ParticularLabs/MsmqReturnToSourceQueue) repository. See the readme in that repository for full details.


## Assembly scanning

See [Assembly Scanning API](/nservicebus/hosting/assembly-scanning.md) for more information.


### Nested Directories

NServiceBus Version 6 is no longer scanning nested directories for assemblies. This behavior can re-enable using the [Assembly Scanning API](/nservicebus/hosting/assembly-scanning.md#nested-directories).


### Include moved to Exclude

In Version 6 the API has been changed to an "Exclude a list" approach. See [Assemblies to scan](/nservicebus/hosting/assembly-scanning.md#assemblies-to-scan) for more information.

snippet:5to6ScanningUpgrade


## Timeout Persistence interfaces redesigned

The `IPersistTimeouts` interface was redesigned, and can now be implemented to provide a customized timeout persistence option. If using a custom timeout persister, note that the interface has been split into `IQueryTimeouts` and `IPersistTimeouts` (while `IPersistTimeoutsV2` has been removed). For more details see [authoring a custom persistence](/nservicebus/persistence/authoring-custom.md#timeout-persister).


## Queue creation

In Version 5 the implementation of the interface `ICreateQueues` was called for each queue that needed to be created. In Version 6 `ICreateQueues` has been redesigned. The implementation of the interface gets called once but with all queues provided on the `QueueBindings` object. It is now up to the implementation of that interface if the queues are created asynchronously in a sequential order or even in parallel.


## Encryption Service

It is no longer possible to access the builder to create an encryption service. If container access is required use the container directly in the factory delegate in the `RegisterEncryptionService` method.


## Conventions

[Conventions](/nservicebus/messaging/conventions.md) are no longer be injected into the [Container](/nservicebus/containers/). Conventions need to be retrieved with `Settings.Get<Conventions>()` over `ReadOnlySettings`.


## Dependency injection

Explicitly setting property values via `.ConfigureProperty<T>()` and `.InitializeHandlerProperty<T>()` has been deprecated. Instead configure the properties explicitly using:

snippet: 5to6-ExplicitProperties


## IConfigureComponents no longer registered in the container

To access it at runtime create a new [`Feature`](/nservicebus/pipeline/features.md) and put the following code in the `.Setup` method

snippet: 5to6-IConfigureComponentsNotInjected