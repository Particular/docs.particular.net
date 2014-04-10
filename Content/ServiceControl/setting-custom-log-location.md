---
title: Configuring ServiceControl Log Location
summary: How to configure ServiceControl to store logs in a different location
tags:
- ServiceControl
- Log
- Configuration
---
You can change the path for the ServiceControl logs, to update the logs path run the following command:

```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d="ServiceControl/LogPath==x:\new\log\location"
```

<p class="alert alert-info">
<strong>ServiceControl default installed location</strong><br/>
By default ServiceControl is installed in C:\Program Files (x86)\Particular Software\ServiceControl for 64bit OS or C:\Program Files\Particular Software\ServiceControl for 32bit OS.
</p>
