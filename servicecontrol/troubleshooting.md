---
title: Troubleshooting
summary: Troubleshooting ServiceControl installation and common issues
tags:
- ServiceControl
- Troubleshooting
---


### Check the configuration via the Management utility

Open the ServiceControl Management utility and review the configuration. The Management utility is a quick way to get the information needed to troubleshoot a ServiceControl issue.


### Service fails to start

There are various reasons that can cause the ServiceControl windows service fail to start. To narrow down the possible cause, review the [ServiceControl logs](logging.md) files.


### The port is already in use

When adding a ServiceControl instance via the management utility, ServiceControl PowerShell Module or a silent install via an unattended XML configuration the port number is checked to ensure it is available. This is not foolproof though as another application or service that uses the same port may not be running at the time the service is added.

In the event that the service fails to start check if the configured port ( typically port 33333) is available. To do this open up a elevated command prompt and issue the following command:

```dos
netstat -a -b
```


### Missing queue

The service expects to be able to connect to the error, audit and forwarding queues specified in the configuration. If the configuration has been manually changes ensure the specified queues exist.


### Cannot connect to the queues

Some transports have access controls build into them. Ensure the service account specified has sufficient rights to access the queues.


### Service won't start after changing service accounts.

 1. The service account has access read rights to the directory the service is installed
 1. The service account has access read/write rights to the database and logs directories specified in the configuration.
 1. The service account has the logon as a service privilege.
 1. Ensure that a URLACL exists for the service (see next point for further info on listing URLACLs
 1. Ensure group or account specified in the URLACL covers the service account.
 1. Confirm that the service account has sufficient writes to manage the configured queues. See [Configuring a Non-Privileged Service Account](configure-non-privileged-service-account.md) for a breakdown of the queues to check.


Note: To examine the configured URLACLs use either the ServiceControl Management PowerShell prompt and issue `Get-UrlAcls` or to examine this from a command prompt using this command line `netsh http show urlacl`.


### Service fails to start: EsentInstanceUnavailableException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentInstanceUnavailableException` ensure that ServiceControl [database directory](configure-ravendb-location.md), sub-directory and files, is excluded from any anti-virus and anti-maleware real-time and scheduled scan.


### Service fails to start: EsentDatabaseDirtyShutdownException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentDatabaseDirtyShutdownException` run Esent Recovery against the ServiceControl database followed by an Esent Repair.

 1. Open an elevated command prompt and navigate to the ServiceControl [database directory](configure-ravendb-location.md)
 1. Change to the RavenDB directory (the default is `localhost-33333`)
 1. Run `esentutl /r RVN /l "logs"` and wait for it to finish
 1. Run `esentutl /p Data` and wait for it to finish
 1. Restart ServiceControl


### Service crashes when hard disk is busy

ServiceControl can run out of memory and crash when the hard drive is busy. When this happens note the following error in the logs

```
The version store for this instance (0) has reached its maximum size of 511Mb. It is likely that a long-running transaction is preventing cleanup of the version store and causing it to build up in size. Updates will be rejected until the long-running transaction has been completely committed or rolled back.
```

Increase the size of the version store by adding a new app setting to the ServiceControl configuration file:

`<add key="Raven/Esent/MaxVerPages" value="1024" />`

The value is the size of the version store in MB.


### Unable to connect to ServiceControl from either ServiceInsight or ServicePulse

 1. Logon to the PC hosting ServiceControl.
 1. Open the ServiceControl Management Utility.
 1. Click the ServiceControl instance is Running.
 1. Click the URL under 'Host'. A valid response with JSON data will be received.
 1. If having issues remotely connecting to ServiceControl. Verify that firewall settings do not block access to the ServiceControl port specified in the URL.

NOTE: Prior to changing firewall setting to expose ServiceControl read [Securing ServiceControl](securing-servicecontrol.md).


### Unable to start Service after exposing RavenDB

The `ExposeRavenDB` setting enables the embedded RavenDB Management Studio to be accessible via a web browser.
When this setting is used in combination with a low privilege service account it can cause the service fail on startup.
This is because a URLACL is required and the service account does not have rights to create it.
 
To workaround this create the required URLACL. This can be done using the ServiceControl Management Powershell module.  

NOTE: Replace the `<hostname>` and `<port>` values in the sample commands below with the appropriate values from the ServiceControl configuration.

```ps
urlacl-add -Url http://<hostname>:<port>/storage/ -Users Users
```

If the `ExposeRavenDB` setting is removed or disabled in the configuration then the URLACL can be cleaned up using this command:

```ps
urlacl-list | ? VirtualDirectory -eq storage | ? Port -eq <port> | urlacl-delete
``` 