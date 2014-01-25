---
title: Preparing your machine to run NServiceBus
summary: Details the infrastructure components required by NServicebus, and how to install them using Visual Studio&#39;s Package Manager Console
originalUrl: http://www.particular.net/articles/preparing-your-machine-to-run-nservicebus
tags:
- Package Manager Console
- infrastructure components
- prerequisites
- installation
- nuget
---


NServiceBus relies on a few key infrastructure components in order to run properly.

-   DTC
-   MSMQ
-   RavenDB (Note: only port 8080 is being scanned to see if RavenDB is installed.)
-   Performance Counters

When installing NServicebus, the installation process verifies that these components are installed as required. If any of the components is not installed, the installation process will install the missing component.

To manually install these components (or make sure they are installed properly), type the following commands into Package Manager Console inside Visual Studio (Package Manager Console can be found under the menu View, Other Windows)

### For NServiceBus version 4.0 or later:

```
PM> Install-NServiceBusDtc 
PM> Install-NServiceBusMsmq
PM> Install-NServiceBusRavenDB
PM> Install-NServiceBusPerformanceCounters
```

### For NServiceBus version 3.x:

```
PM> Install-Dtc
PM> Install-Msmq
PM> Install-RavenDB
PM> Install-PerformanceCounters
```

The Windows operating system requires that elevated privileges to install and configure some of these infrastructure components. Elevated privileges require that you run these command from a process created with [administrator rights](http://windows.microsoft.com/en-us/windows7/how-do-i-run-an-application-once-with-a-full-administrator-access-token).

For more information on poweshell command line changes in NServiceBus version 4, see [release notes](http://www.particular.net/blog/nservicebus-v4.0-release-notes#powershell) .

RavenDB installation is checked by examining port 8080 and port 8081 on the installation local machine. If RavenDB is installed on any other port it needs to be configured and upgraded manually. For more information on configuring RavenDB see [Installing RavenDB for NServiceBus](using-ravendb-in-nservicebus-installing.md) and [Connecting to RavenDB from NServiceBus](using-ravendb-in-nservicebus-connecting.md)



