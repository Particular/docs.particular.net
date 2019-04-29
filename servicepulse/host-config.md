---
title: Configuring Hosting
summary: Describes how to configure the ServicePulse host and connections
reviewed: 2018-04-24
component: ServicePulse
redirects:
 - servicepulse/servicepulse-host-config
---

To modify the port used by ServicePulse, the command line specified in the registry must be updated.

To change it:

 1. Open Regedit.exe
 1. Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
 1. Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a command line of the URL to use:

The default value for ImagePath is:

```dos
"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"
```

Change the value after `--url=` and restart the service.


## Default connection to ServiceControl and ServiceControl Monitoring

ServicePulse will by default, attempt to connect to ServiceControl and ServiceControl Monitoring using the URLs `http://localhost:33333/api/` and `http://localhost:33633/`, respectively.

## Configuring connections via the ServicePulse UI

The default connections can easily be overriden via the Connections configuration screen in ServicePulse:

![Connections configuration](images/connections-configuration.png 'width=500')

When entering the connection URL to ServiceControl and ServiceControl Monitoring, the full URLs including the port number and scheme needs to be entered in the Connections screen. Connection configuration is persisted only locally and is read in the following order:

1. **Query string in browser address bar URL** - allows easily bookmarking and sharing of connection details.
2. **Local storage** - if the connection parameters are not found in the URL, the last successful connection will be used.
