---
title: Upgrading message contracts from Version 7 to 8
summary: Instructions on how to upgrade shared message contracts from NServiceBus version 7 to version 8.
reviewed: 2022-02-02
component: Core
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

NServiceBus version 8 does not support .NET Standard as a target framework. Therefore, message contract assemblies that target `netstandard2.0` and reference NServiceBus must be updated. This guide shows possible approaches. General guidance is available in the [sharing message contracts documentation](/nservicebus/messaging/sharing-contracts.md).

Note: When using [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md), the contracts assembly doesn't require a reference to NServiceBus and therefore is not affected. Unobtrusive message contracts can continue to target .NET Standard.

## Change to specific target Framework

If all endpoints share the same target framework (e.g., .NET Core 3.1), the message contracts assembly can be changed from targeting .NET Standard to the same target framework as the endpoint projects.

## Multi-targeting

If endpoints sharing a common contracts assembly target different platforms and frameworks (e.g. both .NET Framework 4.8 and .NET Core 3.1), the target assembly can use [multi-targeting](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) by replacing the `TargetFramwork` element with the `TargetFrameworks` (note the plural) element in the `.csproj` settings:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- Typically, it is sufficient to list the lowest version needed -->
    <TargetFrameworks>net48;netcoreapp3.1</TargetFrameworks>
  </PropertyGroup>
</Project>
```

## Get help

For further information and assistance, refer to the [support channels](https://particular.net/support).
