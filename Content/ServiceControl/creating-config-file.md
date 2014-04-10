---
title: Customizing ServiceControl configuration
summary: How ServiceControl manages configuration and how to override ServiceControl default configuration settings.
tags:
- ServiceControl
- Configuration
---

ServiceControl allows you to override its default configration settings.

When first installing ServiceControl, it will be set to automatically start (as a Windows Service) using its internal default settings (these include, for example, the default `localhost` hostname and `33333` port number, embedded database location, etc).

These default settings can be overriden by using `ServiceControl.exe` with `-d=setting_name==setting_value` cmd line argument. For a list of all cmd line options run `ServiceControl.exe -?`.


