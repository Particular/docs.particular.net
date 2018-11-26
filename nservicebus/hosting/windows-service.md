---
title: Windows Service Installation
summary: How to install an NServiceBus endpoint as a Windows service
reviewed: 2018-11-22
component: Core
tags:
 - Hosting
related:
 - nservicebus/dotnet-templates
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
---

## Installation

When self-hosting a Windows Service, the startup code is in full control of installation. Windows supports these features though the use of the [Service Control tool](https://technet.microsoft.com/en-us/library/cc754599.aspx). For example a basic install and uninstall commands would be:

```dos
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
sc.exe delete SalesEndpoint
```

For completeness, here are some other common usages of the Service Control tool:


### Setting the Windows Service name

The Windows Service name can be configured at creation time, as follows:

```dos
sc.exe create [ServiceName] binpath= [BinaryPathName]
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Setting the Display name

The display name can be configured, at creation time, using the `displayname` argument:

```dos
sc.exe create [ServiceName] displayname= [Description] binpath= [BinaryPathName]
sc.exe create SalesEndpoint displayname= "Sales Endpoint" binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Setting the Description

The description can be changed, after the Windows Service has been created, using the [sc description](https://technet.microsoft.com/en-us/library/cc742069.aspx) command.

```dos
sc.exe description [ServiceName] [Description]
sc.exe description SalesEndpoint "Service for hosting the Sales Endpoint"
```


### Specifying Service dependencies

The dependencies of a Windows Service can be configured after it has been created using the [sc config](https://technet.microsoft.com/en-us/library/cc990290.aspx) command.

```dos
sc.exe config [ServiceName] depend= <Dependencies(separated by / (forward slash))>
sc.exe config SalesEndpoint depend= MSMQ/MSDTC/RavenDB
```


### Setting the Restart Recovery Options

Windows has a Windows Service recovery mechanism that makes sure a crashed process will be restarted.

The endpoint can fail when using the [NServiceBus Host](nservicebus-host/) or when [self-hosting and implementing a critical error handler that exits the process](critical-errors.md#default-behavior) in case a critical error occurs. 

If Windows Service Recovery is not configured, message processing will halt. Therefore it's important to configure recovery options when hosting an NServiceBus endpoint as a Windows Service. 

The recovery options can be adjusted via the Services dialog or via `sc.exe`. Note that the command line tool has advanced configuration options.


#### Configuring Windows Service Recovery via sc.exe

The default restart duration is 1 minute when enabling recovery via the Windows Service Management Console, but a different restart duration may be defined for the subsequent restarts using `sc.exe`. 

The following example will restart the process after 5 seconds the first time, after 10 seconds the second time and then every 60 seconds. The Restart Service Count is reset after 1 hour (3600 seconds) of uninterrupted work since the last restart.

```dos
sc.exe failure [ServiceName] reset= [seconds] actions= restart/[milliseconds]/restart/[milliseconds]/restart/[milliseconds]
sc.exe failure SalesEndpoint reset= 3600 actions= restart/5000/restart/10000/restart/60000
```


#### Configuring Service Recovery via Windows Service properties

Open the services window, select the endpoint Windows Service and open its properties. Then open the Recovery tab to adjust the settings:

![Windows Service properties Recovery tab](service-properties.png)

NOTE: Restart durations are only configurable using `sc.exe`.


### Username and password

Username and password can be configured at creation time using the `obj` and `password` parameters.

```dos
sc.exe create [ServiceName] obj= [AccountName] password= [Password] binpath= [BinaryPathName]
sc.exe create SalesEndpoint obj= MyDomain\SalesUser password= 9t6X7gkz binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Start mode

The Windows Service start mode can be configured at creation time using the `start` parameter.

```dos
sc.exe create [ServiceName] start= {auto | demand | disabled} binpath= [BinaryPathName]
sc.exe create SalesEndpoint start= demand binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Uninstall

A Windows Service can be uninstalled using the [sc delete](https://technet.microsoft.com/en-us/library/cc742045.aspx) command.

```dos
sc.exe delete [ServiceName]
sc.exe delete SalesEndpoint
```


## Compared to NServiceBus Host


### Code similarity

When using a self-hosted approach inside a Windows Service, this code will share many similarities with other hosting code such as send-only clients and web service hosting. This similarity will result in more consistent (and hence easier to understand) code and increased opportunities for code re-use.


### Performance

Self-hosting is a specific solution to a problem that can be more specialized and has fewer dependencies. This results in

 * Reduced memory usage
 * Faster startup/debugging time
 * Smaller deployment size


### Debugging

The NServiceBus Host is a non-trivial piece of software, especially when including its dependency on TopShelf. As such the NServiceBus Host can add complexity to debugging issues. Self-hosting uses fewer layers of abstraction which results in a simpler debugging experience.


### Controlling the entry point

When using the NServiceBus Host, the host is calling the endpoint configuration code. As such, the configuration code and behaviors (such as startup and shutdown) need to plug into very specific APIs. For example `IWantCustomLogging`, `IWantCustomInitialization`, `IWantToRunWhenEndpointStartsAndStops` and `IConfigureLogging`. If the scenario is inverted, i.e. developer code calls NServiceBus configuration, then the requirement for these APIs no longer exists.
