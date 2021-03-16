---
title: ServiceControl maintenance mode
summary: How to get direct access to the embedded RavenDB instance.
reviewed: 2021-03-16
redirects:
- servicecontrol/use-ravendb-studio
---

NOTE: Requires a currently-supported version of Edge, Chrome, Firefox, or Safari. **Internet Explorer is unsupported!**

ServiceControl stores data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If direct access to the RavenDB instance is required for troubleshooting, launch ServiceControl Management and follow these steps:

1. Open Advanced Options
![ServiceControl Management Utility - Advanced Options](managementutil-advancedoptions.png)
1. Start Maintenance Mode
![ServiceControl Management Utility - Maintenance Mode](managementutil-maintenancemode.png 'width=500')
1. Launch RavenDB Management Studio
![ServiceControl Management Utility - launch RavenDB Management Studio](managementutil-launchstudio.png 'width=500')
1. Stop Maintenance Mode as soon as access to the embedded RavenDB instance is no longer required.

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.
