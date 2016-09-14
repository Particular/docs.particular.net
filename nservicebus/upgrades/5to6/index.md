---
title: Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus Version 5 to 6.
tags:
 - upgrade
 - migration
---


Every solution is different and will encounter unique challenges when upgrading a major dependency like NServiceBus. It's important to plan out an upgrade project and proceed in well defined steps, taking sufficient time to perform adequate regression testing after each step. Here are a few things to consider when planning an upgrade project.


## Endpoint selection

It is not necessary for every endpoint in the solution to be running the same version of NServiceBus. Endpoints running Version 6 are able to exchange messages with endpoints running Version 5 transparently.

Not every endpoint in the solution needs to be upgraded to Version 6 at all. Each endpoint only needs to be upgraded if it will take advantage of a new feature introduced in Version 6. New endpoints added to the system can be started, developed and deployed entirely in Version 6 and will be able to exchange messages with the other endpoints in the solution that are on Version 5.

**Do not upgrade an endpoint unless there is a compelling reason to do so.**

Note that some new features added in Version 6 require that all endpoints are running on Version 6 prior to enabling this feature. For example the [multiple deserializers API](/samples/serializers/multiple-deserializers/). Ensure that any new features are adequately researched in regards to its impact on the upgrade process.

Another factor to consider is the investment required to maintain codebases using different versions of NServiceBus. It may be cheaper in the long run to maintain a single codebase containing just Version 6 code than to invest in training and knowledge around Versions 5 and below.

Once the list of endpoints, that need to be upgraded, has been identified, upgrade them one at a time. As a Version 6 endpoint is able to exchange messages with Version 5 endpoints, upgrade one endpoint, test it, and deploy it to production before upgrading the next endpoint. This keeps the scope of changes to a minimum, which helps to reduce risk and to isolate potential problems if they arise.

**Upgrade one endpoint at a time.**

There is one common issue with upgrading a single endpoint at a time. If the endpoints in a solution share a common library then upgrading one endpoint might lead to changes in the common library and that necessitates changes in all of the other endpoints that rely on the common library at the same time. The recommended approach to dealing with this is to create a copy of the common library for the new endpoint and to upgrade it along with the endpoint. When the time comes to upgrade the second endpoint, change it's dependency to point to the new, upgraded, version of the common library. When using this approach, other changes to the common library should be minimized as they will need to be reflected in both codebases.

The process of upgrading each endpoint is going to follow a common sequence of steps. Being able to repeatably apply those steps is key to the success of the upgrade project. The recommended approach is to upgrade a simple and low risk endpoint first to ensure that the process is well understood before tackling the endpoints that make up the core of the solution. For example endpoints that send emails or generate documents are often good candidates for this. When selecting the first endpoint to upgrade look for a small number of reasonably straightforward handlers and a small amount of NServiceBus configuration. It is worth considering selecting a simple endpoint to upgrade even if it will not take advantage of Version 6 features to practice the upgrade process.


## Move to .NET 4.5.2

The minimum .NET version for NServiceBus Version 6 is [.NET 4.5.2](https://support.microsoft.com/en-us/kb/2901954).

**All projects (that reference NServiceBus) must be updated to .NET 4.5.2 before updating to NServiceBus Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus Version 6. This will help isolate any issues that may occur.

For solutions with many projects the Visual Studio extension [Target Framework Migrator](https://visualstudiogallery.msdn.microsoft.com/47bded90-80d8-42af-bc35-4736fdd8cd13) can reduce the manual effort required in performing an upgrade.

See also:

 * [.NET Blog - Moving to the .NET Framework 4.5.2](https://blogs.msdn.microsoft.com/dotnet/2014/08/07/moving-to-the-net-framework-4-5-2/)
 * [Known issues for the .NET Framework 4.5.2](https://support.microsoft.com/en-us/kb/2962547)


## Update NServiceBus dependencies

All NServiceBus dependencies for an endpoint project are managed via NuGet. Open the Manage NuGet Packages window for the endpoint project, switch to the Updates tab and look for packages that start with NServiceBus. Update each one to the latest Version 6 package.

NOTE: All of the NuGet packages are currently available as pre-release builds so the *Include pre-release* option must be selected by either checking the box in the Manage NuGet Packages window or by including the `-Pre` flag in the Package Manager Console. 

Once packages have been updated the project will contain quite a few errors. This is expected as a lot of things have changed.

See also:

 * [NuGet Package Manager Dialog - Updating a Package](https://docs.nuget.org/consume/package-manager-dialog#updating-a-package)
 * [NuGet Package Manager Console - Updating a Package](https://docs.nuget.org/consume/package-manager-console#updating-a-package)


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

 * [Migrating from IBus](moving-away-from-ibus.md) (provides more in-depth discussion about the decision to deprecate `IBus` and how to handle other scenarios that depend on `IBus`).
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
 1. If the handler has an instance of `IBus` injected into it, it needs to be removed. Before getting rid of it, rename it to `context` as all operations that previously relied on `IBus` will now go through the passed in instance of `IMessageHandlerContext`.
 1. Finally, the methods on `IMessageHandlerContext` all return tasks. It is important to `await` each of these tasks and to add `.ConfigureAwait(false)` on to each one.

<iframe width="560" height="315" src="https://www.youtube.com/embed/QolL1Oum72Q" frameborder="0" allowfullscreen></iframe>

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
 * [Azure Service Bus](../asb-6to7.md)
 * [Azure Storage Queues](../asq-6to7.md)
 * [RabbitMQ](../rabbitmq-3to4.md)
 * [SQL Server](../sqlserver-2to3.md)


#### Persistence

 * [Azure Storage](../asp-6to1.md)
 * [NHibernate](../nhibernate-6to7.md)
 * [RavenDB](../ravendb-3to4.md)


#### Others

 * [NServiceBus Testing](../testing-5to6.md)
 * [Gateway](../gateway-1to2.md)
 * [Azure Blob Storage DataBus](../absdatabus-6to1.md)
