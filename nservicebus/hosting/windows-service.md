---
title: Windows Service Hosting
summary: How to host NServiceBus in a Windows Service
reviewed: 2016-07-28
component: Core
tags:
- Hosting
- Windows Service
related:
 - samples/hosting/windows-service
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
---

Running inside a [Windows Service](https://docs.microsoft.com/en-us/dotnet/framework/windows-services/introduction-to-windows-service-applications) is the most common approach to hosting NServiceBus.


## Example Windows Service Hosting

 * Create a new Console Application.
 * Reference `System.ServiceProcess.dll`.
 * Change the program to inherit from. [ServiceBase](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.aspx)

snippet: windowsservicehosting

NOTE: The use of `ServiceHelper.IsService()` to provide a dual console/service experience, i.e. this process can be executed from the command line or run as a Windows Service.


snippet: ServiceHelper


## Bootstrapping NuGet

There is a [Bootstrapping starter package](https://www.nuget.org/packages/NServiceBus.Bootstrap.WindowsService) on NuGet that automates most of the above code.


### How to use

Create a new Console Application (**.NET 4.5.2 or higher**) and install the NuGet package. A minimal NServiceBus configuration will be setup along with a `ProgramService.cs` class that can be used as both a interactive console for development purposes and a windows service for production use.

NOTE: This will also delete the default `Program.cs` since it is superseded by `ProgramService.cs`.


### Single use NuGet

This is a "single use NuGet". So it after install, and adding code to the project, it will remove itself. Since it is single use there will never be any "upgrade", this is a "use and then own the code" approach.


### For new self hosting applications

This NuGet helps get started on a new self hosted NServiceBus application. For existing NServiceBus projects the problems this NuGet attempts to address are most likely already solved.

### Transport

The LearningTransport is selected by default. 

WARNING: Choose a production grade transport before deploying to production.


### Persistence

No persistence is needed since the LearningTransport is used and no sagas is present. Consult the indivudal transport and Saga documentation for specific storage need.


## Installation

When self-hosting a Windows service the startup code is in full control of installation. Windows supports these features though the use of the [Service Control tool](https://technet.microsoft.com/en-us/library/cc754599.aspx). For example a basic install and uninstall commands would be:

```dos
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
sc.exe delete SalesEndpoint
```

For completeness here are some other common usages of the Service Control tool:


### Service Name

The Windows Service Name can be configured, at creation time, as follows:

```dos
sc.exe create [ServiceName] binpath= [BinaryPathName]
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Display Name

Display name can be configured, at creation time, using the `displayname` argument:

```dos
sc.exe create [ServiceName] displayname= [Description] binpath= [BinaryPathName]
sc.exe create SalesEndpoint displayname= "Sales Endpoint" binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Description

Description can be changed, after the service has been created, using the [sc description](https://technet.microsoft.com/en-us/library/cc742069.aspx) command.

```dos
sc.exe description [ServiceName] [Description]
sc.exe description SalesEndpoint "Service for hosting the Sales Endpoint"
```


### Service Dependencies

Service dependencies can be configured after the service has been created using the [sc config](https://technet.microsoft.com/en-us/library/cc990290.aspx) command.

```dos
sc.exe config [ServiceName] depend= <Dependencies(separated by / (forward slash))>
sc.exe config SalesEndpoint depend= MSMQ/MSDTC/RavenDB
```


### Restart Recovery

Windows has a Windows Service Recovery mechanism that makes sure that a crashed service will be restarted.

The endpoint can fail when using the [NServiceBus Host](nservicebus-host/) or when [self hosting and implementing a critical error handler that exits the process](critical-errors.md#default-action-handling-in-nservicebus) in case a critical error occurs. 

If Windows Service Recovery is not configured, then the message processing will halt. Therefore it's important to configure Recovery options, when hosting an NServiceBus endpoint as a Windows Service. 

The Recovery options can be adjusted via Services dialog or via `sc.exe`. Note that the command line tool has advanced configuration options.


#### Configuring Service Recovery via sc.exe

The default restart duration is 1 minute when enabling recovery via the Windows Service management console, but a different restart duration may be defined for the subsequent restarts using `sc.exe`. 

The following example will restart the service after 5 seconds the first time, after 10 seconds the second time and then every 60 seconds. The restart service count is reset after 1 hour (3600 seconds) of uninterrupted work since the last restart.

```dos
sc.exe failure [ServiceName] reset= [seconds] actions= restart/[milliseconds]/restart/[milliseconds]/restart/[milliseconds]
sc.exe failure SalesEndpoint reset= 3600 actions= restart/5000/restart/10000/restart/60000
```


#### Configuring Service Recovery via Windows Service properties

Open the services window, select the endpoint Windows service and open its properties. Then open the Recovery tab to adjust the settings:

![Windows Service properties Recovery tab](service-properties.png)

NOTE: Restart durations are only configurable using `sc.exe`.


### Username and Password

Username and password can be configured, at creation time, using the `obj` and `password` parameters.

```dos
sc.exe create [ServiceName] obj= [AccountName] password= [Password] binpath= [BinaryPathName]
sc.exe create SalesEndpoint obj= MyDomain\SalesUser password= 9t6X7gkz binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Start Mode

The Windows Service start mode can be configured, at creation time, using the `start` parameter.

```dos
sc.exe create [ServiceName] start= {auto | demand | disabled} binpath= [BinaryPathName]
sc.exe create SalesEndpoint start= demand binpath= "c:\SalesEndpoint\SalesEndpoint.exe"
```


### Uninstall

A service can be uninstalled using the [sc delete](https://technet.microsoft.com/en-us/library/cc742045.aspx) command.

```dos
sc.exe delete [ServiceName]
sc.exe delete SalesEndpoint
```


## Compared to NServiceBus Host


### Code similarity

When using a self hosted approach, inside a windows service, this code will share many similarities with other hosting code such as send only clients and web service hosting. This similarity will result in more consistent (and hence easier to understand code and increased opportunities for code re-use.


### Performance

Self host is a specific solution to a problem that can be more specialized and has less dependencies. This results in

 * Reduced memory usage
 * Faster startup/debugging time
 * Smaller deployment size


### Debugging

The NServiceBus Host is a non-trivial piece of software, especially when including its dependency on TopShelf. As such the NServiceBus Host can add complexity to debugging issues. Taking full control via self hosting allows less layers of abstraction which result in a simpler debugging experience.


### Controlling the entry point

When using the NServiceBus Host, the host is calling the endpoint configuration code. As such the configuration code and behaviors (such as startup and shutdown) need to plug into very specific APIs. For example `IWantCustomLogging`, `IWantCustomInitialization`, `IWantToRunWhenEndpointStartsAndStops` and `IConfigureLogging`. If the scenario is inverted, i.e. the developers code calls NServiceBus configuration, then the requirement for these APIs no longer exists.
