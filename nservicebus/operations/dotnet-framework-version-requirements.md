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

Note: Targets 4.5.2 but is compatible with later versions, see [Compatibility](#compatibility)

### NServiceBus Version 6.x

Requires [.NET Framework Version 4.5.2](https://www.microsoft.com/en-au/download/details.aspx?id=17851) (or higher)

Note: Targets 4.5.2 but is compatible with later versions, see [Compatibility](#compatibility)

### NServiceBus Version 5.x

Requires [.NET Framework Version 4.5](https://www.microsoft.com/en-au/download/details.aspx?id=30653) (or higher)

Note: Targets 4.5 but is compatible with later versions, see [Compatibility](#compatibility)


### NServiceBus Version 3.x and 4.x

Requires [.NET Framework Version 4.0](https://www.microsoft.com/en-au/download/details.aspx?id=17851) (or higher)

Note: Targets 4 but is compatible with later versions, see [Compatibility](#compatibility)



## Compatibility

NServiceBus targets a specific version of the .NET Framework but that does not mean user code must be targeting the same version. This only indicates the *minimum required .net framework 

> * You can choose .NET Framework 4.5 as the target framework. This assembly or executable can then be used on any computer that has the .NET Framework 4.5, 4.5.1, 4.5.2, 4.6, 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, or 4.8 installed.
> * You can choose .NET Framework 4.5.1 as the target framework. This assembly or executable should be run only on computers that have .NET Framework 4.5.1 or a later version of the .NET Framework installed.
> * An app can control the version of the .NET Framework on which it runs, but a component can't. Components and class libraries are loaded in the context of a particular app, and that's why they automatically run on the version of the .NET Framework that the app runs on.


The above information is extracted from [Targeting and running .NET Framework apps for version 4.5 and later](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies#targeting-and-running-net-framework-apps-for-version-45-and-later) and [Version compatibility in the .NET Framework](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/version-compatibility)






