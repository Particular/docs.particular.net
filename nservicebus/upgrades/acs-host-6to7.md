---
title: Azure Cloud Services Host Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Cloud Service Host from version 6 to 7.
reviewed: 2020-06-29
component: CloudServicesHost
related:
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---


## Namespace changes

All types specified in this document are now in the `NServiceBus` namespace.


## IConfigureThisEndpoint changes

`IConfigureThisEndpoint.Customize` is passed an instance of `EndpointConfiguration` instead of `BusConfiguration`.

snippet: AzureServiceBusTransportWithAzureHost


## AsA_Host role changes into IConfigureThisHost

The specifiers `IConfigureThisEndpoint` and `AsA_Host` are now merged into `IConfigureThisHost`.

snippet: AsAHost


### Removal of DynamicHostControllerConfig

The `DynamicHostControllerConfig`configuration section has been removed; instead the `IConfigureThisHost.Customize` implementation must return an instance of `HostSettings` which contains the configuration values.

`RoleEnvironment.GetConfigurationSettingValue` can be used to read an existing configuration setting from the `.cscfg` file.


## Removal of dependencies

The host role entry point and host process no longer depend on the following components.

 * Log4Net
 * Ionic.zip
 * Topshelf
 * CommonServiceLocator

If these components are not used for other purposes they may be removed.


## Deprecation of profiles

The infrastructure backing profiles has been removed from the host and therefore the `Development` and `Production` profiles are no longer available. Note that Visual Studio native logging has replaced the profiles. Refer to the [logging documentation](/nservicebus/hosting/cloud-services-host/logging.md) to learn how to set this up.

Any code in custom profile handlers should be moved into the `IConfigureThisEndpoint` or `IConfigureThisHost` configuration extension points.


## IWantToRunWhenEndpointStartsAndStops

A new interface `IWantToRunWhenEndpointStartsAndStops` has been added. This interface replaces the [`IWantToRunWhenBusStartsAndStops`](/nservicebus/lifecycle/endpointstartandstop.md) in the NServiceBus core.


### Interface in NServiceBus version 5 

```csharp
public class Bootstrapper :
    IWantToRunWhenBusStartsAndStops
{
    public void Start()
    {
        // Do startup actions here.
    }

    public void Stop()
    {
        // Do cleanup actions here.
    }
}
```


### Interface in NServiceBus.Hosting.Azure version 7

snippet: 5to6-EndpointStartAndStopCloudHost
