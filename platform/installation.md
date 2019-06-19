---
title: Platform Installation
summary: Guidance on the different ways to install and use the platform
reviewed: 2019-06-15
---

The Particular Service Platform can be used in different ways.

- Monitoring production systems
- Debugging during development
- Presenting from within Visual Studio

## Monitoring production systems

The Platform Installers requires internet connectivity and performs several actions like verifying MSMQ and MSDTC installation. It is possible to [achieve the same result](https://docs.particular.net/platform/installer/offline) without requiring internet connectivity.

For usage on production environments it is possible and recommended to install the different components individually. Each component has individual installation instructions.

### ServiceControl

The latest download is [available on GitHub here](https://github.com/Particular/servicecontrol/releases/latest).

After installation, the ServiceControl Management application can configure one or multiple instances for [ServiceControl](/servicecontrol/installation) and [ServiceControl Monitoring](/servicecontrol/monitoring-instances/installation/).

### ServiceControl.Monitoring

The latest download is [available on GitHub here](https://github.com/Particular/ServiceControl.Monitoring/releases/latest).

As with ServiceControl the Management application can be used to configure one or multiple instances.

### ServicePulse

The latest download is [available on GitHub here](https://github.com/Particular/servicepulse/releases/latest).

ServicePulse connects to a single instance of ServiceControl. There are different ways of connecting to different ServiceControl installations.

#### Different browser connections

By default ServicePulse connects to the [default ports](/servicepulse/host-config.md#default-connection-to-servicecontrol-and-servicecontrol-monitoring) for ServiceControl. This can be modified via [configuration](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) inside ServicePulse, which will alter the querystring. These unique uri can then be used as bookmarks.

#### Multiple ServicePulse instances

As ServicePulse runs as a Windows Service, it is possible to install the service multiple times and use different ports to host it. There are [installation instructions](/servicepulse/installation.md#installation-available-installation-parameters) to achieve this using installation parameters.

### ServiceInsight

The latest download is [available on GitHub here](https://github.com/Particular/serviceinsight/releases/latest).

ServiceInsight has the ability to connect to multiple ServiceControl instances using the [Endpoint Explorer](/serviceinsight/#endpoint-explorer).

## Debugging during development

For optimal installation of the Platform on a development machine the [Platform Installer](/platform/installer/) can be used.

## Presenting from within Visual Studio

The Service Platform has the ability to be hosted from within Visual Studio for the benefit of not having to install anything. This can be useful during presentations, to provide samples to others or to demonstrate the usage of the platform from within an existing solution. For this feature the Platform Package has been developed.

There are additional [installation instructions](platform-sample) for the Platform Package.
