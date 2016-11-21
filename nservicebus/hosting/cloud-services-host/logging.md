---
title: Logging
summary: Setting up logging in Azure Cloud Services.
tags:
 - Worker Roles
 - Web Roles
 - Azure
 - Cloud
 - Logging
reviewed: 2016-10-21
---

## Logging

The NServiceBus [logging](/nservicebus/logging/) integrates with the Azure Diagnostics service through a simple trace logger.

Azure tooling for Visual Studio will setup the Azure Diagnostics Service and NServiceBus will integrate with it directly. Ensure that the following trace listener is added to the `app.config`, all NServiceBus logs will be forwarded to the diagnostics service. (Version may differ based on Azure tooling version)

Snippet:DiagnosticMonitorTraceListener

Logging settings can than be controlled by configuring the Azure diagnostics service itself using a `.wadcfg` or `.wadcfgx` file. For more information refer to the [Configuring Diagnostics for Azure Cloud Services and Virtual Machines](https://docs.microsoft.com/nl-nl/azure/vs-azure-tools-diagnostics-for-cloud-services-and-virtual-machines) or [Use the Azure Diagnostics Configuration File in Azure SDK 2.4 and earlier](https://msdn.microsoft.com/library/azure/hh411551.aspx) article on MSDN.