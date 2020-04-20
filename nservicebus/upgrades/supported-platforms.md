---
title: Supported platforms
summary: Platforms that are supported by NServiceBus
reviewed: 2019-07-19
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
---

NServiceBus is supported on the .NET Framework and .NET Core.

Note: If possible, packages will target [.NET Standard](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) instead of individual platforms. While this means that packages might work with additional platforms, only the .NET Framework and .NET Core are officially supported by Particular Software.


## .NET Framework

NServiceBus is supported for applications targeting the .NET Framework 4.5.2 or later on Windows (note that some packages may require a later version). For the list of supported Windows versions, refer to [.NET Framework Versions and Dependencies](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies).


## .NET Core

NServiceBus is supported for applications targeting .NET Core on a variety of operating systems:

* **Windows**: All Windows versions according to [.NET Core 2.1 - Supported OS versions](https://github.com/dotnet/core/blob/master/release-notes/2.1/2.1-supported-os.md) are fully supported.
* **Linux**: All Linux versions according to [.NET Core 2.1 - Supported OS versions](https://github.com/dotnet/core/blob/master/release-notes/2.1/2.1-supported-os.md) are fully supported.
* **macOS**: macOS is supported only for development purposes.

### Packages not supporting .NET Core

Some packages do not currently support .NET Core or running on non-Windows platforms:

* Transports
  * NServiceBus.Transport.Msmq - `System.Messaging` is not part of .NET Core, so MSMQ can't be supported on .NET Core.
* Containers
  * NServiceBus.Spring - Spring.Core does not support .NET Core.
* Hosts
  * These hosts are being deprecated and will not receive .NET Core support. They are replaced by the [ParticularTemplates package](https://www.nuget.org/packages/ParticularTemplates) containing [templates for use with `dotnet new`](/nservicebus/dotnet-templates.md).
    * NServiceBus.Host
    * NServiceBus.Host32
    * NServiceBus.Hosting.Azure
    * NServiceBus.Hosting.Azure.HostProcess
* Other
  * NServiceBus.Wcf - Microsoft does not support the server aspects of WCF on .NET Core.
