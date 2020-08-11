---
title: NServiceBus Azure Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Azure Host Version 7 to 8.
reviewed: 2019-06-19
component: CloudServicesHost
related:
 - nservicebus/upgrades/6to7
 - samples/azure/self-host
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Azure Host will be deprecated as of Version 9 and users are recommended to switch to self-hosting for new endpoints. Upgrading existing endpoints is still supported for Version 8.

include: host-deprecate


## Migrating procedure

See the [self-hosting sample](/samples/azure/self-host/) for details. For multi-hosting it is recommended to apply self-hosting with multiple endpoints similar to the [multi-hosting sample](/samples/hosting/generic-multi-hosting).

If process isolation is required between the endpoints it is advised to stay with the latest version of the cloud host. Process isolation will be addressed in the upgrade guides when the host is fully deprecated.


### Configuration

Self-hosting gives access to the same configuration options. See below for migration of host specific configuration APIs.


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


#### Azure Configuration Source

Azure configuration source provided the capability to load configuration settings from either the configuration file or from the cloud services role environment. This logic can simply be replaced by:

snippet: azure-configuration-source-replacement

Depending on whether the endpoint is hosted in a web or workerrole, the configuration file must be resolved from a different location.

snippet: configuration-resolver


#### Role Environment Interaction

Sometimes it is usefull to host an endpoint inside the role environment (e.g. production), or outside of it (e.g. development). The role environment related code available in the cloud services SDK cannot handle this scenario though and will throw exceptions when it is used outside of the runtime. The following class can help resolve this issue by detecting whether the role environment is available.

snippet: safe-role-environment
