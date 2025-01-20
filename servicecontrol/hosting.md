---
title: ServiceControl Hosting Options
summary: Hosting options for running ServiceControl instances
component: ServiceControl
reviewed: 2024-12-16
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

- Commandline: ServiceControl Powershell Module
- Management UI: ServiceControl Management Utility

### Supported editions

- Datacenter
- Standard

### Supported versions

The supported Windows Server versions are aligned with the [Microsoft Mainstream support end date for Windows Server](https://learn.microsoft.com/en-us/windows-server/get-started/windows-server-release-info).

## Containers

Various containerized hosting options are available:

- Docker
- Kubernetes

### Supported architectures

- `linux/arm64`
- `linux/amd64`

### Supported environments

The following environments are getting production supports

- Azure Container Apps
- Azure Managed Kubernetes Service (AKS) 
- Amazon Elastic Kubernetes Service (EKS)
- Amazon Elastic Container Service (ECS)
- Docker Engine on Windows 2022


### Known working environments

Docker and Kubernetes are available in many configurations and environments. Many will "just work" but are unsupported for production deployments.

The following additional environments are known to work:

- Docker Desktop for Windows
- Docker Desktop for Mac
- Docker on Fedora
- Podman on Fedora Workstation 41
