---
title: Hosting in a Windows Service
summary: Hosting NServiceBus as a Windows Service in process with support for streamlined debugging experience from Visual Studio.
reviewed: 2016-03-21
component: Core
tags:
 - Hosting
 - Windows Service
related:
 - nservicebus/dotnet-templates
 - nservicebus/hosting
---

## Code walk-through

This sample shows how to host NServiceBus as a Windows Service in process with support for streamlined debugging experience from Visual Studio.

NServiceBus comes with a host exe that abstracts much of the hosting complexity. Its many features include installation, un-installation and configuring the windows service. It provides these features though a reasonable amount of custom code and the use of some powerful libraries like [TopShelf](http://topshelf-project.com/). Since the NServiceBus Host is a general solution with dependencies there are some drawback associated with using it.

The sample is a console application whose `Main` entry point detects if the application is run in interactive mode or not; when run in interactive mode the `service` is manually created and invoked as any other C# class instance, otherwise the `Run` method is called to invoke the base `ServiceBase` class API.

snippet: windowsservice-hosting-main

The `OnStart` method, manually called when running in interactive mode and automatically called by the Windows Service when running as service, configures the Endpoint Instance:

snippet: windowsservice-hosting-onstart

When the interactive application is shut down or the Windows Service is stopped the `OnStop` method is called perform the required clean up:

snippet: windowsservice-hosting-onstop


### ServiceHelper

To detect if the current process is running as a service the parent process is checked.


include: servicehelper

snippet: ServiceHelper


### Service Management

Management is done using the [Service Control tool](https://technet.microsoft.com/en-us/library/cc754599.aspx).

See also [windows-service installation](/nservicebus/hosting/windows-service.md#installation).


#### Installation

```
sc.exe create nsbSample binpath= "\"[Full Directory]\Sample.exe\""
```


#### Uninstall

```
sc.exe delete nsbSample
```


partial: netcore
