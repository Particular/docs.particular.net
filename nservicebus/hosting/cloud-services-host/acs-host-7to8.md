---
title: NServiceBus Azure Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Azure Host Version 7 to 8.
reviewed: 2017-09-12
component: CloudServicesHost
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Azure Host will be deprecated as of Version 9 and users are recommended to switch to self hosting for new endpoints. Upgrading existing endpoints is still supported for Version 8

## Incompatible with NuGet 4 and higher

NuGet no longer support packages adding source files and modifying project files. This means that installing the host won't result in a runnable endpoint. Existing endpoints using the host can be upgraded without issues.


## Migrating procedure

See the [self hosting sample](/samples/hosting/self-hosting/) for details.

### Configuration

Self hosting gives access to the same configuration options. See below for migration of host specific configuration API's.

#### Custom endpoint configuration

Configuration code in `IConfigureThisEndpoint.Customize` can be transfered as is to the configuration of the self hosted endpoint.

#### Roles

The `AsA_Worker` role didn't change any configuration and can safely be ignored.

#### Endpoint name

The host defaults the endpoint name to the namespace of the type implementing `IConfigureThisEndpoint`. Just pass that value to the name to the constructor of `EndpointConfiguration`.


#### Overriding endpoint name

Overriding endpoint name using the `EndpointName` attribute or `DefineEndpointName` method is no longer needed. Pass the relevant name to the constructor of `EndpointConfiguration`.


#### Executing custom code on start and stop

The host allowed custom code to run at start and stop by implementing `IWantToRunWhenEndpointStartsAndStops`. Since self hosted endpoints are in full control over start and stop this code can be executed explicitly when starting/stopping.