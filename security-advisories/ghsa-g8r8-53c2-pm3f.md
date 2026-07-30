---
title: "GHSA-g8r8-53c2-pm3f"
summary: "Microsoft Security Advisory CVE-2026-47304 – .NET Security Feature Bypass Vulnerability."
reviewed: "2026-07-29"
---


## Security Advisory Id GHSA-g8r8-53c2-pm3f

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-g8r8-53c2-pm3f](https://github.com/advisories/ghsa-g8r8-53c2-pm3f) security advisory: Microsoft Security Advisory CVE-2026-47304 &#8211; .NET Security Feature Bypass Vulnerability.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|NServiceBus.Transport.AzureServiceBus|6.2.3|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus/6.2.3)|
|NServiceBus.Transport.AzureServiceBus.CommandLine|6.2.3|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus.CommandLine/6.2.3) or `dotnet tool update --g NServiceBus.Transport.AzureServiceBus.CommandLine --v 6.2.3`|
|NServiceBus.Transport.AzureServiceBus|6.3.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus/6.3.1)|
|NServiceBus.Transport.AzureServiceBus.CommandLine|6.3.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.AzureServiceBus.CommandLine/6.3.1) or `dotnet tool update --g NServiceBus.Transport.AzureServiceBus.CommandLine --v 6.3.1`|
|ServiceInsight|2.13.3|[The Download Page](https://particular.net/downloads)|


### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
