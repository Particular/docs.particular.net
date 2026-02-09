---
title: Migrate from ServiceInsight to ServicePulse
summary: A guide for migrating to ServicePulse from ServiceInsight
component: ServicePulse
reviewed: 2025-02-19
related:
- serviceinsight
---

ServiceInsight has been sunset and will receive no further updates.  It will be deprecated on February 10th, 2027, at which time it will no longer be supported. 

ServicePulse is the recommended replacement for visualization and debugging capabilities. 

## Migrating

### Prerequisites

- A running instance of [ServiceControl](/servicecontrol). Both ServiceInsight and ServicePulse require this, so if you're reading this because you've been using ServiceInsight, this requirement is met.
- Docker for running ServicePulse in a container is recommended, because it is cross-platform and supports monitoring multiple systems. 
  - Alternatively, a Windows machine with .NET Framework 4.5 or later to install ServicePulse directly. Only one instance can be installed at a time, so if there are multiple systems to monitor then multiple distinct [URL connection configurations will need to be used](/servicepulse/host-config#configuring-connections-via-servicepulse-url-query-string-parameters).
- A modern web browser (Microsoft Edge, Chrome, Firefox, or Safari)

### Migration steps

> [!NOTE]
> ServicePulse does not require any data migration from ServiceInsight. Both tools read data directly from ServiceControl, so the same message information will be available in ServicePulse.

1. Open ServiceInsight and take note of the connection urls.
  ![ServiceInsight connection urls](images/si-migration-si-connections.png 'width=400')

2. Get ServicePulse running and configured with the connection URL [using containers](/servicepulse/containerization/) (recommended) or by [installing to a Windows machine](/servicepulse/installation.md).

    Each method provides a way to configure the URL during setup, but it can also be configured in the ServicePulse UI afterwards:

    ![ServiceInsight connection urls](images/si-migration-sp-connections.png 'width=800')

    If [monitoring](/servicepulse/how-to-configure-endpoints-for-monitoring.md) is enabled on your endpoints system, the ServiceControl monitoring url can be configured at this time to allow ServicePulse to display monitoring information.

  > [!NOTE]
  > If there were multiple urls configured in ServiceInsight, a separate container or URL is needed for each.  See the [known limitations](#known-limitations-connection-to-multiple-servicecontrol-instances).


Uninstalling ServiceInsight is optional. It will continue to function as long as the ServiceControl api remains the same, but it will no longer receive updates and support will end when it is deprecated.

## Known limitations

### Connection to multiple ServiceControl instances

ServicePulse currently connects to one ServiceControl instance at a time. To work around this, a separate container can be run for each system that needs to be monitored, or [separate URLs](https://docs.particular.net/servicepulse/host-config#configuring-connections-via-servicepulse-url-query-string-parameters) can be bookmarked for each primary and monitoring connection configuration.

### Custom message viewer plugins

There is currently no equivalent to the [custom message viewer plugin model](/serviceinsight/custom-message-viewers.md) used by ServiceInsight; however, a [feature request has been created](https://github.com/Particular/ServicePulse/issues/2778) so leave a comment if you need that functionality.
