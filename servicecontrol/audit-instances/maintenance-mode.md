---
title: ServiceControl Audit maintenance mode
summary: How to get direct access to the embedded RavenDB instance.
reviewed: 2019-11-27
---

ServiceControl Audit stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl Audit service. If direct access to the RavenDB instance for troubleshooting is required, run the instance in Maintenance Mode by launching ServiceControl Management and follow these steps:

1. Open Advanced Options
![](../managementutil-advancedoptions.png)
1. Start Maintenance Mode
![](../managementutil-maintenancemode.png 'width=500')
1. Launch RavenDB Studio
![](../managementutil-launchstudio.png 'width=500')
1. Stop Maintenance Mode once work completed

WARNING: The ServiceControl Audit RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.