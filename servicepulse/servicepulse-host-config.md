---
title: Configuring ServicePulse Host
summary: Describes how to configure the ServicePulse Host
tags:
- ServicePulse
---

To modify the port used by ServicePulse the commandline specified in the registry must be updated.  

To change it:

1. Open Regedit.exe
-  Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
-  Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a commandline of the URL to use:  

The default value for ImagePath is:

`"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"`

Change the value after `--url=` and restart the service.


## Changing the ServiceControl URL

ServicePulse is configured to connect to the ServiceControl REST API.  To specify the URL to connect do  the following

- Go to the installation folder for ServicePulse.  Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the `app` folder and edit `config.js`
- Change the value of the url after `service_control_url`
