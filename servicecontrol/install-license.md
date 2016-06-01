---
title: Install the license in ServiceControl
summary: Install the Particular Platform license in ServiceControl
tags:
- servicecontrol
- license
- platform
---


## Using the ServiceControl Management Utility

The ServiceControl Management utility allow has a simple Add License user interface  option which will import the designated license file into the registry.

![](managementutil-addlicense.png)

When the license is [installed in the registry](/nservicebus/licensing/license-management.md) it is available machine wide and applied to all Particular products running on that machine.

This screen utility has a corresponding [PowerShell cmdlet](installation-powershell.md) to allow the same action to be scripted.


## Using the registry

When the license is [installed in the registry](/nservicebus/licensing/license-management.md) it is available machine wide and applied to all Particular products running on that machine.


## Using the license file

Create a folder called `License` under the folder where ServiceControl is installed and copy `license.xml` file there. E.g. `{servicecontrol-directory}/license/license.xml`.