---
title: Managing ServiceControl via PowerShell
reviewed: 2021-08-23
---

## ServiceControl PowerShell module

The `Particular.ServiceControl.Management` module can be installed from the [PowerShell Gallery](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management), and is used to add, remove, update and delete instances of ServiceControl.

## Installing and using the PowerShell module

In order to use the PowerShell module, the PowerShell execution policy needs to be set to `RemoteSigned`. Refer to the [PowerShell documentation](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.security/set-executionpolicy) on how to change the execution policy.

The module can be installed from the PowerShell Gallery with the following command:

```ps
Install-Module -Name Particular.ServiceControl.Management
```

Once the module is installed, it can be used by importing the module into the PowerShell session with the following command:

```ps
Import-Module Particular.ServiceControl.Management
```

NOTE: The majority of the cmdlets will only work if the PowerShell session is running with administrator privileges.

INFO: The ServiceControl installer currently includes a legacy version of the PowerShell module called `ServiceControlMgmt` that is only supported on Windows PowerShell 5.1. It does not work with newer versions of PowerShell. The ServiceControl installer creates a shortcut in the Windows start menu to launch an administrative PowerShell Session with this legacy module automatically loaded. The legacy module is not signed, so the PowerShell execution policy needs to be set to `Unrestricted` to use it.

### Troubleshooting 

```
Method not found: 'System.Security.AccessControl.DirectorySecurity System.IO.DirectoryInfo.GetAccessControl(System.Security.AccessControl.AccessControlSections)'
``` 
This indicates that the PowerShell module is being executed using a newer version of PowerShell than it supports. To resolve this issue, make sure to use the [PowerShell module](/servicecontrol/powershell.md#servicecontrol-powershell-module) hosted on the [PowerShell Gallery](https://www.powershellgallery.com/packages/Particular.ServiceControl.Management/).

## Powershell Commands

For a complete overview of all cmdlets, visit the [Managing ServiceControl via PowerShell](/servicecontrol/installation-powershell.md) page.

## Troubleshooting via PowerShell

The ServiceControl Management PowerShell module offers some cmdlets to assist with troubleshooting the installation of ServiceControl instances.

### Check if a port is already in use

Before adding an instance of ServiceControl test if the port to use is currently in use.

```ps
Test-IfPortIsAvailable -Port 33333
```

This example shows the available ports out of a range of ports

```ps
33330..33339 | Test-IfPortIsAvailable | ? Available
```


### Checking and manipulating UrlAcls

The Window HTTPServer API is used by underlying components in ServiceControl. This API uses a permissions system to limit what accounts can add a HTTP listener to a specific URI. The standard mechanism for viewing and manipulating these ports is with the [netsh.exe](https://technet.microsoft.com/en-us/library/Cc725882.aspx) command line tool.

For example `netsh.exe http show urlacl` will list all of the available UrlAcls. This output is detailed but not easy to query. The ServiceControl Management PowerShell provides simplified PowerShell equivalents for listing, adding, and removing UrlAcls and makes the output easier to query.

For example the following command lists all of the UrlAcls assigned to any URI for port 33333.

```ps
Get-UrlAcls | ? Port -eq 33333
```

In this example any UrlAcl on port 33335 is remove

```ps
Get-UrlAcls | ? Port -eq 33335 | Remove-UrlAcl
```

The following example shows how to add a UrlAcl for a ServiceControl service that should only respond to a specific DNS name. This would require an update of the ServiceControl configuration file as well. Refer to [setting a custom host name and port number](setting-custom-hostname.md)

```ps
Add-UrlAcl -Url http://servicecontrol.mycompany.com:33333/api/ -Users Users
```
