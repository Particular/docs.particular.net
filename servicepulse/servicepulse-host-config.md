---
title: Configuring ServicePulse Host
summary: Describes how to configure the ServicePulse Host
tags:
- ServicePulse
---

To modify the port used by ServicePulse the commandline specified in the registry must be updated.

To change it:

1. Open Regedit.exe
2. Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
3. Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a commandline of the URL to use:

The default value for ImagePath is:

`"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"`

Change the value after `--url=` and restart the service.


## Changing the ServiceControl URL

ServicePulse is configured to connect to the ServiceControl REST API. To specify the URL to connect do  the following

### For Versions before 1.3.0

- Go to the installation folder for ServicePulse. Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the `app` folder and edit `config.js`
- Change the value of the url after `service_control_url`

### For Versions 1.3.0 and after

- Go to the installation folder for ServicePulse. Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the `app\js` folder and edit `app.constants.js`
- If there are any `*.js` in the `app` folder delete them
- Change the value of the url after `service_control_url`
