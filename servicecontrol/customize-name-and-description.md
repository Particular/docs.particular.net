---
title: Name and Description Properties for the API
summary: How to configure the default values for the Name and Description properties of the ServiceControl API
tags:
- ServiceControl
---
The ServiceControl HTTP API exposes two properties that describe the ServiceControl installation:

* Name: by default, the name of the machine where ServiceControl is installed
* Description: a description of the ServiceControl service

Retrieve these two properties by issuing an HTTP call to `http://localhost:33333/API/`, which is the default HTTP endpoint where ServiceControl listens to requests.

You can customize the two values in several ways:

* Modify ServiceControl configuration (see [Customizing ServiceControl configuration](creating-config-file.md)) using the following command:
  ```bat
  x:\Your_Installed_Path\ServiceControl.exe --restart -d="ServiceControl/Name==YourFavoriteName" -d="ServiceControl/Description==ServiceControl service description"
  ```

* Change the registry settings in the `HKEY_LOCAL_MACHINE\SOFTWARE\ParticularSoftware\ServiceControl` node, editing the `Name` and `Description` keys.
* Change the regstry settings in the `HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\ParticularSoftware\ServiceControl` node, editing the `Name` and `Description` keys.

### ServiceControl default installed location

By default ServiceControl is installed in `[Program Files]\Particular Software\ServiceControl`

To apply the new configuration, restart the ServiceControl service.

ServiceControl checks all the locations for the Name and Description properties, in this order:

1. Config file
1. Registry 64
1. Registry 32
