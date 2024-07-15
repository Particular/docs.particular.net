---
title: Migrate ServiceControl to container deployment
summary: Instructions on how to migrate ServiceControl instances from Windows to container-based hosting.
reviewed: 2024-07-15
isUpgradeGuide: true
component: ServiceControl
related:
---

ServiceControl 5.3.0 adds the ability to host ServiceControl instances in Linux containers. This article describes how to migrate from ServiceControl hosted on Windows VMs to new ServiceControl instances hosted in containerized infrastructure.

## Networking / access

TODO

Diagram?

## Reverse proxy

TODO

## Database containers

When hosted on Windows, the ServiceControl instance (or Error instance) instance and ServiceControl Audit instances store data in embedded databases that are launched as part of each instance's service process.

When hosted in containers, the Error instance and each Audit instance (if more than one are in use) use separate database containers.

> [!CAUTION]
> A single database container should not be shared between multiple ServiceControl instances in production scenarios.

As part of setting up a container for an Error or Audit instance, [deploy a database container](/servicecontrol/ravendb/deployment/containers.md) for the instance and take note of its URL.

## Migrating instance types

Migration for ServiceControl instances is different for each instance type.

### Migrating Error instances

### Migrating Audit instances

### Migrating Monitoring instances

Monitoring instances do not permanently store any data. As a result, a [new Monitoring instance can be deployed using containers](/servicecontrol/monitoring-instances/deployment/containers.md) at any time, while the old instance is stopped and removed.