---
title: Platform Installation
summary: Guidance on the different ways to install and use the platform
reviewed: 2020-08-17
---

The Particular Service Platform may be installed on either a server, for use in production environments, or a workstation, during development. A portable version is also available, for use in samples or demonstrations.

Note that, for the purposes of this document, shared testing environments may be treated in the same way as production environments.

## Installation

Platform components are installed separately using dedicated [installation packages](https://particular.net/downloads).

### ServiceControl

After installation, the ServiceControl Management application is used to create one or more instances of [ServiceControl](/servicecontrol/installation.md) and [ServiceControl Monitoring](/servicecontrol/monitoring-instances/installation/).

At any one time, a single instance of ServicePulse or ServiceInsight can connect to a single instance of ServiceControl. For multiple instances of ServiceControl, there are various options:

### ServicePulse

#### Switch between ServiceControl instances

This can be done via the [ServicePulse UI](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui).

#### Run multiple instances of ServicePulse

ServicePulse runs as a Windows Service and can be installed more than once on a single machine, with each instance listening on it's own port. This is done by [specifying appropriate arguments during installation](/servicepulse/installation.md#installation-available-installation-parameters).

### ServiceInsight

The instance of ServiceControl can be chosen in the [Endpoint Explorer](/serviceinsight/#endpoint-explorer).

## Portable version

The Platform can be hosted in Visual Studio, with no installation required, using the [Platform Sample package](/platform/platform-sample-package.md). This is useful for presentations, for providing samples, or to demonstrate usage of the Platform within an existing solution.
