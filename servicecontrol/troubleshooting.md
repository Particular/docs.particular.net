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

### Method not found: 'Void System.Net.Http.Formatting.BaseJsonMediaTypeFormatter.set_SerializerSettings(Newtonsoft.Json.JsonSerializerSettings)'

If the following exception occurs at startup, this is likely because there are one or more versions of `Newtonsoft.Json` registered in the Global Assembly Cache (GAC).

```txt
Service cannot be started. System.MissingMethodException: Method not found: 'Void System.Net.Http.Formatting.BaseJsonMediaTypeFormatter.set_SerializerSettings(Newtonsoft.Json.JsonSerializerSettings)'.
```

This problem can be resolved by removing `Newtonsoft.Json` entries from the GAC. It can be done with the [gacutil command](https://docs.microsoft.com/en-us/dotnet/framework/tools/gacutil-exe-gac-tool) in an elevated (administrator) console:

```cmd
gacutil /u Newtonsoft.Json
```

It may be required to first remove all `HKEY_LOCAL_MACHINE\SOFTWARE\Classes\Installer\Assemblies\Global Newtonsoft.Json` keys from the registry before using `gacutil /u Newtonsoft.Json`.

## Resolve messages that cannot be retried 

If certain messages are not scheduled for retry and the logs show the following message then the database could be in an inconsistent state:

```txt
2020-10-16 13:31:58.9863|190|Info|ServiceControl.Recoverability.RetryProcessor|Retry batch RetryBatches/1c33af76-8177-494d-ae9a-af060cefae02 cancelled as all matching unresolved messages are already marked for retry as part of another batch.
2020-10-16 13:31:59.2826|173|Info|ServiceControl.Recoverability.InMemoryRetry|Retry operation bf05499a-9261-41ec-9b49-da40e22a6f20 completed. 1 messages skipped, 0 forwarded. Total 1.
```

The internal *FailedMessageRetries* collection must be purged in order to restore retries for such messages.

1. Upgrade to the [latest ServiceControl version](https://github.com/particular/servicecontrol/releases)
1. Ensure that currently there are no retry operations active
1. Start the instance in Maintenance Mode
1. Open the embedded RavenDB Management Studio
1. Select the "FailedMessageRetries" collection in the left tree
1. Delete all documents in the collection
1. Stop maintenance mode

## SmartScreen blocks the installer

The installer is [code signed](https://en.wikipedia.org/wiki/Code_signing), but [SmartScreen](https://en.wikipedia.org/wiki/Microsoft_SmartScreen) (called Windows SmartScreen, Windows Defender SmartScreen and SmartScreen Filter in different places) may classify the code signing certificate as "untrusted" and block the installer from running until permission is granted by the user.

Although the installer is code signed correctly with a certificate owned by "NServiceBus Ltd", SmartScreen will block it from running until Microsoft has built enough "trust" in the certificate. One of the main inputs to building that trust is when users grant permission to run the installer. To grant permission to run the installer, click "Run Anyway". This will no longer be required when Microsoft decides to trust the certificate.

When building ServiceControl, all build artifacts are virus scanned to ensure no viruses or malware are shipped with the installer packages.

## Stale indexes

The database technology used for ServiceControl is based on asynchronous index updates. Indexes are not updated immediately, but very soon after data updates. A healthy system has indexes updated in milliseconds or up to several seconds under load. When indexes get very stale, this means that indexes lag behind for a long duration and can affect data presented and started tasks.

Systems are affected by severe index lag when the following custom check message is presented:

> At least one index significantly stale. Please run maintenance mode if this custom check persists to ensure index(es) can recover. See log file in `{LogPath}` for more details.

After launching a ServiceControl instance in maintenance mode, message ingestion stops but the database engine still runs and messages will continue to queue. This ensures that any tasks related to index rebuilding or index scanning can run without interruption. This is useful when the storage isn't fast enough to do both message ingestion and index operations.

Consider upgrading the storage if these errors persists.

Contact [Particular support](https://particular.net/support) for assistance.

## Index errors

Index issues are usually automatically corrected at start-up time but sometimes index issues require manual intervention. When unrecoverable index issues occur the following custom check message is visible:

> Detected RavenDB index errors, please start maintenance mode and resolve the following issues:

Often [indexes get corrupted](#corrupted-indexes). Resolve these errors by inspecting the errors in [ServiceControl (audit or error) maintenance mode](maintenance-mode.md).

Contact [Particular support](https://particular.net/support) for assistance.

## Corrupted indexes

Sometimes the following error may be observed:

```txt
Raven.Abstractions.Exceptions.IndexDisabledException: The index has been disabled due to errors
```

or

```txt
2021-03-23 09:27:50.0593|14|Warn|Raven.Database.DocumentDatabase|Could not create index batch
System.InvalidOperationException: Cannot modify indexes while indexing is in progress (already waited full minute). Try again later
```

This risk of these error occurring is mitigated by:

- [Excluding the database storage folder from virus scanning](servicecontrol-in-practice.md#anti-virus-checks)
- [Ensuring enough storage space is available](capacity-and-planning.md#storage-size)
- [Setting up server monitoring and proactively monitoring free storage space](servicecontrol-in-practice.md#server-monitoring)

To resolve these errors, the affected indexes must be rebuilt:

- Start the [ServiceControl (audit or error) in maintenance mode](maintenance-mode.md)
- In RavenDB Management Studio, navigate to the Indexes view
- [Reset the relevant index(es)](https://ravendb.net/docs/article-page/3.5/csharp/server/administration/index-administration)

If many indexes are affected it may be easier to rebuild all indexes, although this can take a very long time if the database is large, and it will use a lot of CPU and storage IO capacity:

- Stop the ServiceControl (audit or error) instance
- Navigate to the [database folder](configure-ravendb-location.md) on disk
- Delete the `Indexes` folder
- Start the ServiceControl instance
