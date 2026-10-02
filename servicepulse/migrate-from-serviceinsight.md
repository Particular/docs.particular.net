---
title: Migrate from ServiceInsight to ServicePulse
summary: A guide for migrating to ServicePulse from ServiceInsight
component: ServicePulse
reviewed: 2026-09-29
related:
- serviceinsight
---

ServiceInsight has been sunset and will receive no further updates. It will be deprecated on February 10, 2027, at which time it will no longer be supported. 

ServicePulse is the recommended replacement for visualization and debugging capabilities. 

## Migrating

### Prerequisites

- A running instance of [ServiceControl](/servicecontrol). As both ServiceInsight and ServicePulse require this, any system already using ServiceInsight meets this requirement.
- ServicePulse hosted using any of the following ways:
  - Integrated ServicePulse (ServiceControl 6.13 or later). [Integrated ServicePulse](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) runs inside the ServiceControl Error instance and is automatically configured to connect to it, so no separate installation is needed. It is also upgraded automatically whenever ServiceControl is upgraded.
  - Docker for running ServicePulse in a container. This is best suited for cross-platform environments or monitoring multiple systems. 
  - A Windows machine with .NET Framework 4.5 or later to install ServicePulse directly. Only one instance can be installed at a time, so if there are multiple systems to monitor then multiple distinct [URL connection configurations will need to be used](/servicepulse/host-config.md#configuring-connections-via-servicepulse-url-query-string-parameters).
- A currently supported version of Microsoft Edge, Chrome, Firefox, or Safari.

### Migration steps

> [!NOTE]
> ServicePulse does not require any data migration from ServiceInsight. Both tools read data directly from ServiceControl, so the same message information will be available in ServicePulse.

1. Open ServiceInsight and take note of the connection urls.
  ![ServiceInsight connection urls](images/si-migration-si-connections.png 'width=400')

2. Get ServicePulse running and configured with the connection URL [using containers](/servicepulse/containerization/) (recommended) or by [installing to a Windows machine](/servicepulse/installation.md).

    Each method provides a way to configure the URL during setup, but it can also be configured in the ServicePulse UI afterwards:

    ![ServicePulse connection settings](images/si-migration-sp-connections.png 'width=800')

    If [monitoring](/servicepulse/how-to-configure-endpoints-for-monitoring.md) is enabled on your endpoints system, the ServiceControl monitoring url can be configured at this time to allow ServicePulse to display monitoring information.

  > [!NOTE]
  > If there were multiple urls configured in ServiceInsight, a separate container or URL is needed for each.  See the [known limitations](#known-limitations-connection-to-multiple-servicecontrol-instances).


 3. Optionally, uninstall ServiceInsight. It will continue to function as long as the ServiceControl api remains the same, but it will no longer receive updates and support will end when it is deprecated.

## Known limitations

### Connection to multiple ServiceControl instances

ServicePulse currently connects to one ServiceControl instance at a time. To work around this, a separate container can be run for each system that needs to be monitored, or [separate URLs](/servicepulse/host-config.md#configuring-connections-via-servicepulse-url-query-string-parameters) can be bookmarked for each primary and monitoring connection configuration.

### Custom message viewer plugins

There is currently no equivalent to the [custom message viewer plugin model](/serviceinsight/custom-message-viewers.md) used by ServiceInsight; however, a [feature request has been created](https://github.com/Particular/ServicePulse/issues/2778) so leave a comment if you need that functionality.
