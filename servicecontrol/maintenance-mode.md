---
title: ServiceControl maintenance mode
summary: How to get direct access to the embedded RavenDB instance.
reviewed: 2021-03-12
redirects:
- servicecontrol/use-ravendb-studio
---

NOTE: Requires a currently-supported version of Edge, Chrome, Firefox, or Safari. **Internet Explorer is unsupported!**

ServiceControl stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If direct access to the RavenDB instance for troubleshooting is required, run the instance in Maintenance Mode by launching ServiceControl Management and follow these steps:

1. Open Advanced Options
![ServiceControl Management Utility - Advanced options](managementutil-advancedoptions.png)
1. Start Maintenance Mode
![ServiceControl Management Utility - maintenance mode](managementutil-maintenancemode.png 'width=500')
1. Launch RavenDB Studio
![ServiceControl Management Utility - Luanch ravendb studio](managementutil-launchstudio.png 'width=500')
1. Stop Maintenance Mode once work completed

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.
