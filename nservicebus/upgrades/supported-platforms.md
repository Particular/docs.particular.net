---
title: Supported frameworks and platforms
summary: Frameworks and platforms supported by NServiceBus
reviewed: 2021-02-04
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
---

| Framework | Version | Platform | Support | Notes |
|------------------|:-------:|:--------:|:-------:|:------|
| .NET Framework | 4.5.2 or later | [Windows](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies) | Supported | Some packages may require a later version. |
| .NET Core | 2.1 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/master/release-notes/2.1/2.1-supported-os.md) | Supported | macOS is supported only for development purposes. |
| .NET Core | 3.1 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/master/release-notes/3.1/3.1-supported-os.md) | Supported | macOS is supported only for development purposes. |
| .NET | 5.0 | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md) | Supported | macOS is supported only for development purposes. |

### Packages not supporting .NET Core/ .NET 5

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
