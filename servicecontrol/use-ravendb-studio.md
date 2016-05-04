---
title: Exposing Data via RavenDB Studio
summary: How to configure ServiceControl to allow direct access to the embedded RavenDB instance.
tags:
- ServiceControl
- RavenDB
---

ServiceControl stores its data in a RavenDB embedded instance. By default, the RavenDB instance is accessible only by the ServiceControl service. If direct access to the RavenDB instance is required, ServiceControl can be configured to expose the RavenDB studio.

WARNING: The ServiceControl RavenDB embedded instance is used exclusively by ServiceControl and is not intended for external manipulation or modifications.

There are two ways to enable direct access to RavenDB:

* Open the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md)) add the following setting:

```
<add key="ServiceControl/ExposeRavenDB" value="true" />
```
	
* Edit the registry and add a new String Value called `ExposeRavenDB` to the ServiceControl key as shown below:

```
[HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl]
"ExposeRavenDB"="true"
```

After restarting the ServiceControl service, access the RavenDB studio locally at the following endpoint:

    http://localhost:33333/storage

NOTE: The ServiceControl embedded RavenDB studio can be accessed from localhost regardless of the hostname customization setting. To allow external access the hostname must be [set to a fully qualified domain name](setting-custom-hostname.md).


### Troubleshooting

If ServiceControl is configured to use a service account other than `localsystem` it may be necessary to manually add a URLACL.
Refer to [TroubleShooting](troubleshooting.md#unable-to-start-service-after-exposing-ravendb) 
