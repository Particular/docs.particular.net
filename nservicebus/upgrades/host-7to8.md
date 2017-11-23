---
title: NServiceBus Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Host Version 7 to 8.
reviewed: 2016-04-06
component: Host
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Host will be deprecated as of Version 9 and users are recommended to switch to self hosting for new endpoints. Upgrading existing endpoints is still supported for Version 8.

include: host-deprecate


## Migrating procedure

Switching to self hosting is as easy as creating a new console project and moving relevant code and config over. See the [self hosting sample](/samples/hosting/self-hosting/) for details.


### Configuration

Self hosting gives access to the same configuration options as provided by the host. See below for migration of host specific configuration API's.


#### Custom endpoint configuration

Code in `IConfigureThisEndpoint.Customize` can be transfered as is to the configuration of the self hosted endpoint.


#### Roles

The `AsA_Client` role can be replaced with the following configuration:

snippet: 7to8AsAClient 

The `AsA_Server` role didn't change any configuration and can safely be ignored.

The `UsingTransport<MyTransport>` role can be replaced with the equivalent `EndpointConfiguration.UseTransport<MyTransport>()` call.


#### Endpoint name

The host defaults endpoint name to the namespace of the type implementing `IConfigureThisEndpoint`. When self hosting that name should be passed to the constructor of an `EndpointConfiguration`.


#### Overriding endpoint name

Overriding endpoint name using the `EndpointName` attribute or `DefineEndpointName` method is no longer needed. Pass the relevant name to the constructor of `EndpointConfiguration`.


#### Defining SLA for the endpoint

Defining endpoint SLA via the `EndpointSLA` attribute is no longer supported. 

Install the `NServiceBus.WindowsPerformanceCounters` package and follow the [configuration instructions](/nservicebus/operations/metrics/performance-counters.md).


#### Executing custom code when endpoint starts and stops

The host allowed custom code to run when endpoint started and stopped by implementing `IWantToRunWhenEndpointStartsAndStops`. Since self hosted endpoints are in full control over what happens in their start and stop phases, this code can be executed explicitly when starting or stopping the endpoint.


#### Profiles    

Profiles allowed endpoint configuration to be customized for different runtime environments like dev, test and prod. Self hosted endpoints can instead explictly change configuration based on environment variables, command lines arguments, machine names etc.


### Installation

See the instructions on [how to use `SC.exe` to install self hosted windows services](/nservicebus/hosting/windows-service.md#installation).

Details on how to run custom installers can be found in the [installation documentation](/nservicebus/operations/installers.md).
