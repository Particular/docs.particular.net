---
title: Hosting in a Windows Service
summary: This sample shows how to host NServiceBus as a Windows Service in process with support for streamlined debugging experience from Visual Studio.
tags:
- Hosting
- Windows Service
- Sample
related:
- nservicebus/hosting
---

## Code walk-through

This sample shows how to host NServiceBus as a Windows Service in process with support for streamlined debugging experience from Visual Studio.

NServiceBus comes with a very functional host exe that abstracts much of the hosting complexity. Its many features include installation, un-installation and configuring the windows service. It provides these features though a reasonable amount of custom code and the use of some powerful libraries like TopShelf. Since the NServiceBus Host is a general solution with dependencies there are some drawback associated with using it.

The sample is a console application whose `Main` entry point detects if the application is run in interactive mode or not; when run in interactive mode the `service` is manually created and invoked as any other C# class instance, otherwise the `Run` method is called to invoke the base `ServiceBase` class API.

snippet:windowsservice-hosting-main

The `OnStart` method, manually called when running in interactive mode and automatically called by the Windows Service when running as service, configures the instance of `IEndpointInstance` (in Version 6) or `IBus` (in Version 3-5):

snippet:windowsservice-hosting-onstart

When the interactive application is shut down or the Windows Service is stopped the `OnStop` method is called perform the required clean up:

snippet:windowsservice-hosting-onstop
