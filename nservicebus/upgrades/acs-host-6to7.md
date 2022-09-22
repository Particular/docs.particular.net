---
title: Azure Cloud Services Host Upgrade Version 6 to 7
summary: Instructions on how to upgrade Azure Cloud Service Host from version 6 to 7.
reviewed: 2020-06-29
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

```csharp
// For Azure Cloud Service Host version 8.x
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Worker
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        // Configure transport, persistence, etc.
    }
}

// For Azure Cloud Service Host version 7.x
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Worker
{
    public void Customize(EndpointConfiguration endpointConfiguration)
    {
        // Configure transport, persistence, etc.
    }
}

// For Azure Cloud Service Host version 6.x
public class EndpointConfig :
    IConfigureThisEndpoint,
    AsA_Worker
{
    public void Customize(BusConfiguration busConfiguration)
    {
        // Configure transport, persistence, etc.
    }
}
```

## AsA_Host role changes into IConfigureThisHost

The specifiers `IConfigureThisEndpoint` and `AsA_Host` are now merged into `IConfigureThisHost`.

```csharp
// For Azure Cloud Service Host version 8.x
public class EndpointHostConfig :
    IConfigureThisHost
{
    public HostingSettings Configure()
    {
        return new HostingSettings("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
    }
}

// For Azure Cloud Service Host version 7.x
public class EndpointHostConfig :
    IConfigureThisHost
{
    public HostingSettings Configure()
    {
        return new HostingSettings("DefaultEndpointsProtocol=https;AccountName=[ACCOUNT];AccountKey=[KEY];");
    }
}

// For Azure Cloud Service Host version 6.x
public class EndpointHostConfig :
    IConfigureThisEndpoint,
    AsA_Host
{
    public void Customize(BusConfiguration configuration)
    {
    }
}
```

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

The infrastructure backing profiles has been removed from the host and therefore the `Development` and `Production` profiles are no longer available. Note that Visual Studio native logging has replaced the profiles.

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

```csharp
public class Bootstrapper :
    IWantToRunWhenEndpointStartsAndStops
{
    public Task Start(IMessageSession session)
    {
        // Do startup actions here.
        // Either mark Start method as async or do the following
        return Task.CompletedTask;
    }

    public Task Stop(IMessageSession session)
    {
        // Do cleanup actions here.
        // Either mark Stop method as async or do the following
        return Task.CompletedTask;
    }
}
```
