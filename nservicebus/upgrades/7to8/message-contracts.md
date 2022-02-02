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

Since NServiceBus version 8 no longer supports .NET Standard, message contract assemblies refrencing NServiceBus might need to be updated. This guidance outlines possible approaches.

Note: When using [unobtrusive mode](TODO), the contracts assembly doesn't require a reference to NServiceBus and therefore is not affected. Unobtrusive message contracts can continue to target .NET Standard.

## Change to specifc target Platform

If all endpoints are targeting the same platform (e.g. .NET Core 3.1), the message contracts assembly can be changed to be aligned with all other endpoint projects.

## Multi-targeting

If endpoints sharing a contracts assembly are targeting different platforms and frameworks (e.g. .NET Framework 4.8 and .NET Core 3.1), the target assembly can be changed to use [multi-targeting](https://docs.microsoft.com/en-us/dotnet/standard/library-guidance/cross-platform-targeting#multi-targeting) by replacing the `TargetFramwork` element with the `TargetFrameworks` element in the `.csproj` settings:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <!-- Typically, it is sufficient to only list the lowest version needed for each platform -->
    <TargetFrameworks>net48;netcoreapp3.1</TargetFrameworks>
  </PropertyGroup>
</Project>
```

## Migration

When upgrading endpoints one-by-one, message contracts might need to work across endpoints targeting different versions of NServiceBus. There are multiple ways to handle this:

### Backwards compatiblity

NServiceBus version 8 can reference message contracts that are referencing older versions of NServiceBus. Therefore, message contract assemblies can remain targeting NServiceBus version 7 until all endpoints have been successfully upgraded to version 8. This approach allows to use the same message contracts assembly version across all endpoints.

### Release new contracts assembly

When creating a new version of the contracts assembly (deploying it as a NuGet package or directly as a DLL file), the messages remain wire-level compatible as long as the message contract itself is not changed. See the [evolving message contracts](TODO) documentation for more information on updating message contracts. This approach can be chosen if message contract changes don't need to be propoagated to endpoints that remain on the old version of the message contract assembly.

Note: It is recommended to maintain a stable assembly-version across different message contract versions. Indicate the contract version information via the NuGet package version or the file-version.

## Need help?
