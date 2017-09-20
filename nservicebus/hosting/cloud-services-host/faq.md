---
title: Azure Cloud Services Host FAQ
summary: Frequently Asked Questions related to the Azure Cloud Services Host.
component: CloudServicesHost
tags:
- Azure
- Hosting
reviewed: 2016-10-21
---

include: cloudserviceshost-deprecated-warning

This document captures some frequently asked questions when hosting NServiceBus in Azure Cloud Services.


### MVC WebRole 'hangs' when hosted in the compute emulator

There is a known issue when enabling NServiceBus in a website that is hosted in the Microsoft Azure compute emulator and when performance counters have been installed (either via the installer or via PowerShell). There are 2 possible workarounds:

 * Do not host the website in the compute emulator, but outside of it.
 * Remove the performance counters using PowerShell `([Diagnostics.PerformanceCounterCategory]::Delete("NServiceBus" ))`.


### Role instance stays in 'Busy' status, or an infinite reboot loop, after deploying a project to the cloud

This is almost always related to an exception happening at startup of the RoleEntryPoint. Typically this is a [TypeLoadException](https://msdn.microsoft.com/en-us/library/system.typeloadexception.aspx) coming from a missing assembly or a bad connection string, one that is still pointing to development storage for example.


### Exceptions occurring at startup are not visible in the logs

When using the diagnostics service in Cloud Services, this service starts in parallel with the startup code. If an exception occurs at this point in time, the code may not be able to call the diagnostics service yet and the exception information may get lost. Use [IntelliTrace](https://msdn.microsoft.com/en-us/library/dd264915.aspx) and [Historical Debugging](https://msdn.microsoft.com/en-us/library/mt228143.aspx) instead to learn more about the cause of the exception.


## Specify Endpoint Name

Set the endpoint name using the `DefineEndpointName(name)` extension method on the endpoint configuration.

snippet: EndpointNameInCodeForAzureHost


## Host Identifier

The cloud services role entry point takes care of updating these values, used for identification of the endpoint instance in ServiceControl, automatically, i.e. the `$.diagnostics.hostdisplayname` defaults to the role name and the `$.diagnostics.hostid` contains the instance ID.

In web roles these values must be set manually, refer to [Override Host Id](/nservicebus/hosting/override-hostid.md) for more information on this topic.
