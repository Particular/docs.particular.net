---
title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS.
tags:
- ServicePulse
---

[ServicePulse](introduction-and-installing-servicepulse.md), by default, is installed as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

* Extract ServicePulse files using, at a command prompt, the following command:
```
ServicePulse.Host.exe --extract --serviceControlUrl="http://localhost:8080/api" --outputPath="C:\temp\SP"
```

Note: ServicePulse.Host.exe can be found in the ServicePulse installation directory, whose default is `%programfiles(x86)%\Particular Software\ServicePulse`

Once all the ServicePulse files are successfully extracted you can configure a new IIS web site whose physical path points to the location where files have been extracted.

### Changing ServicePulse port

The port is set in the registry:
 
    HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse\imagepath 

To modify it, replace `:9090` to the desired port and then restart service.

NOTE: If you upgrade ServicePulse you need to check this entry was not overwritten by the installer.
