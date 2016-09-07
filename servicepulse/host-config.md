---
title: 'ServicePulse: Configuring Hosting'
summary: Describes how to configure the ServicePulse Host
reviewed: 2016-09-02
component: ServicePulse
tags:
 - ServicePulse
redirects:
 - servicepulse/servicepulse-host-config
---

To modify the port used by ServicePulse the command line specified in the registry must be updated.

To change it:

 1. Open Regedit.exe
 1. Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
 1. Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a command line of the URL to use:

The default value for ImagePath is:

```dos
"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"
```

Change the value after `--url=` and restart the service.


## Changing the ServiceControl URL

ServicePulse is configured to connect to the ServiceControl REST API. To specify the URL to connect do  the following

Note: Ensure Version 1.3 or above is being used.

 * Go to the installation directory for ServicePulse. Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
 * Go to the `app\js` directory and edit `app.constants.js`
 * If there are any `*.js` in the `app` directory delete them
 * Change the value of the url after `service_control_url`