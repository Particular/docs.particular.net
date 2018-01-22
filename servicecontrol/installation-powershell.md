---
title: Manage ServiceControl instances via PowerShell
reviewed: 2017-07-26
tags:
 - Installation
 - PowerShell
---

NOTE: For general information about ServiceControl Powershell, including troubleshooting and licensing guidance, see [Managing ServiceControl via PowerShell](/servicecontrol/powershell.md).

## ServiceControl instance Cmdlets and Aliases

The following cmdlets and aliases are provided by the ServiceControl Management PowerShell module for the management of ServiceControl instances.

| Alias                  | Cmdlet                                        |
| ---------------------- | --------------------------------------------- |
| sc-add                 | New-ServiceControlInstance                    |
| sc-addfromunattendfile | New-ServiceControlInstanceFromUnattendedFile  |
| sc-delete              | Remove-ServiceControlInstance                 |
| sc-instances           | Get-ServiceControlInstances                   |
| sc-makeunattendfile    | New-ServiceControlUnattendedFile              |
| sc-transportsinfo      | Get-ServiceControlTransportTypes              |
| sc-upgrade             | Invoke-ServiceControlInstanceUpgrade          |



### Help

All of the cmdlets have local help which can be accessed via the standard PowerShell help command

```ps
Get-Help Get-ServiceControlInstances
```


### Adding an instance

```ps
New-ServiceControlInstance -Name Test.ServiceControl -InstallPath C:\ServiceControl\Bin -DBPath C:\ServiceControl\DB -LogPath C:\ServiceControl\Logs -Port 33334 -Transport MSMQ -ErrorQueue error1 -AuditQueue audit1 -ForwardAuditMessages:$false
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

The upgrade will stop the service if it is running. Additional parameters for `Invoke-ServiceControlInstanceUpgrade` may be required. The configuration file of the existing version is examined prior to determine if all the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter.


### Building an unattended install file

Since ServiceControl 1.7 the installation executable has a MSI command line argument to enable the installation of a ServiceControl service instance during installation. This is intended to assist with [unattended installation](installation-silent.md)

The MSI command line argument requires an XML file which detail the instance options. The file can be produced by running the following cmdlet or by manually creating the XML file.

```ps
New-ServiceControlUnattendedFile -OutputFile c:\temp\unattended.xml -Name Test -InstallPath c:\servicecontrol\test\bin -DBPath c:\servicecontrol\test\db -LogPath c:\servicecontrol\test\logs -Port 33335 -ErrorQueue error-test -AuditQueue audit-test -ErrorLogQueue errorlog-test -AuditLogQueue auditlog-test -Transport MSMQ -ForwardAuditMessages $false -ForwardErrorMessages $false
```

This sample produces the following file

```xml
<?xml version="1.0"?>
<ServiceControlInstanceMetadata
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      xmlns:xsd="http://www.w3.org/2001/XMLSchema">
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
