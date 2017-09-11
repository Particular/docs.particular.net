---
title:  Microsoft Logging
summary: Logging to Microsoft.Extensions.Logging
reviewed: 2017-09-11
component: MsLogging
tags:
 - Logging
related:
 - nservicebus/logging
---

Support for writing all NServiceBus log entries to  [Microsoft.Extensions.Logging](https://github.com/aspnet/Logging).


## Usage

snippet: MsLoggingInCode


## Usage when hosting

As `LoggerFactory` implements [IDisposable](https://msdn.microsoft.com/en-us/library/system.idisposable.aspx) it must be disposed of after `IEndpointInstance.Stop` has been called. The process for doing this will depend on how the endpoint is being hosted.


### In a windows service

When [hosting in a windows service](/nservicebus/hosting/windows-service.md) `LoggerFactory` should be disposed of as part of the [ServiceBase.OnStop](https://msdn.microsoft.com/en-us/library/system.serviceprocess.servicebase.onstop.aspx) execution.

snippet: MsLoggingInService