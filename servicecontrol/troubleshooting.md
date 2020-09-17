---
title: Troubleshooting
summary: Troubleshooting ServiceControl installation and common issues
reviewed: 2020-06-26
---

### Check the configuration via ServiceControl Management

Open ServiceControl Management and review the instance configuration. The user interface presents basic installation information for each instance of the ServiceControl service installed. To review the application configuration file for a specific instance click the installation path and then locate `ServiceControl.exe.config` from the Explorer window.

### Service stops unexpectedly

The ServiceControl Windows Services are configured for automatic restart via Windows Service recoverability policy. The services are restarted after 1 minute of the unplanned shutdown.

### Service fails to start

There are various reasons that can cause the ServiceControl Windows Service fail to start. If a critical exception is thrown at service start up this is reported via an error message in the `Application` Windows Event Log. Additional information may also be present in the [ServiceControl logs](logging.md).

### The port is already in use

When adding a ServiceControl instance the configured port number is checked to ensure it is available. This is not infallible though as another application or service that uses the same port may not be running at the time the service is added.

In the event that the service fails to start to check if the configured port (typically port 33333) is available. To do this open up an elevated command prompt and issue the following command:

```dos
netstat -a -b
```
or use the provided [ServiceControl Management PowerShell](/servicecontrol/powershell.md) cmdlet to check a specific port:

```ps
Test-IfPortIsAvailable -Port 33333
```

### Missing queue

The service expects to be able to connect to the error, audit and forwarding queues specified in the configuration. If the configuration has been manually changed ensure the specified queues exist.

### Cannot connect to the queues

Some transports have access controls built into them. Ensure the service account specified has sufficient rights to access the queues.

### Service won't start after changing service accounts

 1. The service account has access read rights to the directory the service is installed
 1. The service account has access read/write rights to the database and logs directories specified in the configuration.
 1. The service account has the logon as a service privilege.
 1. Ensure that a URLACL exists for the service (see next point for further info on listing URLACLs
 1. Ensure the group or account specified in the URLACL covers the service account.
 1. Confirm that the service account has sufficient writes to manage the configured queues. See [Configuring a Non-Privileged Service Account](configure-non-privileged-service-account.md) for a breakdown of the queues to check.

Note: To examine the configured URLACLs use either the PowerShell prompt and issue `Get-UrlAcls` or to examine the ACLS from a command prompt using the command line `netsh http show urlacl`.

### Service fails to start: EsentInstanceUnavailableException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentInstanceUnavailableException` ensure that ServiceControl [database directory](configure-ravendb-location.md), sub-directory and files, is excluded from any anti-virus and anti-malware real-time and scheduled scan.

### Service fails to start: EsentDatabaseDirtyShutdownException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentDatabaseDirtyShutdownException` run Esent Recovery against the ServiceControl database followed by an Esent Repair.

 1. Open an elevated command prompt and navigate to the ServiceControl [database directory](configure-ravendb-location.md) (the default is `%PROGRAMDATA%\Particular\ServiceControl\Particular.ServiceControl\DB`)
 1. Run `esentutl /r RVN /l "logs"` and wait for it to finish
 1. Run `esentutl /p Data` and wait for it to finish
 1. Restart ServiceControl

### Unable to connect to ServiceControl from either ServiceInsight or ServicePulse

 1. Log on to the machine hosting ServiceControl.
 1. Open ServiceControl Management.
 1. Click the on the ServiceControl instance that is running and needs to be examined.
 1. Click the URL under 'Host'. A valid response with JSON data will be received.
 1. If having issues remotely connecting to ServiceControl. Verify that firewall settings do not block access to the ServiceControl port specified in the URL.

NOTE: Before changing firewall setting to expose ServiceControl read [Securing ServiceControl](securing-servicecontrol.md).

## Migrate data to a different disk

If the disk containing the ServiceControl database is either to small or is too slow then the files can easily be migrated to another disk.

1. Open the ServiceControl Management Utility.
2. Stop the ServiceControl instance type.
3. Open the DB location by clickinb DB Path.
4. Copy the files to their new destination and ensure ServiceControl has read/write permissions (Moving is not recommended).
5. Update the database path 
6. Start the instance.
7. Verify if it is running without issues by inspecting the log files.
8. Archive or delete the data from the original location.

## Migrate data to a different server

This is very similar to [Migrate data to a different disk](#migrate-data-to-a-different-disk).

1. Install the same version of ServiceControl on the new server.
2. Create an instance that has the exact same instance name.
  - Name must be identical! This defines the queue name.
  - Disk locations can be different
3. Stop the instance.
4. Open the database location and remove all data.
5. Copy the database files from the previous location to the new location as per [Migrate data to a different disk](#migrate-data-to-a-different-disk).
7. Verify if it is running without issues by inspecting the log files.
8. Archive or delete the data from the original location.

