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

NServiceBus version 8 does not support .NET Standard as a target framework. Therefore, message contract assemblies that target `netstandard2.0` and reference NServiceBus must be updated. This guidance outlines possible approaches.

Note: When using [unobtrusive mode](https://docs.particular.net/nservicebus/messaging/unobtrusive-mode), the contracts assembly doesn't require a reference to NServiceBus and therefore is not affected. Unobtrusive message contracts can continue to target .NET Standard.

## Change to specific target Platform

If all endpoints are targeting the same platform (e.g. .NET Core 3.1), the message contracts assembly can be changed to align with all other endpoint projects.

## Multi-targeting

If endpoints that share a common contracts assembly target different platforms and frameworks (e.g. both .NET Framework 4.8 and .NET Core 3.1), the target assembly can use [multi-targeting](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) by replacing the `TargetFramwork` element with the `TargetFrameworks` (note the plural) element in the `.csproj` settings:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- Typically, it is sufficient to only list the lowest version needed for each platform -->
    <TargetFrameworks>net48;netcoreapp3.1</TargetFrameworks>
  </PropertyGroup>
</Project>
```

## Migration

When upgrading endpoints one by one, message contracts might need to work across endpoints targeting different versions of NServiceBus. There are several ways to handle this.

### Backwards compatiblity

NServiceBus version 8 can reference message contracts referencing older versions of NServiceBus. Therefore, message contract assemblies can remain targeting NServiceBus version 7 until all endpoints have been successfully upgraded to version 8. This approach allows all endpoints to use the same message contracts assembly version.

### Release new contracts assembly

When deploying a new version of the contracts assembly (e.g. as a NuGet package or directly as a DLL file), the messages remain wire-level compatible as long as the message contract itself is not changed. See the [evolving message contracts](https://docs.particular.net/nservicebus/messaging/evolving-contracts) documentation for more information on updating message contracts. This approach can be chosen if message contract changes don't need to propagate to endpoints that remain on the old version of the message contract assembly.

Note: It is recommended to maintain a stable assembly version across different message contract versions. Indicate the contract version information via the NuGet package version or the file-version.

## Need help?
