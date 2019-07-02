The following command will list the ServiceControl instances current installed and their version number.

```ps
Get-ServiceControlInstances | Select Name, Version
```

To upgrade an instance to the latest version of the binaries:

```ps
Invoke-ServiceControlInstanceUpgrade -Name <Instance To upgrade>
```

The upgrade will stop the service if it is running. Additional parameters for `Invoke-ServiceControlInstanceUpgrade` may be required. The configuration file of the existing version is examined prior to determine if all the required settings are present. If a configuration setting is missing then the cmdlet will throw an error indicating the required additional parameter.

NOTE: Upgrading an instance to version may require the use of a different cmdlet. See [the upgrade guide](/servicecontrol/upgrade/3to4/) for more information.