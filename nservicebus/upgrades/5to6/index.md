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


## [Endpoint](/nservicebus/endpoints/) Name is mandatory

In Versions 6 and above endpoint name is mandatory.

snippet: 5to6-endpointNameRequired

The endpoint name is used as a logical identifier when sending or receiving messages. It is also used for determining the name of the input queue the endpoint will be bound to. See [Derived endpoint name](endpoint-name-helper.md) for the algorithm used in Versions 5 and below to select endpoint name if backwards compatibility is a concern.


## Message handlers

The handler method on `IHandleMessages<T>` now returns a Task. In order to leverage async code, add the `async` keyword to the handler method and use `await` for async methods. In order to convert the synchronous code add `return Task.FromResult(0);` or `return Task.CompletedTask` (.NET 4.6 and higher) to the handler methods.

WARNING: Do not `return null` from the message handlers. A `null` will result in an Exception.

snippet:5to6-messagehandler

For a step by step upgrade of existing handlers or sagas, see: [Migrate existing handlers/sagas](migrate-existing-handlers.md).


### Bus Send and Receive

There is also a change in the parameters, giving access to the `IMessageHandlerContext`, which provides the methods that used to be called from `IBus`. Use the `IMessageHandlerContext` to send and publish messages.

snippet:5to6-bus-send-publish


### Message handler ordering

In Version 6 the message handler ordering APIs are simplified. The full API can be seen in [Handler ordering](/nservicebus/handlers/handler-ordering.md).


#### Specifying a Handler to run first

snippet:5to6HandlerOrderingWithFirst


#### Specifying Handler order

snippet:5to6HandlerOrderingWithCode



### New context arguments

The signature for the mutators now passes context arguments that give access to relevant information on the message and also the mutation the message. This context will give access to the same functionality as previous versions so just update the code accordingly.

See [header manipulation](/nservicebus/messaging/header-manipulation.md) for one example on how this might look.

include:5to6removePShelpers


## Timeouts


### Timeout storage

`IPersistTimeouts` has been split into two interfaces, `IPersistTimeouts` and `IQueryTimeouts`, to properly separate those storage concerns. Both must be implemented to have a fully functional timeout infrastructure.

`IQueryTimeouts` implements the concern of polling for timeouts outside the context of a message pipeline. `IPersistTimeouts` implements the concern of storage and removal for timeouts which is executed inside the context of a pipeline. Depending on the design of the timeout persisters, those concerns can now be implemented independently. Furthermore, `IPersistTimeouts` introduced a new parameter `TimeoutPersistenceOptions `. This parameter allows access to the pipeline context. This enables timeout persisters to manipulate everything that exists in the context during message pipeline execution.


### Automatic retries

Previously configuring the number of times a message will be retried by the First Level Retries (FLR) mechanism also determined how many times the `TimeoutManager` attempted to retry dispatching a deferred message in case an exception was thrown. From Version 6, the `TimeoutManager` will attempt the dispatch five times (this number is not configurable anymore). The configuration of the FLR mechanism for non-deferred message dispatch has not been changed.


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


## Notifications

The `BusNotifications` class has been renamed to `Notifications`.

`BusNotifications` previously exposed the available notification hooks as observables implementing `IObservable`. This required implementing the `IObserver` interface or including [Reactive-Extensions](https://msdn.microsoft.com/en-au/data/gg577609.aspx) to use this API. In Version 6 the notifications API has been changed for easier usage. It exposes regular events instead of observables. To continue using Reactive-Extensions the events API can be transformed into `IObservable`s like this:

snippet: ConvertEventToObservable

Notification subscriptions can now also be registered at configuration time on the `EndpointConfiguration.Notifications` property. See the [error notifications documentation](/nservicebus/recoverability/subscribing-to-error-notifications.md) for more details and samples.


### Delayed delivery error notifications

In Versions 6 and above the `TimeoutManager` does not provide any error notifications. When an error occurs during processing of a deferred message by the `TimeoutManager`, the message will be retried and possibly moved to the error queue. The user will not be notified about these events.

Note that in Versions 5 and below, when the user [subscribes to error notifications](/nservicebus/recoverability/subscribing-to-error-notifications.md) they receive notification in the situation described above.


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
