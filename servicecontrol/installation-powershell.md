---
title: Managing Instances via PowerShell
reviewed: 2016-10-06
tags:
 - ServiceControl
 - Installation
 - PowerShell
---


## ServiceControl PowerShell

ServiceControl 1.7 introduced a new graphical management utility to add, remove, update and delete instances of the ServiceControl service. These actions and some additional tools have also exposed via PowerShell module called `ServiceControlMgmt`.


## Prerequisites

The ServiceControlMgmt module requires:

 * Microsoft PowerShell 3.0


## Loading and Running the PowerShell Module

The majority of the ServiceControlMgmt PowerShell module cmdlets will only work if the PowerShell session is running under administrator privileges. The ServiceControl installer creates a shortcut in the Windows start menu to launch an administrative PowerShell Session with the module automatically loaded. Alternatively the module can be loaded directly into an an existing PowerShell session by loading `ServiceControlMgmt.psd1` using the `Import-Module` cmdlet as show below:

```ps
Import-Module "C:\Program Files (x86)\Particular Software\ServiceControl Management\ServiceControlMgmt.psd1"
```


## Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                    |
| sc-addfromunattendfile | New-ServiceControlInstanceFromUnattendedFile  |
| sc-addlicense          | Import-ServiceControlLicense                  |
| sc-delete              | Remove-ServiceControlInstance                 |
| sc-findlicense         | Get-ServiceControlLicense                     |
| sc-help                | Get-ServiceControlMgmtCommands                |
| sc-instances           | Get-ServiceControlInstances                   |
| sc-makeunattendfile    | New-ServiceControlUnattendedFile              |
| sc-transportsinfo      | Get-ServiceControlTransportTypes              |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade          |
| urlacl-add             | Add-UrlAcl                                    |
| urlacl-delete          | Remove-UrlAcl                                 |
| urlacl-list            | Get-UrlAcls                                   |
| port-check             | Test-IfPortIsAvailable                        |
| user-sid               | Get-SecurityIdentifier                        |


### Examples

To following commands show some uses of some of the cmdlets provided in the module. All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-ServiceControlInstances
```


### Adding an instance

```ps
New-ServiceControlInstance -Name Test.ServiceControl -InstallPath C:\ServiceControl\Bin -DBPath C:\ServiceControl\DB -LogPath C:\ServiceControl\Logs -Port 33334 -Transport MSMQ -ErrorQueue error1 -AuditQueue audit1
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string or hostname.


### Removing an instance

The following commands show how to remove a ServiceControl instance(s). To List existing instances of the ServiceControl service use `Get-ServiceControlInstances`.

Remove the instance that was created in the Add sample and delete the database and logs:

```ps
Remove-ServiceControlInstance -Name Test.ServiceControl -RemoveDB -RemoveLogs
```

Remove all ServiceControl instance created in the Add sample and delete the database and logs for each one:

```ps
Get-ServiceControlInstances | Remove-ServiceControlInstance -RemoveDB -RemoveLogs
```

There are additional parameters available to set additional configuration options such as forwarding queues, the transport connection string or host name.


### Upgrading an instance

The following command will list the ServiceControl instances current installed and their version number.

```ps
Get-ServiceControlInstances | Select Name, Version
```

