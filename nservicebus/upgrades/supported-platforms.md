---
title: Supported platforms
summary: Platforms that are supported by NServiceBus
reviewed: 2018-01-18
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
---

NServiceBus is supported on the .NET Framework and .NET Core.

Note: If possible, packages will target [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) instead of individual platforms. While this means that packages might work with additional platforms, only the .NET Framework and .NET Core are officially supported by Particular Software.


## .NET Framework

NServiceBus is supported for applications targeting the .NET Framework on Windows. For the list of supported Windows versions, refer to [.NET Framework Versions and Dependencies](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies).


## .NET Core

NServiceBus is supported for applications targeting .NET Core on a variety of operating systems:

* **Windows**: All Windows versions according to [.NET Core 2.0 - Supported OS versions](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md) are fully supported.
* **Linux**: All Linux versions according to [.NET Core 2.0 - Supported OS versions](https://github.com/dotnet/core/blob/master/release-notes/2.0/2.0-supported-os.md) are fully supported.
* **macOS**: macOS is supported only for development purposes.

### Packages not supporting .NET Core

Some packages do not currently support .NET Core or running on non-Windows platforms:

* Transports
  * NServiceBus.AmazonSQS - Blocked from running on Linux due to a [bug in the AWS SDK](https://github.com/aws/aws-sdk-net/issues/796).
  * NServiceBus.Azure.Transports.WindowsAzureServiceBus - The new Azure Service Bus library is about to reach feature completeness, which will [unblock development of the transport for .NET Core](https://particular.net/blog/a-new-azure-service-bus-transport-but-not-just-yet).
  * NServiceBus.Transport.Msmq - MSMQ only runs on Windows.
* Persistence
  * NServiceBus.NHibernate - NHibernate 4.x and 5.0 do not support .NET Core, but support is [slated for NHibernate 5.1](https://github.com/nhibernate/nhibernate-core/issues/954).
  * NServiceBus.Persistence.ServiceFabric - Support for Service Fabric is slated for a future minor release.
  * NServiceBus.Persistence.Sql - Microsoft SQL Server and PostgreSQL are supported. Oracle is not supported due to the lack of a .NET Core version of Oracle.ManagedDataAccess. The MySQL library does support .NET Core, but it has not yet been validated to work with SQL Persistence.
* Containers
  * NServiceBus.Spring - Spring.Core does not support .NET Core.
* Loggers
  * NServiceBus.NLog - Waiting for stable release of NLog 4.5.0, which introduces .NET Core support
* Hosts
  * These hosts are being deprecated and will not receive .NET Core support. They are replaced by the [ParticularTemplates package](https://www.nuget.org/packages/ParticularTemplates) containing [templates for use with `dotnet new`](/nservicebus/dotnet-templates.md).
    * NServiceBus.Host
    * NServiceBus.Host32
    * NServiceBus.Hosting.Azure
    * NServiceBus.Hosting.Azure.HostProcess
  * ParticularTemplates - The Windows Service templates will be able to support .NET Core on Windows once a stable version of the [Windows Compatibility Pack](https://www.nuget.org/packages/Microsoft.Windows.Compatibility) is released.
* Other
  * NServiceBus.Metrics.PerformanceCounters - Will be able to support .NET Core on Windows once a stable version of the [Windows Compatibility Pack](https://www.nuget.org/packages/Microsoft.Windows.Compatibility) is released.
  * NServiceBus.Wcf - Microsoft does not support the server aspects of WCF on .NET Core.
