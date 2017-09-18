---
title: Logging
summary: Setting up logging in Azure Cloud Services.
component: CloudServicesHost
versions: '[6,)'
tags:
 - Azure
 - Logging
reviewed: 2016-10-21
related:
 - nservicebus/logging
---

include: cloudserviceshost-deprecated-warning

The NServiceBus [logging](/nservicebus/logging/) integrates with the Azure Diagnostics service through a simple trace logger.

Azure tooling for Visual Studio will setup the Azure Diagnostics Service and NServiceBus will integrate with it directly. Ensure that the following trace listener is added to the `app.config`, all NServiceBus logs will be forwarded to the diagnostics service. (Version may differ based on Azure tooling version)

snippet: DiagnosticMonitorTraceListener

Logging settings can than be controlled by configuring the Azure diagnostics service itself using a `.wadcfg` or `.wadcfgx` file. For more information refer to the [Configuring Diagnostics for Azure Cloud Services and Virtual Machines](https://docs.microsoft.com/en-us/azure/vs-azure-tools-diagnostics-for-cloud-services-and-virtual-machines) or [Use the Azure Diagnostics Configuration File in Azure SDK 2.4 and earlier](https://msdn.microsoft.com/library/azure/hh411551.aspx) article on MSDN.