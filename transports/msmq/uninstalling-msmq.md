---
title: Uninstalling the MSMQ Service
summary: How to remove the Microsoft Messaging Queue (MSMQ) service
reviewed: 2020-01-24
redirects:
 - nservicebus/msmq/uninstalling-msmq
---


The Platform Installer and the NServiceBus.PowerShell modules provide a simple mechanism for installing and configuring the MSMQ service to suit NServiceBus. Particular does not provide an uninstall for this as there are built-in removal options within the Windows operating system.

The removal instructions vary depending on the operating system and are detailed below.


## Before proceeding

DANGER: Removing the MSMQ Service is a destructive operation which can result in data loss

When the MSMQ service is uninstalled the following actions are also carried out:

 * All existing queues and queue configuration information are deleted
 * All messages contained in those queue and the system dead letter queue (DLQ) are deleted


### Dependent services

Services in Microsoft Windows can be configured to depend on each other. Prior to removing MSMQ, ensure that no services depend on MSMQ. To do this

 * Load the Windows Services MMC snapin `Services.msc`,
 * Right click on `Message Queuing` in the list of services
 * Check the `Dependencies` tab in the window to see if any dependencies exist 

Alternatively this can be done from PowerShell with the following command:

```ps
(Get-Service MSMQ).DependentServices
```


## Removal instructions


### Interactive removal


#### Windows 2012/Windows 2016

 * Open Server Manager
 * From the manage menu, click the Remove Roles and Features
 * This will open the "Remove Roles and Features" Wizard
 * Click `Next` until the Features option is shown
 * Scroll down, deselect the `Message Queuing` option, and click `Next`
 * Click the `Remove` Button to complete the removal.

A reboot may be required to finalize the changes.


#### Windows 8/Windows 10

 * Open the Programs option from Control Panel
 * Under Programs and Features click on `Turn Windows features on or off`
 * Scroll down and deselect the `Microsoft Message Queue (MSMQ) Server` option and then click `OK`

Reboot to finalize the changes.


### Removal using DISM.exe

`DISM.exe` is the command line tool Microsoft provides for enabling and disabling Windows Features such as the MSMQ subsystem on Windows and Windows Server.

`DISM.exe` requires admin privileges so all the commands listed should be run from an admin command prompt.

NOTE: DISM command line options and feature names are all case-sensitive.

To list which MSMQ features are enabled:

```dos
DISM /Online /Get-Features /Format:Table | FINDSTR "^MSMQ-"
```

The output will be similar to this:

```
MSMQ-Container                                        | Enabled
MSMQ-Server                                           | Enabled
MSMQ-Triggers                                         | Disabled
MSMQ-ADIntegration                                    | Disabled
MSMQ-HTTP                                             | Disabled
MSMQ-Multicast                                        | Disabled
MSMQ-DCOMProxy                                        | Disabled
WCF-MSMQ-Activation45                                 | Disabled
```

To disable a feature execute the following:

```dos
DISM /Online /Disable-Feature /FeatureName:<FeatureName>
```

Once a feature is removed, reboot the system to finalize the changes.


#### Removal from a PowerShell prompt

Windows 8 and higher and Windows Server 2012 and higher ship with a PowerShell module for managing installed features that mirrors the DISM.exe command line functions.

The following PowerShell script uses the DISM Module to remove any `MSMQ` features form the system.

```ps
Import-Module DISM
Get-WindowsOptionalFeature -Online |
 ? FeatureName -Match MSMQ |
 ? State -EQ Enabled | % {
	 Disable-WindowsOptionalFeature -Online -FeatureName $_.FeatureName -NoRestart
}
```

The script is suppressing restarts to stop a prompt being shown for each feature as it is removed. Once the script has completed the system should be restarted to finalize the changes.