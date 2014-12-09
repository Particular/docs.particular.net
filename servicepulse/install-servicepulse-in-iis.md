title: Install ServicePulse in IIS
summary: Describes how to manually install ServicePulse in IIS.
tags: [ ServicePulse ]
---

[ServicePulse](introduction-and-installing-servicepulse), by default, is installed as a Windows Service that will self-host the ServicePulse web application.

It is possible to manually install ServicePulse using IIS following these steps:

* Extract ServicePulse files using, at a command prompt, the following command:
```
ServicePulse.Host.exe` --extract--serviceControlUrl="http://localhost:8080/api" --outPath="C:\temp\SP
```

Note: ServicePulse.Host.exe can be found in the ServicePulse installation directory, whose default is `%programfiles(x86)%\Particular Software\ServicePulse`

Once all the ServicePulse files are successfully extracted you can configure a new IIS web site whose physical path points to the location where files have been extracted.