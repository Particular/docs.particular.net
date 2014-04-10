---
title: Customizing ServiceControl Configuration
summary: How ServiceControl manages configuration and how to create and customize the ServiceControl configuration file.
tags:
- ServiceControl
- Configuration
---

ServiceControl allows you to create a configuration file with settings that override default settings.

When you first install ServiceControl, it is set to automatically start (as a Windows Service) using its internal default settings. Examples: the default `localhost` hostname and `33333` port number, and the embedded database location.

To override these default settings:

1. Stop the ServiceControl service.
1. Locate/create a configuration file named `ServiceControl.exe.config` in the ServiceControl installation folder.
1. Edit the configuration file and add the relevant settings to the `<appSettings>` section.
1. Start the ServiceControl service.
 
###Sample configuration file
 
* File name: `servicecontrol.exe.config`
* File path: `C:\Program Files (x86)\Particular Software\ServiceControl` (default ServiceControl installation folder)
 
 
```xml
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<configuration>
  <appSettings>
      <add key="ServiceControl/Hostname" value="my.domain-name.com" />
      <add key="ServiceControl/Port" value="33333" />
      <add key="ServiceControl/DbPath" value="x:\mydb" />
      ...
  </appSettings>
</configuration>
```
