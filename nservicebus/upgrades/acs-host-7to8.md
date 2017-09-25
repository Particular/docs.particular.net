---
title: NServiceBus Azure Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Azure Host Version 7 to 8.
reviewed: 2017-09-21
component: CloudServicesHost
related:
 - nservicebus/upgrades/6to7
 - samples/azure/self-host
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Azure Host will be deprecated as of Version 9 and users are recommended to switch to self hosting for new endpoints. Upgrading existing endpoints is still supported for Version 8.

include: host-deprecate


## Installation incompatible with NuGet

NuGet no longer support packages adding source files and modifying project files. This means that installing the host won't result in a runnable endpoint. Existing endpoints using the host can be upgraded without issues.


## Migrating procedure

See the [self hosting sample](/samples/azure/self-host/) for details. For multi-hosting it is recommended to apply self-hosting with multiple endpoints similar to the [multi hosting sample](/samples/hosting/multi-hosting).

If process isolation is required between the endpoints it is advised to stay with the latest version of the cloud host. Process isolation will be addressed in the upgrade guides when the host is fully deprecated.


### Configuration

Self hosting gives access to the same configuration options. See below for migration of host specific configuration APIs.


#### Custom endpoint configuration

Configuration code in `IConfigureThisEndpoint.Customize` can be transferred as-is to the configuration of the self-hosted endpoint.


#### Roles

The `AsA_Worker` role didn't change any configuration and can safely be ignored.


#### Endpoint name

The host defaults the endpoint name to the namespace of the type implementing `IConfigureThisEndpoint`. Pass that value to the name to the constructor of `EndpointConfiguration`.


#### Overriding endpoint name

Overriding endpoint name using the `EndpointName` attribute or `DefineEndpointName` method is no longer needed. Pass the relevant name to the constructor of `EndpointConfiguration`.


#### Executing custom code on start and stop

The host allowed custom code to run at start and stop by implementing `IWantToRunWhenEndpointStartsAndStops`. Since self-hosted endpoints are in full control over start and stop operations this code can be executed explicitly when starting/stopping.
