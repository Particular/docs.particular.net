---
title: Configuring a Non-Privileged Account
summary: Using a low privilege account for ServiceControl
reviewed: 2025-07-11
---

To use a low-privileged accounts for ServiceControl instances, the following should be considered:

## Access control on queues

The transport connection string used by ServiceControl must enable access to all of the ServiceControl queues as configured by the `InstanceName` setting:

 * [ServiceControl/InstanceName](/servicecontrol/servicecontrol-instances/configuration.md#host-settings-servicecontrolinstancename)
 * [ServiceControl.Audit/InstanceName](/servicecontrol/audit-instances/configuration.md#host-settings-servicecontrol-auditinstancename)
 * [ServiceControl.Monitoring/InstanceName](/servicecontrol/monitoring-instances/configuration.md#host-settings-monitoringinstancename)

The queues that ServiceControl needs to access will reflect the `InstanceName` used and the [instance type](/servicecontrol/#servicecontrol-instance-types):

> [!WARNING]
> If the connection string does not provide appropriate rights, the service may fail to start or may experience errors when certain operations are performed.

### All instance types:

Both read and send permissions are required for each of these queues:

 * `{InstanceName}`:
 * `{InstanceName}.errors`

### [Error instances](/servicecontrol/servicecontrol-instances/):

 * `{InstanceName}.staging`: Both read and send permissions are required.
 * `error` (see the [`ServiceBus/ErrorQueue`](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorqueue) setting): Read permission is required.
 * `error.log` (optional, see the [`ServiceBus/ErrorLogQueue`](/servicecontrol/servicecontrol-instances/configuration.md#transport-servicebuserrorlogqueue) setting): Send permission is required.
 * The Error instance will require send permission for every endpoint queue to allow for [failed message retries](/servicepulse/intro-failed-message-retries.md).
 * If subscribing to [ServiceControl integration events](/servicecontrol/contracts.md), send/publish permission to the subscriber queues and/or any pub/sub mechanism for the transport will be required.

### [Audit instances](/servicecontrol/audit-instances/):

 * `audit` (see the [`ServiceBus/AuditQueue`](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditqueue) setting): Read permission is required.
 * `audit.log` (optional, see the [`ServiceBus/AuditLogQueue`](/servicecontrol/audit-instances/configuration.md#transport-servicebusauditlogqueue) setting): Send permission is required.

> [!NOTE]
> For [MSMQ](/servicecontrol/transports.md#msmq), the ACL default for a queue allows Administrators full access. Switching to a low-privileged account requires modification of rights to give full control to the custom account.

## Filesystem paths

The service account running ServiceControl instance requires following filesystem level access rights:

| Path | Rights |
|------|--------|
| Executables (e.g.  `C:\Program Files (x86)\Particular Software\Particular.ServiceControl`) | Read |
| Logs (e.g. `C:\ProgramData\Particular\ServiceControl\Particular.ServiceControl\Logs`)      |   Write     |
| Database (e.g `C:\ProgramData\Particular\ServiceControl\Particular.ServiceControl\DB`) | Write|
| Database volume (e.g. `C:`) | Read Attributes|

> [!NOTE]
> The database volume `Read attributes` access right is needed by ServiceControl to query for total and total free space on the volume.

## Performance counters

ServiceControl requires access to Windows performance counter infrastructure. <!-- TODO: Is that still true? --> As a result the service account needs to be a member of [Performance Monitor Users](https://docs.microsoft.com/en-us/windows/security/identity-protection/access-control/active-directory-security-groups#a-href-idbkmk-perfmonitorusersaperformance-monitor-users) group.


## Testing the configuration

These methods confirm that the service account has sufficient rights:

 * Configure the ServiceControl Windows service to run under the custom service account, start it and check the log files.
 * Interactively run ServiceControl under the custom service account.

> [!NOTE]
> When running `ServiceControl.exe` from the command line, it is important to use the same command line switches that are used when running the Windows service. The command line is visible from within the standard Windows Services user interface.

![](servicedetailsview.png 'width=500')

### Method 1: Running the service as a non-privileged user

 1. Open Computer Management.
 1. Change the service account to the custom user, provide the password and apply the change. The account will be given "logon as a service" privilege.
 1. Start the service and confirm that it started.
 1. Examine the log file to ensure that the service is operating as expected. If the service does not start and the log file does not indicate the issue, try Method 2.

### Method 2: Running the service interactively as a non-privileged user

To run the service this way, the custom service account must have rights to log on interactively on the computer.

 1. Log on to the computer with admin privileges.
 1. Switch to the appropriate domain and username.
 1. Issue the following command, entering the password when prompted:

For example

```shell
runas /user:MyDomain\MyTestUser cmd.exe
```

If the command returns the error below, then the user account cannot be tested this way without adjusting the login rights. Normally this only occurs if the computer is configured as a domain controller or the system administrator has restricted login access using group policies.

```
1385: Logon failure: the user has not been granted the requested logon type at this computer.
```

Once login rights are granted:

 1. Ensure that the service is stopped.
 1. From the command prompt running as the service account, change to the ServiceControl installation directory and run `ServiceControl.exe` with the `--serviceName` parameter. In the following example, the default name has been used. Check ServiceControl Management if unsure of the service name
 1. Examine the output and confirm that there are no critical errors.
 1. Shut down the console session.
 1. Start the service.

```shell
ServiceControl.exe --serviceName=Particular.ServiceControl
```

> [!NOTE]
> Specify the correct name of the service on the command line as this impacts the queue names used.
