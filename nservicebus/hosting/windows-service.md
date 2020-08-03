---
title: Windows Service Installation
summary: How to install an NServiceBus endpoint as a Windows service
reviewed: 2020-08-03
component: Core
isLearningPath: true
related:
 - nservicebus/dotnet-templates
 - nservicebus/lifecycle
 - samples/startup-shutdown-sequence
 - samples/endpoint-configuration
 - samples/hosting/generic-host
---

## Installation

When self-hosting a Windows Service, the startup code is in full control of installation. Windows supports these features though the use of the [Service Control tool](https://technet.microsoft.com/en-us/library/cc754599.aspx). For example, a basic install and uninstall commands would be:

```dos
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe --run-as-service"
sc.exe delete SalesEndpoint
```

For completeness, here are some other common usages of the Service Control tool:


### Setting the Windows Service name

The Windows Service name can be configured at creation time, as follows:

```dos
sc.exe create [ServiceName] binpath= [BinaryPathName]
sc.exe create SalesEndpoint binpath= "c:\SalesEndpoint\SalesEndpoint.exe --run-as-service"
```


### Setting the Display name

The display name can be configured, at creation time, using the `displayname` argument:

```dos
sc.exe create [ServiceName] displayname= [Description] binpath= [BinaryPathName]
sc.exe create SalesEndpoint displayname= "Sales Endpoint" binpath= "c:\SalesEndpoint\SalesEndpoint.exe --run-as-service"
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
sc.exe create SalesEndpoint obj= MyDomain\SalesUser password= 9t6X7gkz binpath= "c:\SalesEndpoint\SalesEndpoint.exe --run-as-service"
```


### Start mode

The Windows Service start mode can be configured at creation time using the `start` parameter.

```dos
sc.exe create [ServiceName] start= {auto | demand | disabled} binpath= [BinaryPathName]
sc.exe create SalesEndpoint start= demand binpath= "c:\SalesEndpoint\SalesEndpoint.exe --run-as-service"
```


### Uninstall

A Windows Service can be uninstalled using the [sc delete](https://technet.microsoft.com/en-us/library/cc742045.aspx) command.

```dos
sc.exe delete [ServiceName]
sc.exe delete SalesEndpoint
```