---
title: Logging
summary: Setting up logging in Azure Cloud Services.
tags:
 - Worker Roles
 - Web Roles
 - Azure
 - Cloud
 - Logging
---

## Logging

The NServiceBus [logging](/nservicebus/logging/) integrates with the Azure Diagnostics service through a simple trace logger. 

Azure tooling for Visual Studio will setup the Azure Diagnostics Service and nservicebus will integrate with it directly. Ensure that the following trace listener is added to the `app.config`, all NServiceBus logs will be forwarded to the diagnostics service. (Version may differ based on Azure tooling version)

Snippet:DiagnosticMonitorTraceListener

Logging settings can than be controlled by configuring the Azure diagnostics service itself using a `.wadcfg` or `.wadcfgx` file. Check out the [MSDN documentation](https://msdn.microsoft.com/library/azure/hh411551.aspx) for more information on this topic.