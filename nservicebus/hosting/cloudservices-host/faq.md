---
title: Azure Cloud Services Host FAQ
summary: Frequently Asked Questions related to the Azure Cloud Services Host.
tags:
- Cloud
- Azure
- Hosting
---

This document captures some frequently asked questions when hosting nservicebus in azure cloud services

### My MVC webrole 'hangs' when hosted in the compute emulator

There is a known issue when enabling NServiceBus in a website that is hosted in the Microsoft Azure compute emulator and when performance counters have been installed (either via the installer or via PowerShell). There are 2 possible workarounds:

* Do not host the website in the compute emulator, but outside of it
* Remove the performance counters using PowerShell `([Diagnostics.PerformanceCounterCategory]::Delete( "NServiceBus" ))`


### My role instance stays in 'Busy' status, or an infinite reboot loop, after deploying my project to the cloud

This is almost always related to an exception happening at startup of the roleentrypoint. Typically this is a [TypeLoadException](https://msdn.microsoft.com/en-us/library/system.typeloadexception.aspx) coming from a missing assembly or a bad connection string, one that is still pointing to development storage for example.


### Exceptions occurring at startup are not visible in the logs

When using the diagnostics service in cloud services, this service starts in parallel with the startup code. If an exception occurs at this point in time, the code may not be able to call the diagnostics service yet and the exception information may get lost. Use intellitrace and historical debugging instead to learn more about the cause of the exception.
