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

The NServiceBus will be deprecated as of Version 9 and users are recommended to avoid using it for new endpoints. Upgrading existing endpoints is still supported for Version 8

## Incompatible with NuGet X.Y

NuGet no longer support packages adding files and modifying project files. This means that installing the host won't result in a runnable endpoint.

TBD: Is it possible/should we add instructions on how to manually modify the csproj to get this working?


## Incompatible with the new Visual Studio project system

TBD: What is actally not compatible?

## Migrating procedure

Create a new console application project. According to the following guidelined LINK TO SAMPLE

### Configuration

Self hosting gives access to the same configuration options. See below for migration of host specific configuration API's.

#### Custom endpoint configuration

public interface IConfigureThisEndpoint
{
    void Customize(NServiceBus.EndpointConfiguration configuration);
}


#### Roles

public interface AsA_Client 

#### Overriding endpoint name

public static void DefineEndpointName(this NServiceBus.EndpointConfiguration configuration, string endpointName)
public sealed class EndpointNameAttribute : System.Attribute

#### Defining SLA for the endpoint

    public sealed class EndpointSLAAttribute : System.Attribute
{
    public EndpointSLAAttribute(string sla) { }
    public System.TimeSpan SLA { get; }
}

#### Executing custom code on start and stop

    public static void RunWhenEndpointStartsAndStops(this NServiceBus.EndpointConfiguration configuration, NServiceBus.IWantToRunWhenEndpointStartsAndStops startableAndStoppable)

#### Profiles    
public interface Integration : NServiceBus.IProfile { }
public interface IProfile { }
public interface IWantTheListOfActiveProfiles
public interface PerformanceCounters : NServiceBus.IProfile { }
public interface Production : NServiceBus.IProfile { }
public interface Lite : NServiceBus.IProfile { }

[System.ObsoleteAttribute(@"PerformanceCounters has been moved to the external nuget package 'NServiceBus.Metrics.PerformanceCounters'. Add an extra package reference and then call endpointConfiguration.EnableCriticalTimePerformanceCounter(); and endpointConfiguration.EnableSLAPerformanceCounter(); inside the IConfigureThisEndpoint.Customize(). Will be removed in version 9.0.0.", true)]

### Installation

TBD