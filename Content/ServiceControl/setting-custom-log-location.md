---
title: Configuring ServiceControl Log Location
summary: How to configure ServiceControl to store logs in a different location
tags:
- ServiceControl
- Log
- Configuration
---
You can change the location on disk where ServiceControl stores its log information, as follows:

1. Stop the ServiceControl service.
1. Locate the `ServiceControl.dll.config` configuration file located in the ServiceControl installation folder.
1. Edit the configuration file, adding the following setting:

	`xml
	<add key="ServiceControl/LogPath" value="x:\new\log\location" />
	`
1. Start the ServiceControl service.
