---
title: NServiceBus Azure Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Azure Host Version 7 to 8.
reviewed: 2021-07-23
component: CloudServicesHost
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Azure Host package is deprecated as of Version 9 as Microft has deprecated the Cloud Service hosting model. Users are recommended to switch to a different cloud hosting model.


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
