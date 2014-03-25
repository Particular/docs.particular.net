---
title: Configuring ServiceControl log location
summary: How to configure ServiceControl to store logs in a differnt location
tags:
- ServiceControl
- Log
- Configuration
---
It is possible to change the location on disk where ServiceControl stores its log information, the steps required to change the log location are the followings:

* Stop the ServiceControl service;
* Locate di `ServiceControl.dll.config` configuration file located in the ServiceControl installation folder;
* Edit the configuration file adding the following setting:

	```xml
	<add key="ServiceControl/LogPath" value="x:\new\log\location" />
	```
* Start the ServiceControl service;