To upgrade and instance to the latest version of the binaries run.

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance To upgrade>
```

The upgrade will stop the service if it is running. Additional parameters for `Invoke-ServiceControlInstanceUpgrade` may be required. The configuration file of the existing version is examined prior to deter,in e if all the required settings are present. If a configuration setting is missing  then the cmdlet will throw an error indicating the required additional parameter.


### Licensing

Add the license file to the registry by running the following cmdlet.

```ps
Import-ServiceControlLicense <License-File>
```

The license file is added to the `HKEY_LOCAL_MACHINE` registry hive so it available to all instances of ServiceControl installed on the machine.


### Building an unattended install file

Since ServiceControl 1.7 the installation executable has a MSI command line argument to enable the installation of a ServiceControl service instance during installation. This is intended to assist with [unattended installation](installation-silent.md)

The MSI command line argument requires an XML file which detail the instance options. The file can be produced by running the following cmdlet or by manually creating the XML file.

```ps
New-ServiceControlUnattendedFile -OutputFile c:\temp\unattended.xml -Name Test -InstallPath c:\servicecontrol\test\bin -DBPath c:\servicecontrol\test\db -LogPath c:\servicecontrol\test\logs -Port 33335 -ErrorQueue error-test -AuditQueue audit-test -ErrorLogQueue errorlog-test -AuditLogQueue auditlog-test -Transport MSMQ -ForwardAuditMessages $false -ForwardErrorMessages $false
```

This sample produces the following Files

```xml
<?xml version="1.0"?>
<ServiceControlInstanceMetadata xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <LogPath>C:\servicecontrol\test\db</LogPath>
  <DBPath>C:\servicecontrol\test\logs</DBPath>
  <HostName>localhost</HostName>
  <InstallPath>C:\servicecontrol\test\bin</InstallPath>
  <Port>33335</Port>
  <ErrorQueue>error-test</ErrorQueue>
  <ErrorLogQueue>errorlog-test</ErrorLogQueue>
  <AuditQueue>audit-test</AuditQueue>
  <AuditLogQueue>auditlog-test</AuditLogQueue>
  <ForwardAuditMessages>false</ForwardAuditMessages>
  <ForwardErrorMessages>false</ForwardEroroMessages>
  <TransportPackage>MSMQ</TransportPackage>
  <Name>Test</Name>
  <DisplayName>Test</DisplayName>
</ServiceControlInstanceMetadata>
```

NOTE: The settings contained in an unattended installation files are version specific. The file contents will be validated when used and if a required setting is missing an error will be logged. To correct this regenerate the XML file using the `New-ServiceControlUnattendedFile` cmdlet.


### Testing an unattended install file

There `New-ServiceControlInstanceFromUnattendedFile` cmdlet creates an instance from the unattended file. The service account details can optionally be provided. If no service account details are specified the `LocalSystem` account is used

```ps
New-ServiceControlInstanceFromUnattendedFile -UnattendFile c:\temp\unattended.xml -ServiceAccount MyServiceAccount -ServiceAccountPassword MyPassword
```

Note: Neither the unattended file method or the `New-ServiceControlInstance` cover all the configuration settings that are available to ServiceControl. To set additonal options refer to [Customizing ServiceControl configuration](creating-config-file.md). A scripted method of adding additional settings is detailed in [Installing ServiceControl Silently](installation-silent.md).


## Troubleshooting via PowerShell

The ServiceControl Management PowerShell offers some cmdlets to assist with troubleshooting the install of ServiceControl instances.


### Check if a Port is already in use

Before adding an instance of ServiceControl test if the port to use is currently in use.

```ps
Test-IfPortIsAvailable -Port 33333
```

This example shows the available ports out of a range of ports

```ps
33330..33339 | Test-IfPortIsAvailable | ? Available
```


### Checking and manipulating UrlAcls

The Window HTTPServer API is used by underlying components in ServiceControl. This API uses a permissions system to limit what accounts can add a HTTP listener to a specific URI.
The standard mechanism for viewing and manipulating these ports in via the [netsh.exe](https://technet.microsoft.com/en-us/library/Cc725882.aspx) command line tool.

For example `netsh.exe http show urlacl` will list all of the available. This output is detailed but not very friendly to query. The ServiceControl Management PowerShell provides simplified PowerShell equivalents for listing, add and removing UrlAcls and makes the output easier to query.

For example the following command lists all of the UrlAcls assigned to any URI for port 33333.

```ps
Get-UrlAcls | ? Port -eq 33333
```

In this example any UrlAcl on port 33335 is remove

```ps
Get-UrlAcls | ? Port -eq 33335 | Remove-UrlAcl
```

The following example shows how to add UrlAcl for a ServiceControl service that should only respond to a specific DNS name. This would require an update of the ServiceControl configuration file as well. Refer to [setting a custom host name and port number](setting-custom-hostname.md)

```ps
Add-UrlAcl -Url http://servicecontrol.mycompany.com:33333/api/ -Users Users
```