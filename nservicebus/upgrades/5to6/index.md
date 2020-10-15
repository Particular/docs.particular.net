---
title: Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus from version 5 to 6.
component: Core
reviewed: 2020-07-11
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


include: upgrade-major


## Move to .NET 4.5.2

The minimum .NET version for NServiceBus version 6 is [.NET 4.5.2](https://support.microsoft.com/en-us/kb/2901954).

**All projects (that reference NServiceBus) must be updated to .NET 4.5.2 before updating to NServiceBus version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus version 6. This will help isolate any issues that may occur.

For solutions with many projects, the [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) Visual Studio extension can reduce the manual effort required in performing an upgrade.

See also:

 * [.NET Blog - Moving to .NET 4.5.2](https://blogs.msdn.microsoft.com/dotnet/2014/08/07/moving-to-the-net-framework-4-5-2/)
 * [Known issues for .NET 4.5.2](https://support.microsoft.com/en-us/kb/2962547)

NOTE: While a minimum of .NET 4.5.2 is required, it is recommended to update to at least [.NET 4.6.1](https://www.microsoft.com/en-au/download/details.aspx?id=49981) since this gives access to [Task.CompletedTask](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.completedtask.aspx).


include: dependencies



## Update endpoint configuration

In previous versions of NServiceBus, connecting a process to the transport required an instance of `IBus`. Starting from version 6, this concept has been deprecated and instead, an instance of `IEndpointInstance` is required. The code required to create and configure an `IEndpointInstance` is very similar to the code found in version 5 endpoints for creating and configuring `IBus` instances.

NOTE: This section describes updating a self-hosted endpoint. For endpoints that rely on the NServiceBus Host, see: [NServiceBus Host Upgrade Version 6 to 7](../host-6to7.md).

First, change all mentions of `BusConfiguration` to `EndpointConfiguration`. Note that `EndpointConfiguration` has a required constructor parameter to set the endpoint name. In NServiceBus version 5, the name of the endpoint was provided via the `.EndpointName(name)` method on the `BusConfiguration` class. This call is no longer required in version 6 and the method is deprecated.

Most of the other method calls on `EndpointConfiguration` work the same way as they did on `BusConfiguration`. The methods that have changed between versions will each have deprecation messages that describe how to achieve the same effect in version 6.

Once the instance of `EndpointConfiguration` has been created, it can be used to create an `IEndpointInstance`. In versions 5 and below, this step is accomplished using the `Bus` static class. In version 6, this has been replaced with an `Endpoint` static class that works in a similar manner.

In NServiceBus version 6 and above, any operation that interacts with the transport is asynchronous and returns a `Task`. This includes the `Start` method on the static `Endpoint` class and the `Stop` method on `IEndpointInstance`. Ideally these methods are called from within an `async` method and the results can simply be `awaited` (with `ConfigureAwait(false)` applied to them).

snippet: v6-endpoint-start-stop-full-async

If this is not the case then these calls must be converted back into synchronous ones using `GetAwaiter().GetResult()`. It is recommended that this conversion occur early in the application lifecycle.

snippet: v6-endpoint-start-stop-sync-wrapper

Note that in NServiceBus version 5, `IBus` implements `IDisposable` and stops communicating with the transport when `Dispose` is called. It is common to call `Bus.Create` from within a `using` block in console applications. In version 6, stopping an instance of an endpoint is asynchronous and must return a `Task` which is not possible with the signature of `IDisposable`. `IEndpointInstance` does not implement `IDisposable` and explicitly calling `Stop` and `await`ing the returned `Task` is the only way to shut down the endpoint.

See also:

 * [Migrating from IBus](moving-away-from-ibus.md) provides more in-depth discussion about the decision to deprecate `IBus` and how to handle other scenarios that depend on `IBus`.
 * [Endpoint API Changes in NServiceBus Version 6](endpoint.md)
 * [NServiceBus Host Upgrade Version 6 to 7](../host-6to7.md)
 * [Transaction Configuration Changes in NServiceBus Version 6](transaction-configuration.md)
 * [Recoverability Changes in NServiceBus Version 6](recoverability.md)
 * [Assembly Scanning Changes in NServiceBus Version 6](assembly-scanning.md)


## Update handlers

In NServiceBus version 6, the signature of `IHandleMessages<T>` has been changed to support asynchronous processing. In versions 5 and below, each message is handled by a dedicated thread, so NServiceBus is able to take advantage of [thread static](https://msdn.microsoft.com/en-us/library/system.threadstaticattribute.aspx) state to keep track of the message being handled as well as the current transaction. In version 6, each message is handled by a task which may run on several threads before processing is completed. Rather than rely on thread context and state, each handler is explicitly passed in a context object with all of the information it needs to execute.

NOTE: This context includes methods to send and publish new messages. As NServiceBus can no longer rely on thread static state to access information about the message and transaction being handled, it is not possible to rely on an injected copy of `IBus` to perform these operations. It is necessary to always send and publish new messages using the context object rather than an injected instance of the endpoint.

As each handler is running in the context of a transport receive operation (an I/O context) and is likely to contain other asynchronous operations (such as sends and publishes), all handlers must return a [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx).

To update a handler to NServiceBus version 6 follow this process:

 1. As the signature of `IHandleMessages<T>` has changed, Visual Studio will show an error that the handler is not implementing the interface. To correctly implement the handler interface, change the return type of the `Handle` method from `void` to `async Task`. Next add a second parameter to the `Handle` method `IMessageHandlerContext context`.
 1. If the handler has an instance of `IBus` injected into it, it must be removed. Prior to removing it, rename it to `context` as all operations that previously relied on `IBus` will now go through the passed in instance of `IMessageHandlerContext`.
 1. Finally, the methods on `IMessageHandlerContext` all return tasks. It is important to `await` each of these tasks and to add `.ConfigureAwait(false)` to each method call.

<div class="video-container">
<iframe src="https://www.youtube.com/embed/QolL1Oum72Q" frameborder="0" allowfullscreen></iframe>
</div>

See also:

 * [Migrate Handlers and Sagas to NServiceBus Version 6](handlers-and-sagas.md)
 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging Changes in NServiceBus Version 6](messaging.md)


## Update sagas

Updating a saga is similar to updating a handler with a few extra steps.

The `Saga<T>` base class has been moved from the `NServiceBus.Saga` namespace to the `NServiceBus` namespace. Remove all `using` statements that refer to `NServiceBus.Saga` (or change them to refer to `NServiceBus` if this using statement doesn't already exist in the class).

Update all of the `IHandleMessages<T>` implementations using the process outlined in the previous section. Note that `IAmStartedByMessages<T>` implementations can be updated in the same manner.

Check the implementation of `ConfigureHowToFindSaga()`. NServiceBus will be able to automatically correlate any message that is a reply to a message originally sent by this saga. For any other message type, including messages that can start the saga, an explicit mapping is required.

Remove the `[Unique]` attribute from the saga data class. NServiceBus automatically makes correlated saga properties unique in version 6.

Note that calls to `RequestTimeout()` now require an instance of `IMessageHandlerContext` to be passed in. Pass in the context parameter that was passed in to the `Handle()` method. Additionally, this method returns a `Task` which should have `ConfigureAwait(false)` applied and then wait for the response with `await`.

See also:

 * [Migrate Handlers and Sagas to NServiceBus Version 6](handlers-and-sagas.md)
 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging Changes in NServiceBus Version 6](messaging.md)


## Sending and publishing outside of a handler

Once all handlers and sagas in an endpoint have been updated to NServiceBus version 6, there may still be places in the code that send and publish messages using an instance of `IBus` that need to be updated.

WARNING: All message handlers and sagas that are included in the endpoint should be updated before taking this step. It is important that the techniques presented here are not used from inside message handlers or transactional consistency with the transport is not guaranteed.

The endpoint instance returned from `Endpoint.Create()` or `Endpoint.Start()` implements `IMessageSession` which contains `Send()` and `Publish()` methods that can be used outside of a message handler or saga. If the endpoint sends messages in the same part of the code that creates/starts the endpoint, call these methods on the returned endpoint instance directly.

NOTE: As the `Send` and `Publish` methods on the endpoint instance should not be used from within a handler or saga, there is no implementation of the interface injected into dependency injection. For recommendations on how to get access to `IMessageSession` in other locations, see [Dependency Injection](moving-away-from-ibus.md#dependency-injection).

See also:

 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging Changes in NServiceBus Version 6](messaging.md)
