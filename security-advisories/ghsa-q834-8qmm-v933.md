---
title: "GHSA-q834-8qmm-v933"
summary: "OpenTelemetry dotnet: OTLP exporter reads unbounded HTTP response bodies"
reviewed: "2026-05-14"
---

## Security Advisory Id GHSA-q834-8qmm-v933

This advisory discloses a security vulnerability 
Patches for components to update their dependencies to avoid references that have the [GHSA-q834-8qmm-v933](https://github.com/advisories/ghsa-q834-8qmm-v933) security advisory: OpenTelemetry dotnet: OTLP exporter reads unbounded HTTP response bodies.

### Patch releases

| Component | Version | Where to get it |
| --------- | ------- | --------------- |
|ServiceControl|6.14.2|[The downloads page](https://particular.net/downloads)|
|Particular.ServiceControl.Management|6.14.2|Update-Module -Name Particular.ServiceControl.Management -RequiredVersion 6.14.2|
|servicecontrol|6.14.2|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol) or `docker pull particular/servicecontrol:6.14.2`|
|servicecontrol-audit|6.14.2|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-audit) or `docker pull particular/servicecontrol-audit:6.14.2`|
|servicecontrol-monitoring|6.14.2|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-monitoring) or `docker pull particular/servicecontrol-monitoring:6.14.2`|
|servicecontrol-ravendb|6.14.2|[Docker Hub](https://hub.docker.com/r/particular/servicecontrol-ravendb) or `docker pull particular/servicecontrol-ravendb:6.14.2`|

### How to know if you are affected

You are affected if you are using previous versions of any of these components, but this doesn't necessarily mean you are vulnerable.

### Symptoms

For NuGet packages your projects have the setting `NuGetAuditMode` set to `all` and see transitive dependency warnings at build time that mention Particular packages.

Other components of the platform will not have any symptoms.

### When to upgrade

You should upgrade immediately if you are affected. Otherwise, you should upgrade during your next maintenance window.
