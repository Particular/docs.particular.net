---
title: "GHSA-2fmv-rv2c-59x7"
summary: "Microsoft Security Advisory CVE-2026-70162 - .NET Denial of Service Vulnerability"
reviewed: "2026-10-28"
---

## Security Advisory Id GHSA-2fmv-rv2c-59x7

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-2fmv-rv2c-59x7](https://github.com/advisories/ghsa-2fmv-rv2c-59x7) security advisory: Microsoft Security Advisory CVE-2026-70162 - .NET Denial of Service Vulnerability.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|NServiceBus.Transport.PostgreSql|9.1.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.PostgreSql/9.1.1)|
|NServiceBus.Transport.SqlServer|9.1.1|[NuGet](https://www.nuget.org/packages/NServiceBus.Transport.SqlServer/9.1.1)|
|ServiceControl|6.21.3|[The Download Page](https://particular.net/downloads)|
|Particular.ServiceControl.Management|6.21.3|`Update-Module -Name Particular.ServiceControl.Management -RequiredVersion 6.21.3`|
|servicecontrol|6.21.3|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol) or `docker pull particular/servicecontrol:6.21.3`|
|servicecontrol-audit|6.21.3|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-audit) or `docker pull particular/servicecontrol-audit:6.21.3`|
|servicecontrol-monitoring|6.21.3|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-monitoring) or `docker pull particular/servicecontrol-monitoring:6.21.3`|
|servicecontrol-ravendb|6.21.3|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-ravendb) or `docker pull particular/servicecontrol-ravendb:6.21.3`|


### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
