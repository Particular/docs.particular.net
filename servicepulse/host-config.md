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

It may be necessary to reserve the new URL for the account being used to run ServicePulse. For example, to reserve port 9090 for all users:
```dos
netsh http add urlacl url=http://+:9090/ user=Everyone
```

ServicePulse runs as a Windows Service.

## Default connections to ServiceControl and ServiceControl Monitoring

ServicePulse will, by default, attempt to connect to ServiceControl and ServiceControl Monitoring using the URLs `http://localhost:33333/api/` and `http://localhost:33633/`, respectively.

This configuration is stored in a Javascript file located in the ServicePulse isntallation folder. By default, this is `%ProgramFiles(x86)%\Particular Software\ServicePulse\app\js\app.constants.js`.

## Configuring connections via the ServicePulse UI

The connections to ServiceControl and ServiceControl Monitoring can be changed using the connections tab in the configuration screen in ServicePulse:

![Connections configuration](images/connections-configuration.png 'width=500')

Full URLs must be specified, including scheme and port number. The URLs are persisted in local storage and are also embedded in the query string of ServicePulse URL in the browser address bar, for bookmarking and sharing.

ServicePulse uses connection URLs in the following order:

1. The query string of the ServicePulse URL.
2. Local storage.
3. Default values.

In order to use the query string to specify a ServiceControl instance that ServicePulse should connect to, the browser URL should be of the format:

```http://localhost:9090/?scu=http://qacontrol:33333/api/&mu=http://qamonitoring:33633/#/```

The query string parameter for `scu=` is the URL representation of the ServiceControl API url, and the query string paramater `mu=` is the URL of the ServiceControl Monitoring server. In the example above, ServicePulse is connecting to the ServiceControl instance at `http://qacontrol:33333/api/` and connecting to a ServiceControl Monitoring instance at `http://qamonitoring:33633/`. By providing these connection details in the URL, a browser bookmark or desktop shortcut can be created to instantly open ServicePulse with those connection details or easily share them with other people.
