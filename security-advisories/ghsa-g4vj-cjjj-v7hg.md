---
title: "GHSA-g4vj-cjjj-v7hg: Defense in Depth update for NuGet Client"
summary: "Defense in Depth update for NuGet Client"
reviewed: "2026-05-13"
---

## Security Advisory Id GHSA-g4vj-cjjj-v7hg

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-g4vj-cjjj-v7hg](https://github.com/advisories/ghsa-g4vj-cjjj-v7hg) security advisory: Defense in Depth update for NuGet Client.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|Particular.AzureTable.Export|1.2.1|[NuGet](https://www.nuget.org/packages/Particular.AzureTable.Export/1.2.1) or `dotnet tool update --g Particular.AzureTable.Export --v 1.2.1`|

### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
