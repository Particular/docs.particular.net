---
title: Platform Installation
summary: Guidance on the different ways to install and use the platform
reviewed: 2019-06-15
---

The Particular Service Platform can either be installed to a server for use with production systems, or on a workstation during development. In addition, a portable version is available for use in samples or demonstrations.

## Monitoring production systems

The Platform Installer requires Internet connectivity. It is possible to [achieve the same result](/platform/installer/offline.md) without requiring internet connectivity.

For usage on production environments it is possible and recommended to install the different components individually. More details can be found on the [download page](https://particular.net/downloads).

### ServiceControl

After installation, the ServiceControl Management application can configure one or multiple instances for [ServiceControl](/servicecontrol/installation.md) and [ServiceControl Monitoring](/servicecontrol/monitoring-instances/installation/).

### ServicePulse

ServicePulse connects to a single instance of ServiceControl. There are different ways of connecting to different ServiceControl installations.

#### Different browser connections

By default ServicePulse connects to the [default ports](/servicepulse/host-config.md#default-connection-to-servicecontrol-and-servicecontrol-monitoring) for ServiceControl. This can be modified via [configuration](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) inside ServicePulse, which will alter the querystring. These unique URLs can then be used as bookmarks.

#### Multiple ServicePulse instances

As ServicePulse runs as a Windows Service, it is possible to install the service multiple times and use different ports to host it. There are [installation instructions](/servicepulse/installation.md#installation-available-installation-parameters) to achieve this using installation parameters.

### ServiceInsight

ServiceInsight has the ability to connect to multiple ServiceControl instances using the [Endpoint Explorer](/serviceinsight/#endpoint-explorer).

## Debugging during development

For optimal installation of the Platform on a development machine the [Platform Installer](/platform/installer/) can be used.

## Portabe mode

The Service Platform has the ability to be hosted from within Visual Studio for the benefit of not having to install anything. This can be useful during presentations, to provide samples to others or to demonstrate the usage of the platform from within an existing solution. For this feature the Platform Package has been developed.

For more information, see [Platform Sample Installation](/platform/platform-sample-package.md).
