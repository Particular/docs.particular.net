---
title: ServiceControl Hosting Options
summary: Hosting options for running ServiceControl instances
component: ServiceControl
reviewed: 2025-03-20
---

ServiceControl instances can be hosted on:

- Windows Server (virtual machines)
- Linux containers

Deployment options:

- [Monitoring instances](/servicecontrol/monitoring-instances/deployment/)
- [Audit instances](/servicecontrol/audit-instances/deployment/)
- [Error instances](/servicecontrol/servicecontrol-instances/deployment/)

## Windows Server

Instances can be installed on Windows Server (virtual) machines using:

- Terminal: ServiceControl Powershell Module
- Management UI: ServiceControl Management Utility

### Supported editions

- Datacenter
- Standard

### Supported versions

The supported Windows Server versions are aligned with the [Microsoft Mainstream support end date for Windows Server](https://learn.microsoft.com/en-us/windows-server/get-started/windows-server-release-info).

## Containers

That Particular Platform images are OCI compliant and can be used in various environments:

- Docker
- Kubernetes
- Podman

### Supported architectures

- `linux/arm64`
- `linux/amd64`
