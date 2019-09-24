---
title: .NET Framework requirements
summary: List of the .NET Framework requirements for NServiceBus.
reviewed: 2019-08-06
related:
 - nservicebus/operations
tags:
redirects:
 - nservicebus/nservicebus-net-framework-version-requirements
---

### NServiceBus Version 7.x

Requires [.NET Core 2.0](https://www.microsoft.com/net/core/) (or higher) **or** [.NET Framework Version 4.5.2](https://www.microsoft.com/en-au/download/details.aspx?id=42642) (or higher)


### NServiceBus Version 6.x

Requires [.NET Framework Version 4.5.2](https://www.microsoft.com/en-au/download/details.aspx?id=17851) (or higher)


### NServiceBus Version 5.x

Requires [.NET Framework Version 4.5](https://www.microsoft.com/en-au/download/details.aspx?id=30653) (or higher)


### NServiceBus Version 3.x and 4.x

Requires [.NET Framework Version 4.0](https://www.microsoft.com/en-au/download/details.aspx?id=17851) (or higher)


## Compatibility

NServiceBus targets a specific version of the .NET Framework but that does not mean user code must be targeting the same version. This only indicates the *minimum* required .NET Framework. The actual framework used at runtime is determined by the application.

See [Targeting and running .NET Framework apps for version 4.5 and later](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies#targeting-and-running-net-framework-apps-for-version-45-and-later) and [Version compatibility in the .NET Framework](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/version-compatibility) for more information.
