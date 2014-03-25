---
title: How to directly access ServiceControl data via RavenDB studio
summary: Explains how to condigure ServiceControl to allow the direct access to the Embedded RavenDB instance
tags:
- ServiceControl
- Configuration
- RavenDB
---
ServiceControl stores its data in a RavenDB Embedded instance, by default the RavenDB instance is accessible only by the ServiceControl service, if for any reason, it is required to directly access the RavenDB instance ServiceControl can be configured to expose the RavenDB studio.

To enable ravenDB direct access there are 2 options:

* Modify the ServiceControl configration file locate din the installation directory adding the following setting:

	`<add key="ServiceControl/ExposeRavenDB" value="true" />`
	
* Edit the Registry adding the following Key:
 
	```
	[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
	"ExposeRavenDB"="true"
	```

After restarting te ServiceControl service the RavenDB studio can be accessed locally at the following endoint:

`http://localhost:33333/storage`

*** note: The RavenDB studio can be accessed from localhost regardless of hostname setting ***