---
title: Configuring ServicePulse Host
summary: Describes how to configure the ServicePulse Host
tags:
- ServicePulse
- Configuration
---

### Changing ServicePulse port

When ServicePulse is installed as a Windows service it the URL it listens on is added as a command line parameter.

To change it:

1. Open Regedit.exe
-  Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
-  Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a commandline of the URL to use:  

The default value for ImagePath is:

`"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"`

Change the value of after --url= and restart the service.

### Changing the ServiceControl URL

ServicePulse is configured to connect to the ServiceControl REST API.  To specify the URL to connect do  the following

- Go to the installation folder for ServicePulse.  Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the app sub folder and edit config.js
- Change the value of the url service_control_url

In the example below the URL has ben changed to connect to a ServiceControl service listening on http://testserver:33333/api/

```js
'use strict';

var SC = SC || {};

SC.config = {
    service_control_url: 'http://testserver:33333/api/',
    service_pulse_url: 'http://platformupdate.particular.net/servicepulse.txt'

};
```
