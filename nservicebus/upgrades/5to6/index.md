---
title: Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus Version 5 to 6.
component: Core
reviewed: 2016-11-16
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


include: upgrade-major


## Move to .NET 4.5.2

The minimum .NET version for NServiceBus Version 6 is [.NET 4.5.2](https://support.microsoft.com/en-us/kb/2901954).

**All projects (that reference NServiceBus) must be updated to .NET 4.5.2 before updating to NServiceBus Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus Version 6. This will help isolate any issues that may occur.

For solutions with many projects the Visual Studio extension [Target Framework Migrator](https://marketplace.visualstudio.com/items?itemName=PavelSamokha.TargetFrameworkMigrator) can reduce the manual effort required in performing an upgrade.

See also:

 * [.NET Blog - Moving to .NET 4.5.2](https://blogs.msdn.microsoft.com/dotnet/2014/08/07/moving-to-the-net-framework-4-5-2/)
 * [Known issues for .NET 4.5.2](https://support.microsoft.com/en-us/kb/2962547)

NOTE: While a minimum of .NET 4.5.2 is required, it is recommended to update to at least [.NET 4.6.1](https://www.microsoft.com/en-au/download/details.aspx?id=49981) since this gives access to [Task.CompletedTask](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.completedtask.aspx).


include: dependencies



## Update Endpoint configuration

In previous versions of NServiceBus, to connect a process to the transport, an instance of `IBus` was needed. In Version 6 and above, this concept has been deprecated and now an instance of `IEndpointInstance` is required. The code required to create and configure an `IEndpointInstance` is very similar to the code found in Version 5 endpoints for creating and configuring `IBus` instances.

NOTE: This section describes updating a self-hosted endpoint. For endpoints that rely on the NServiceBus Host, see: [NServiceBus Host Upgrade Version 6 to 7](../host-6to7.md).

First, change all mentions of `BusConfiguration` to `EndpointConfiguration`. Note that `EndpointConfiguration` has a required constructor parameter to set the endpoint name. In Version 5, the name of the endpoint was provided via the `.EndpointName(name)` method on the `BusConfiguration` class. This call is no longer required in Version 6 and the method has been deprecated.

Most of the other method calls on `EndpointConfiguration` should continue to work the same way as they did on `BusConfiguration`. The methods that have changed between versions will each have deprecation messages that describe how to achieve the same effect in Version 6.

Once the instance of `EndpointConfiguration` has been created, it can be used to create an `IEndpointInstance`. In Version 5 and below, this step is accomplished using the `Bus` static class. In Version 6, this has been replaced with an `Endpoint` static class that works in a similar manner.

In Version 6 and above, any operation that interacts with the transport is asynchronous and returns a `Task`. This includes the `Start` method on the static `Endpoint` class and the `Stop` method on `IEndpointInstance`. Ideally these methods are called from within an `async` method and the results can simply be `awaited` (with `ConfigureAwait(false)` applied to them).

snippet: v6-endpoint-start-stop-full-async

If this is not the case then these calls must be converted back into synchronous ones using `GetAwaiter().GetResult()`. It is recommended that this conversion occurs early in the application lifecycle.

snippet: v6-endpoint-start-stop-sync-wrapper

Note that in Version 5 and below, `IBus` implements `IDisposable` and stops communicating with the transport when `Dispose` is called. It has been common to call `Bus.Create` from within a `using` block in console applications. In Version 6 as above, stopping an instance of an endpoint is asynchronous and needs to return a `Task` which is not possible with the signature of `IDisposable`. `IEndpointInstance` does not implement `IDisposable` and explicitly calling `Stop` and `await`ing the returned `Task` is the only way to shut down the endpoint.

See also:

 * [Migrating from IBus](moving-away-from-ibus.md) provides more in-depth discussion about the decision to deprecate `IBus` and how to handle other scenarios that depend on `IBus`.
 * [Endpoint API changes in Version 6](endpoint.md)
 * [NServiceBus Host Upgrade Version 6 to 7](../host-6to7.md)
 * [Transaction configuration changes in Version 6](transaction-configuration.md)
 * [Recoverability changes in Version 6](recoverability.md)
 * [Assembly scanning changes in Version 6](assembly-scanning.md)


## Update Handlers

In Version 6 the signature of `IHandleMessages<T>` has been changed to support asynchronous processing. In Version 5 and below, each message was handled by a dedicated thread. This meant that NServiceBus was able to take advantage of [thread static](https://msdn.microsoft.com/en-us/library/system.threadstaticattribute.aspx) state to keep track of the message being handled as well as the current transaction. In Version 6, each message is handled by a task which may run on several threads before processing is completed. Rather than rely on thread context and state, each handler is explicitly passed in a context object with all of the information it needs to execute.

NOTE: This context includes methods to send and publish new messages. As NServiceBus can no longer rely on thread static state to access information about the message and transaction being handled, it is not possible to rely on an injected copy of `IBus` to perform these operations. It is necessary to always send and publish new messages using the context object rather than an injected instance of the endpoint.

As each handler is running in the context of a transport receive operation (an I/O context) and is likely to contain other asynchronous operations (like sends and publishes), all handlers must return a [Task](https://msdn.microsoft.com/en-us/library/system.threading.tasks.task.aspx).

To update a handler to Version 6 follow this process:

 1. As the signature of `IHandleMessages<T>` has changed, Visual Studio will complain that the handler is not implementing it. To correctly implement the handler interface, change the return type of the `Handle` method from `void` to `async Task`. Next add a second parameter to the `Handle` method `IMessageHandlerContext context`.
 1. If the handler has an instance of `IBus` injected into it, it needs to be removed. Prior to removing it, rename it to `context` as all operations that previously relied on `IBus` will now go through the passed in instance of `IMessageHandlerContext`.
 1. Finally, the methods on `IMessageHandlerContext` all return tasks. It is important to `await` each of these tasks and to add `.ConfigureAwait(false)` on to each one.

<div class="video-container">
<iframe src="https://www.youtube.com/embed/QolL1Oum72Q" frameborder="0" allowfullscreen></iframe>
</div>

See also:

 * [Migrate handlers and sagas to Version 6](handlers-and-sagas.md)
 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging changes in Version 6](messaging.md)


## Update Sagas

Updating a saga is very similar to updating a handler with just a few extra steps.

The `Saga<T>` base class has been moved from to the `NServiceBus` namespace. Remove all `using` statements that refer to `NServiceBus.Saga`.

Update all of the `IHandleMessages<T>` implementations using the process outlined in the previous section. Note that `IAmStartedByMessages<T>` implementations can be updated in the same manner.

Check the implementation of `ConfigureHowToFindSaga()`. NServiceBus will be able to automatically correlate any message that is a reply to a message originally sent by this saga. For any other message type, including messages that can start the saga, an explicit mapping is required.

Remove the `[Unique]` attribute from the saga data class. NServiceBus will now automatically make correlated saga properties unique.

Note that calls to `RequestTimeout()` now require an instance `IMessageHandlerContext` to be passed in. Pass in the context parameter that was passed in to the `Handle()` method. Additionally, this method returns a `Task` which should have `ConfigureAwait(false)` applied and then wait for the response with `await`.

See also:

 * [Migrate handlers and sagas to Version 6](handlers-and-sagas.md)
 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging changes in Version 6](messaging.md)


## Sending and Publishing outside of a handler

Once all of the handlers and sagas in an endpoint have been updated to Version 6 there may still be places in the code that send and publish messages using an instance of `IBus` that need to be updated.

WARNING: All message handlers and sagas that are included in the endpoint should be updated before taking this step. It is important that the techniques presented here are not used from inside message handlers or transactional consistency with the transport is not guaranteed.

The endpoint instance returned from `Endpoint.Create()` or `Endpoint.Start()` implements `IMessageSession` which contains `Send()` and `Publish()` methods that can be used outside of a message handler or saga. If the endpoint sends messages in the same part of the code that creates/starts the endpoint then call these methods on the returned endpoint instance directly.

NOTE: As the `Send` and `Publish` methods on the endpoint instance should not be used from within a handler or saga, there is no implementation of the interface injected into the configured IoC container. For recommendations on how to get access to `IMessageSession` in other locations, see [Dependency Injection](moving-away-from-ibus.md#dependency-injection).

See also:

 * [Migrating from IBus](moving-away-from-ibus.md)
 * [Messaging changes in Version 6](messaging.md)


## Final steps

This covers the basic steps required to update an endpoint to Version 6. Each of the other NServiceBus dependencies may also require additional steps. See the dependency specific upgrade guides for more information.


#### Hosting

 * [NServiceBus Host](../host-6to7.md)
 * [Azure Cloud Services Host](../acs-host-6to7.md)


#### Transports

 * MSMQ - There are no special upgrade requirements for endpoints using the MSMQ transport. If the solution being upgraded includes the distributor component then see [Upgrading an endpoint using Distributor from Version 5 to 6](/samples/scaleout/distributor-upgrade/).
 * [Azure Service Bus](/transports/upgrades/asb-6to7.md)
 * [Azure Storage Queues](/transports/upgrades/asq-6to7.md)
 * [RabbitMQ](/transports/upgrades/rabbitmq-3to4.md)
 * [SQL Server](/transports/upgrades/sqlserver-2to3.md)


#### Persistence

 * [Azure Storage](/persistence/upgrades/asp-6to1.md)
 * [NHibernate](/persistence/upgrades/nhibernate-6to7.md)
 * [RavenDB](/persistence/upgrades/ravendb-3to4.md)


#### Others

 * [NServiceBus Testing](../testing-5to6.md)
 * [Gateway](../gateway-1to2.md)
 * [Azure Blob Storage DataBus](../absdatabus-6to1.md)
