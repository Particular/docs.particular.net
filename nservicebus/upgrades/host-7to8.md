---
title: NServiceBus Host Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus Host Version 7 to 8.
reviewed: 2020-12-02
component: Host
related:
 - nservicebus/upgrades/6to7
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---

The NServiceBus Host will be deprecated as of version 9; it is recommended to switch to self-hosting or the [generic host](/nservicebus/hosting/extensions-hosting.md) for new endpoints. Upgrading existing endpoints is still supported for version 8.

include: host-deprecate


## Migrating procedure

Switching to self-hosting can be done by using the [generic host](/nservicebus/hosting/extensions-hosting.md), [NServiceBus Windows Service](/nservicebus/dotnet-templates.md#nservicebus-windows-service), or [NServiceBus Docker Container](/nservicebus/dotnet-templates.md#nservicebus-docker-container) templates to create a new project, then moving the relevant code and configuration to the new project.


### Configuration

Self-hosting provides access to the same configuration options as provided by the Host. See below for the migration of host-specific configuration APIs.


#### Custom endpoint configuration

Code in `IConfigureThisEndpoint.Customize` can be transferred as-is to the configuration of the self-hosted endpoint.


#### Roles

The `AsA_Client` role can be replaced with the following configuration:

snippet: 7to8AsAClient 

NOTE: When trasitioning from `AsA_Client` to self-hosting the equivalent setting for the transport transaction mode is `None`. Make sure that message loss is acceptable. See [transport transactions](/transports/transactions.md) documentation for more details.

The `AsA_Server` role didn't change any configuration and can safely be ignored.

The `UsingTransport<MyTransport>` role can be replaced with the equivalent `EndpointConfiguration.UseTransport<MyTransport>()` call.


#### Endpoint name

The Host defaulted the endpoint name to the namespace of the type implementing `IConfigureThisEndpoint`. When self-hosting, that name should be passed to the constructor of an `EndpointConfiguration`.


#### Overriding the endpoint name

Overriding the endpoint name using the `EndpointName` attribute or `DefineEndpointName` method is no longer needed. Pass the relevant name to the constructor of `EndpointConfiguration`.


#### Defining an SLA for the endpoint

Defining an endpoint's SLA via the `EndpointSLA` attribute is no longer supported. 

Install the `NServiceBus.WindowsPerformanceCounters` package and follow the [configuration instructions](/monitoring/metrics/performance-counters.md).


#### Executing custom code when the endpoint starts and stops

The Host allowed custom code to run when an endpoint started and stopped by implementing `IWantToRunWhenEndpointStartsAndStops`. Since self-hosted endpoints are in full control over what happens in their start and stop phases, this code can be executed explicitly when starting or stopping the endpoint.


#### Profiles    

Profiles allowed endpoint configuration to be customized for different runtime environments like dev, test, and prod. Self-hosted endpoints can instead explicitly change configuration based on environment variables, command-line arguments, machine names, etc.


### Installation

See the instructions on [how to use `SC.exe` to install self-hosted windows services](/nservicebus/hosting/windows-service.md#installation).

Details on how to run custom installers can be found in the [installation documentation](/nservicebus/operations/installers.md).
