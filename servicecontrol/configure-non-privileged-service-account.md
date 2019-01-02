---
title: Configuring a Non-Privileged Account
summary: Using a low privilege account for ServiceControl
reviewed: 2018-10-04
---

To allow a low-privileged account to function as the the service account for ServiceControl, the following should be considered:


### Access control on queues

The connection string used by ServiceControl needs to needs to enable access to the following ServicControl queues: 

 * `particular.servicecontrol`
 * `particular.servicecontrol.errors`
 * `particular.servicecontrol.staging` (ServiceControl version 1.6 and above)
 * `particular.servicecontrol.timeouts`
 * `particular.servicecontrol.timeoutsdispatcher`

In addition connection string should grant rights to the configured audit and error queues and the corresponding forwarding queues. These are typically named:

 * `audit`
 * `error`
 * `error.log`
 * `audit.log`

If the service account user does not have appropriate rights, the service will fail to start.

NOTE: For MSMQ, the ACL default for a queue allows Administrators full access. Switching to a low-privileged account requires modification of rights to give full control to the custom account. 

### Url Namespace Reservations

The account under which the ServiceControl instance is running requires url namespace reservations for the hostname and ports used by the instance. The reservations can be managed from the command using [netsh.exe](https://docs.microsoft.com/en-us/windows/desktop/http/add-urlacl). For example, to add url reservation for `http:\\localhost:33533` to `LocalService` account the following command can be used `netsh http add urlacl=http://localhost:33533 user=LocalService listen=yes delegate=no`. 

For instructions on how to review and change the urls used by ServiceControl instance, refer to [Changing the ServiceControl URI](setting-custom-hostname.md).


### RavenDB security

The installer will set the permissions to allow any member of the local Windows Users group to modify files in the embedded Raven DB directory. These rights can be changed manually to be more restrictive as long as the service account user retains modify rights. Note that manual changes to the ACLs may be lost during an upgrade or re-installation of ServiceControl.


### Testing the configuration

These methods confirm that the user account has sufficient rights:

 * Configure and start the service as the user and then check the log files.
 * Interactively run ServiceControl as the user.

Note: When running the ServiceControl.exe from the command line, it is important to use the same command line switches that are used when running the service. The command line is visible from within the standard Windows Services user interface.  


![](servicedetailsview.png 'width=500')


#### Method 1: Running the service as a non-privileged user

 1. Open Computer Management.
 1. Change the service account to the non-privileged user and password and apply the change. The user account will be given "logon as a service" privilege.
 1. Start the service and confirm that it started.
 1. Examine the log file to ensure that the service is operating as expected. If the service does not start and the log file does not indicate the issue, try Method 2.


#### Method 2: Running the service interactively as a non-privileged user

To run the service this way, the user account must have rights to log on interactively on the computer.

 1. Log on to the computer with admin privileges.
 1. Substitute the appropriate domain and user name.
 1. Issue the following command, entering the password when prompted:

For example

```dos
runas /user:MyDomain\MyTestUser cmd.exe
```

If the command returns the error below, then the user account cannot be tested this way without adjusting the logon rights. Normally this only occurs if the computer is configured as a domain controller or the system administrator has restricted logon access using group policies.

```
1385: Logon failure: the user has not been granted the requested logon type at this computer.
```

Once logon rights are granted:

 1. Ensure that the service is stopped.
 1. From the command prompt running as the service account, change to the ServiceControl installation directory and run `ServiceControl.exe` with the `--serviceName` parameter. In the following example, the default name has been used. Check ServiceControl Management if unsure of the service name
 1. Examine the output and confirm that there are no critical errors.
 1. Shut down the console session.
 1. Start the service.

```dos
ServiceControl.exe --serviceName=Particular.ServiceControl
```

NOTE: Specify the correct name of the service on the command line as this impacts the queue names used.


### Expected warnings when running as a non-privileged account

On service start up, the embedded RavenDB attempts to create Windows performance counters. This does not succeed and RavenDB performance counters are not available. This warning can be safely ignored.
