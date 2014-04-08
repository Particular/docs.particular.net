---
title: Customizing ServiceControl configuration
summary: How ServiceControl manages configuration and how to create and customize the ServiceControl configuration file.
tags:
- ServiceControl
- Configuration
---

ServiceControl allows you to create a configuration file that contains settings that override default settings.

When first installing ServiceControl, it will be set to automatically start (as a Windows Service) using its internal default settings (these include, for example, the default `localhost` hostname and `33333` port number, embedded database location etc.).

To override these default settings, perform the following:

* Stop the ServiceControl service.
* Locate/Create a configuration file named `ServiceControl.exe.config` in the ServiceControl installation folder.
* Edit the configuration file and add the relevant settings to the `<appSettings>` section
* Start the ServiceControl service.
 
###Sample configuration file
 
File name: `servicecontrol.exe.config`
File Path: `C:\Program Files (x86)\Particular Software\ServiceControl` (default ServiceControl installation folder)
 
 
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
