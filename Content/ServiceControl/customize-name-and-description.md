---
title: Setting Custom Name and Description properties for ServiceControl API
summary: How to configure the default values for the Name and Description properties of the ServiceControl API
tags:
- ServiceControl
- Configuration
---
The ServiceControl http API exposes 2 properties that describe the ServiceControl installation:

* Name: by default the name of the machine where ServiceControl is installed;
* Description: a description of the ServiceControl service;

These 2 properties can be retrieved issuing an http call to `http://localhost:33333/API/` that is the default http endpoint where ServiceControl listen to requests.

it is possible to customize the 2 value above in several manner:

* Edit the ServiceControl configuration file located in the installation directory adding the following settings:

	`xml
    	<add key="ServiceControl/Name" value="YourFavoriteName" />
    	<add key="ServiceControl/Description" value="ServiceControl service description" />
	`
	
* Change the registry settings in the `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl` node editing the `Name` and `Description` keys;
* Change the regstry settings in the `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ParticularSoftware\ServiceControl` node editing the `Name` and `Description` keys;

Restart the ServiceControl service to apply the new configuration.

All the above locations are checked by ServiceControl to read the name and Descrition properties, the order in which they are checked is the following:

1. Config file
1. Registry 64
1. Registry 32