---
title: Supported frameworks and platforms
summary: Frameworks and platforms supported by NServiceBus
reviewed: 2024-05-06
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
---

Particular software provides technical support for production environments based on [Microsoft support policy for .NET](https://dotnet.microsoft.com/en-us/platform/support/policy).

Support policies for individual NServiceBus components can be found in the [NServiceBus Packages Supported Versions documentation](supported-versions.md).

1. Not all packages support the available frameworks and versions, due to framework restrictions, obsolete APIs, or transitive dependencies.
2. In this context, "supported" applies only to the framework runtime and doesn't necessarily indicate support for all languages features.

## NServiceBus 9

| Framework | Version | Platform | Support |
|------------------|:-------:|:--------:|:-------:|
| .NET | 8.0 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md) | [Supported until November 10, 2026](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| .NET | 9.0 (STS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/9.0/supported-os.md) | [Supported until May 12, 2026](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core)  |


macOS is supported for development purposes but not for production workloads.

## NServiceBus 8

| Framework | Version | Platform | Support |
|------------------|:-------:|:--------:|:-------:|
| .NET Framework | 4.7.2 or later | [Windows](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies) | Supported |
| .NET | 6.0 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md) | [Supported until November 12, 2024](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| .NET | 7.0 | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md) | [Supported until May 14, 2024](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| .NET | 8.0 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md) | [Supported until November 10, 2026](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |

macOS is supported for development purposes but not for production workloads.

## NServiceBus 7

| Framework | Version | Platform | Support |
|------------------|:-------:|:--------:|:-------:|
| .NET Framework | 4.6.2 or later<sup>1</sup> | [Windows](https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/versions-and-dependencies) | Supported |
| .NET | 6.0 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md) | [Supported until November 12, 2024](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| .NET | 7.0 | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md) | [Supported until May 14, 2024](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |
| .NET | 8.0 (LTS) | [Windows / Linux](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md) | [Supported until November 10, 2026](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) |

<sup>1</sup> Some packages may require a later version of the .NET Framework.

macOS is supported for development purposes but not for production workloads.
