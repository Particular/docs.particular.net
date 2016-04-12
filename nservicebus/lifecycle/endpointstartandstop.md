---
title: When Endpoint Instance Starts and Stops
summary: An interface that allows hooking into into the startup and shutdown sequence of an endpoint instance.
reviewed: 2016-04-06
tags:
 - life-cycle
related:
 - samples/startup-shutdown-sequence
redirects:
 - nservicebus/lifecycle/iwanttorunwhenbusstartsandstops
---

Classes that plug into the startup/shutdown sequence are invoked just after the endpoint instance has been started and just before it is stopped. Use this approach for any tasks that need to execute with the same life-cycle as the endpoint instance.

NOTE: The endpoint instance keeps an internal list of instances which need to be stopped but the instances are registered with the `Builder` as Instance Per Call by default. This means that any attempt to resolve a custom instance via dependency injection will result in a new instance and not the one which has been started and which will be stopped.


## Versions 6 and above

The `IWantToRunWhenBusStartsAndStops` interface is no longer available.  

When self-hosting, there are several options for equivalent behavior:

 * Writing code in the endpoint class after start and stop
 * [FeatureStartupTask](/nservicebus/pipeline/features.md#feature-startup-tasks)
 * [Using MEF or Reflection](/samples/plugin-based-config) to run code at startup and shutdown in a pluggable way.


### Achieving the IWantToRunWhenBusStartsAndStops when using the Hosts

 * In the [NServiceBus Host](/nservicebus/hosting/nservicebus-host/) Versions 7 and above, use the [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/nservicebus-host) interface.
 * In the [AzureCloudService Host](/nservicebus/hosting/cloudservices-host/) Versions 7 and above, use the [`IWantToRunWhenEndpointStartsAndStops`](/nservicebus/hosting/cloudservices-host/) interface.


## Versions 5 and below

Instances are:

 * Located by [assembly scanning](/nservicebus/hosting/assembly-scanning.md) and automatically registered into the [configured container](/nservicebus/containers/) during endpoint creation. These are registered as `Instance Per Call`.
 * Created and started as the last step when the endpoint is started.
 * Started after the Transport and any Satellites have started. While all instances of `IWantToRunWhenBusStartsAndStops` are being started, no message will be processed.
 * Created on the same thread that is starting the endpoint.
 * Created by the [Container](/nservicebus/containers/) which means they:
  * Will have dependencies injected.
  * Do not require a default constructor.

Once created `Start` is called on each instance in parallel. Each `Start` call is made on a different background thread. Instances of `IWantToRunWhenBusStartsAndStops` are kept internally to be stopped when the endpoint is disposed.

NOTE: The call to `IStartableBus.Start` may return before all instances of `IWantToRunWhenBusStartsAndStops.Start` are completed.

When the endpoint is disposed, all instances of `IWantToRunWhenBusStartsAndStops` are stopped by calling their `Stop` method. Each call to `Stop` happens on parallel background threads but the call to `Dispose()` will block until they have all completed. 

NOTE: `Stop` will wait for any outstanding instances of `Start` to complete. If an instance of `IWantToRunWhenBusStartsAndStops` needs to be long running then it must start it's own background thread. Failure to do so will prevent the endpoint from being disposed.


### Exceptions

Exceptions thrown in the constructors of instances of the start and stop interfaces are unhandled by NServiceBus. These will bubble up to the code that starts the the endpoint.

Exceptions raised from the `Start` method will cause a [Critical Error](/nservicebus/hosting/critical-errors.md). As they are run on separate threads, an exception on one `Start` call will not interfere with any others.

Exceptions raised from the `Stop` method will not halt shutdown and will be logged at the Fatal level.


## Code

snippet:lifecycle-EndpointStartAndStop