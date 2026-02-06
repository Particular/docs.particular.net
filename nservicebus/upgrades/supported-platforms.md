---
title: Supported frameworks and platforms
summary: Frameworks and platforms supported by NServiceBus
reviewed: 2024-05-06
related:
 - nservicebus/licensing
 - nservicebus/upgrades/release-policy
 - nservicebus/upgrades/supported-versions
---

Particular software provides technical support for production environments based on [Microsoft's support policy for .NET](https://dotnet.microsoft.com/en-us/platform/support/policy).

Support policies for individual NServiceBus components can be found in the [NServiceBus Packages Supported Versions documentation](supported-versions.md).

1. Not all packages support the available frameworks and versions, due to framework restrictions, obsolete APIs, or transitive dependencies.
2. In this context, "supported" applies only to the framework runtime and doesn't necessarily indicate support for all languages features.

## NServiceBus major versions

Each major version of NServiceBus is built against a specific framework version which will not change until the next major version. Software built with that version of NServiceBus can be used on any version of .NET greater than or equal to the version on which it was built. After new versions of .NET are released, Particular Software updates all of our automated tests to ensure that still-supported versions of NServiceBus can continue to run when upgraded to the new .NET version.

| NServiceBus Version | Build Targets<sup>1</sup>                  | Runs On |
|---------------------|:------------------------------------------:|:--------:|
| NServiceBus 10      | .NET 10                                    | .NET 10 and up |
| NServiceBus 9       | .NET 8                                     | .NET 8 and up |
| NServiceBus 8       | .NET 6<br/>.NET Framework 4.7.2            | .NET 6 and up<br/>.NET Framework 4.7.2 and up |
| NServiceBus 7       | .NET Standard 2.0<br/>.NET Framework 4.5.2 | .NET Core 2.0 and up<br/>.NET Framework 4.5.2 and up |

## Supported .NET platforms

When a version of NServiceBus [is supported](supported-versions.md) according to our [support policy](support-policy.md), it can be run in production on any Windows or Linux platform supported by Microsoft according to the Microsoft resources in the following table, until [Microsoft's end-of-support date](https://dotnet.microsoft.com/en-us/platform/support/policy/dotnet-core) for that .NET version, after which it may be run on a newer .NET version that is still supported by Microsoft. macOS platforms are supported as development environments but not for production workloads. Other platforms like televisions and Android devices are not supported.

| .NET Version | Release Type | Supported Platforms | .NET Support End Date |
|--------------|:------------:|:-------------------:|:---------------------:|
| .NET 10 | LTS | [.NET 10 Platforms](https://github.com/dotnet/core/blob/main/release-notes/10.0/supported-os.md)   | November 14, 2028 |
| .NET 9  | STS | [.NET 9 Platforms](https://github.com/dotnet/core/blob/main/release-notes/9.0/supported-os.md)     | November 10, 2026 |
| .NET 8  | LTS | [.NET 8 Platforms](https://github.com/dotnet/core/blob/main/release-notes/8.0/supported-os.md)     | November 10, 2026 |
| .NET 7  | STS | [.NET 7 Platforms](https://github.com/dotnet/core/blob/main/release-notes/7.0/supported-os.md)     | May 14, 2024 |
| .NET 6  | LTS | [.NET 6 Platforms](https://github.com/dotnet/core/blob/main/release-notes/6.0/supported-os.md)     | November 12, 2024 |
| .NET 5  |     | [.NET 5 Platforms](https://github.com/dotnet/core/blob/main/release-notes/5.0/5.0-supported-os.md) | May 10, 2022 |
| .NET Core 3.1 | | [.NET Core 3.1 Platforms](https://github.com/dotnet/core/blob/main/release-notes/3.1/3.1-supported-os.md) | December 13, 2022 |
| .NET Core 3.0 | | [.NET Core 3.0 Platforms](https://github.com/dotnet/core/blob/main/release-notes/3.0/3.0-supported-os.md) | March 3, 2020 |
| .NET Core 2.2 | | [.NET Core 2.2 Platforms](https://github.com/dotnet/core/blob/main/release-notes/2.2/2.2-supported-os.md) | December 23, 2019 |
| .NET Core 2.1 | | [.NET Core 2.1 Platforms](https://github.com/dotnet/core/blob/main/release-notes/2.1/2.1-supported-os.md) | August 21, 2021 |
| .NET Core 2.0 | | [.NET Core 2.0 Platforms](https://github.com/dotnet/core/blob/main/release-notes/2.0/2.0-supported-os.md) | October 1, 2018 |
