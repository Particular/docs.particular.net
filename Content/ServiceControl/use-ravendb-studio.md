---
title: Exposing Data via RavenDB Studio
summary: How to configure ServiceControl to allow direct access to the embedded RavenDB instance.
tags:
- ServiceControl
- Configuration
- RavenDB
---
ServiceControl stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If direct access to the RavenDB instance is required, ServiceControl can be configured to expose the RavenDB studio.

NOTE: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications. 

There are two ways to enable direct access to RavenDB:

* Open the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file)) add the following setting:

```
<add key="ServiceControl/ExposeRavenDB" value="true" />
```
	
* Edit the registry by adding the following key:
 
```
[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
"ExposeRavenDB"="true"
```

After restarting the ServiceControl service, you can access the RavenDB studio locally at the following endpoint:

    http://localhost:33333/storage

NOTE: The ServiceControl embedded RavenDB studio can be accessed from localhost regardless of the hostname customization setting.
