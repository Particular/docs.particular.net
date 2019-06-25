---
title: Configuring Hosting
summary: Describes how to configure the ServicePulse host and connections
reviewed: 2019-06-17
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

As ServicePulse is installed as a Windows Servce, optionally multiple ServicePulse instances can be installed. The default ports can be changed by providing [installation parameters](/servicepulse/installation.md#installation-available-installation-parameters).

## Default connection to ServiceControl and ServiceControl Monitoring

ServicePulse will by default, attempt to connect to ServiceControl and ServiceControl Monitoring using the URLs `http://localhost:33333/api/` and `http://localhost:33633/`, respectively.

The configuration is stored inside a javascript file located in the folder ServicePulse is installed in. By default this is `C:\Program Files (x86)\Particular Software\ServicePulse\app\js\app.constants.js`.

## Configuring connections via the ServicePulse UI

The default connections can easily be overriden via the Connections configuration screen in ServicePulse:

![Connections configuration](images/connections-configuration.png 'width=500')

When entering the connection URL to ServiceControl and ServiceControl Monitoring, the full URLs including the port number and scheme needs to be entered in the Connections screen. Connection configuration is persisted only locally and is read in the following order:

1. **Query string in browser address bar URL** - allows easily bookmarking and sharing of connection details.
2. **Local storage** - if the connection parameters are not found in the URL, the last successful connection will be used.

In order to use the query string to specify a ServiceControl instance that ServicePulse should connect to, the browser URL should be of the format:

```http://localhost:9090/?scu=http%3A%2F%2Fqacontrol%3A33333%2Fapi%2F&mu=http%3A%2F%2Fqamonitoring%3A33633%2F#/```

The querystring parameter for `scu=` is a URL Encoded representation of the ServiceControl API url, and the querystring paramater `mu=` is a URL encoded representation of the ServiceControl Monitoring server. In the example above, ServicePulse is connecting to the ServiceControl at `http://qacontrol:33333/api/` and connecting to a ServiceControl Monitoring instance at `http://qamonitoring:33633/`. If the example URL is bookmarked - or a shortcut created - then when that bookmark is openened ServicePulse will connect to that environment.
