The following command will list the ServiceControl instances current installed and their version number.

```ps
Get-ServiceControlInstances | Select Name, Version
```

To upgrade an instance to the latest version of the binaries:

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance To upgrade>
```

The following command will list the ServiceControl Audit instances currently installed and their version number.

```ps
Get-AuditInstances | Select Name, Version
```

To upgrade an instance to the latest version of the binaries:

```ps
Invoke-AuditInstanceUpgrade -Name <Instance To upgrade>
```

These upgrades will stop the service if it is running. Additional parameters for the upgrade cmdlets may be required. The configuration file of the existing version is examined prior to determine if all the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter.
