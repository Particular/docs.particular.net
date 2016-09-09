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


## Upgrading to NServiceBus 6

Every solution is different and will encounter unique challenges when upgrading a major dependency like NServiceBus. It's important to plan out an upgrade project and proceed in well defined steps, stopping to ensure that everything is working after each step. Here are a few things to consider when planning an upgrade project.

### Endpoint selection

It is not necessary for every endpoint in the solution to be running the same version of NServiceBus. Endpoints running Version 6 are able to exchange messages with endpoints running Version 5 transparently. This [wire compatability](link to wire compat doco) helps to reduce the complexity of an upgrade project by allowing each endpoint to be upgraded one at a time. 

Not every endpoint in the solution needs to be upgraded to Version 6 at all. Each endpoint only needs to be upgraded if it will take advantage of a new feature introduced in Version 6. New endpoints added to the system can be started, developed and deployed entirely in Version 6 and will be able to exchange messages with the other endpoints in the solution that are on Version 5.

**Do not upgrade an endpoint unless there is a compelling reason to do so.**

Note that some new features added in Version 6 require that all endpoints are running on Version 6 before they can be switched on (can we identify even a subset of these?). Another factor to consider is the investment required to maintain codebases using different versions of NServiceBus. It may be cheaper in the long run to maintain a single codebase containing just Version 6 code than to invest in training and knowledge around Versions 5 and below.

Once the list of endpoints that need to be upgraded to Version 6 has been identified, upgrade them one at a time. As a Version 6 endpoint is able to exchange messages with Version 5 endpoints, upgrade one endpoint, test it, and deploy it to production before upgrading the next endpoint. This keeps the scope of changes to a minimum which help to reduce risk and to isolate potential problems when they arise. 

**Upgrade one endpoint at a time.**

There is one common issue with upgrading a single endpoint at a time. If the endpoints in a solution share a common libary then upgrading one endpoint might lead to changes in the common library and that necessitates changes in all of the other endpoints that rely on the common library at the same time. The recommended approach to dealing with this is to create a copy of the common libary for the new endpoint and to upgrade it along with the endpoint. When the time comes to upgrade the second endpoint, change it's dependency to point to the new, upgraded, version of the common library. When using this approach, other changes to the common library should be minimized as they will need to be reflected in both codebases.

The process of upgrading each endpoint is going to follow a common sequence of steps. Being able to repeatably apply those steps is key to the success of the upgrade project. The recommended approach is to upgrade a simple and low risk endpoint first to ensure that the process is well understood before tackling the endpoints that make up the core of the solution. Endpoints that send email or generate documents are often good candidates for this. When selecting the first endpoint to upgrade look for a small number of reasonably straightforward handlers and a small amount of NServiceBus configuration. It is worth considering selecting a simple endpoint to upgrade even if it will not take advantage of Version 6 features to practice the upgrade process. 


### Move to .NET 4.5.2

The minimum .NET version for NServiceBus Version 6 is .NET 4.5.2.

**All projects (that reference NServiceBus) must be updated to .NET 4.5.2 before updating to NServiceBus Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus Version 6.

For larger solutions the Visual Studio extension [Target Framework Migrator](https://visualstudiogallery.msdn.microsoft.com/47bded90-80d8-42af-bc35-4736fdd8cd13) can reduce the manual effort required in performing an upgrade.


### Update NServiceBus dependencies

All NServiceBus dependencies for an endpoint project are managed via NuGet. Open the Manage NuGet Packages window for the endpoint project, switch to the Updates tab and look for packages that start with NServiceBus. Update each one to the latest Version 6 package.

(screenshot?)

NOTE: All of the NuGet packages are currently available as prerelease builds so the Include prerelease option must be selected by either checking the box in the Manage NuGet Packages window or by including the `-Pre` flag in the Package Manager Console. 

Once all of the NServiceBus packages have been updated to Version 6 the project will contain quite a few errors. This is expected as a lot of things have changed.


### Update Endpoint configuration

In previous versions of NServiceBus, to connect a process to the transport, an instance of `IBus` was needed. In Version 6 and above, this concept has been deprecated and now an instance of `IEndpointInstance` is required. The code required to create and configure an `IEndpointInstance` is very similar to the code found in Version 5 endpoints for creating and configuring `IBus` instances.

NOTE: This section describes updating a self-hosted endpoint. For endpoints that rely on the NServiceBus Host, see: [NServiceBus Host Upgrade Version 6 to 7](host-6to7.md).

First, change all mentions of `BusConfiguration` to `EndpointConfiguration`. Note that `EndpointConfiguration` has a required constructor parameter to set the endpoint name. In Version 5, the name of the endpoint was provided via the `.EndpointName(name)` method on the `BusConfiguration` class. This call is no longer required in Version 6 and the method has been deprecated.

Most of the other method calls on `EndpointConfiguration` shoud continue to work the same way as they did on `BusConfiguration`. The methods that have changed between versions will each have deprecation messages that describe how to achieve the same effect in Version 6.

Once the instance of `EndpointConfiguration` has been created, it can be used to create an `IEndpointInstance`. In Version 5 and below, this step is accomplished using the `Bus` static class. In Version 6, this has been replaced with an `Endpoint` static class that works in a similar manner. 

In Version 6 and above, any operation that interacts with the transport is asynchronous and returns a `Task`. This includes the `Start` method on the static `Endpoint` class and the `Stop` method on `IEndpointInstance`. Ideally these methods are called from within an `async` method and the results can simply be `awaited` (with `ConfigureAwait(false)` applied to them).

```CSharp
public async Task Run(EndpointConfiguration config)
{
  // pre startup
  var endpointInstance = await Endpoint.Start(config).ConfigureAwait(false);
  // post startup

  // block process

  // pre shutdown
  await endpointInstance.Stop().ConfigureAwait(false);
  // post shutdown
}
```

If this is not the case then you can convert these calls back into synchronous ones using `GetAwaiter().GetResult()`. It is recommended that this conversion occurs early in the application lifecycle.

```CSharp
public void Run(EndpointConfiguration config)
{
	RunAsync(config).GetAwaiter().GetResult();
}

public async Task RunAsync(EndpointConfiguration config)
{
  // pre startup
  var endpointInstance = await Endpoint.Start(config).ConfigureAwait(false);
  // post startup

  // block process

  // pre shutdown
  await endpointInstance.Stop().ConfigureAwait(false);
  // post shutdown
}
```

Note that in Version 5 and below, `IBus` implements `IDisposable` and stops communicating with the transport when `Dispose` is called. It has been common to call `Bus.Create` from within a `using` block in console applications. In Version 6 as above, stopping an instance of an endpoint is asynchronous and needs to return a `Task` which is not possible with the signature of `IDisposable`. `IEndpointInstance` does not implement `IDisposable` and explicitly calling `Stop` and `await`ing the returned `Task` is the only way to shut down the endpoint. 

See also:
- [Migrating from IBus](moving-away-from-ibus.md) (provides more in-depth discussion about the decision to deprecate `IBus` and how to handle other scenarios that depend on `IBus`).
- [Endpoint API changes in Version 6](endpoint.md)
- [NServiceBus Host Upgrade Version 6 to 7](host-6-7.md)


### Update Handlers

Short explanation
Link off to full doco about how to do this


### Update sending messages

Now that Handler Contexts have been introduced, talk about bus.Send and bus.Publish outside of handlers


### Update Sagas

Should be simple given the above topics.


### Finishing touches

At this point the number of issues remaining should be minimal. Link off to the full upgrade guide for all of the remaining topics. 