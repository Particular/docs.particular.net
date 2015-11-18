---
title: Configuring a Non-Privileged Service Account
summary: Using low privilege account for ServiceControl 
tags:
- ServiceControl
---

To allow a non-privileged account to function as the the service account for ServiceControl the following things should be considered:

### Access Control on queues

For MSMQ, the ACL default for a queue allows Administrators full access.  
Switching to a low privileged account means that you need to modify the rights to give full control to the custom account.
Assuming the service name the ServiceControl service is `particular.servicecontrol` the ServiceControl queues names would be  


- `particular.servicecontrol`
- `particular.servicecontrol.errors`
- `particular.servicecontrol.staging` (only created from v1.6+)
- `particular.servicecontrol.timeouts`
- `particular.servicecontrol.timeoutsdispatcher`

In addition the Service requiresrtights to the configured audit and error queues and the corresponding forwarding queues. These are typically named: 

- `audit`
- `error`
- `error.log`
- `audit.log`

If the service account user does not have appropriate rights the service will fail.

### Configuration Changes

If the ServiceControl configuration is manually changed to listen to an alternate URL as detailed in [Customizing ServiceControl configuration](creating-config-file.md), then update the URLACL to reflect the user account assigned to run the service.  Otherwise, the service will not start.

### RavenDB Security

The installer will set the permissions to allow any member of the local Windows Users group to modify files in the embedded Raven DB folder.  You can change these rights manually to be more restrictive as long as the service account user retains modify rights.  Note that manual changes to the ACLs may be lost during an upgrade or re-installation of ServiceControl.    

### Testing the Configuration

These methods confirm that the user account has sufficient rights:

 - Configure and start the service as the user and then check the log files.   
 - Interactively run ServiceControl as the user.

#### Method 1: Running the service as a non-privileged user 

1. Open computer management.
1. Change the service account to the non-privileged user and password and apply the change. The user account will be given "logon as a service privilege".
1. Start the service and confirm that it started.
1. Examine the log file to ensure that the service is operating as you expect. If the service does not start and the log file does not indicate the issue, try Method 2.

#### Method 2: Running the service interactively as a non-privileged user 

To run the service this way the user account must have rights to log on interactively on the computer.  
2. Log on to the computer with admin privileges. 
2. Substitute the appropriate domain and user name. 
2. Issue the following command, entering the password when prompted:

```
e.g.   runas /user:MyDomain\MyTestUser cmd.exe

```

If the command returns the error below then you cannot test the user account this way without adjusting the logon rights.  Normally this only occurs if the computer is configured as a domain controller or the System Administrator has restricted login access using group policies. 

``` 
1385: Logon failure: the user has not been granted the requested logon type at this computer.
```

Once logon rights are granted you can proceed: 
3. Ensure that the service is stopped. 
3. From the command prompt running as the service account, change to the ServiceControl installation directory and run ServiceControl.exe. You must specify the name of the Service on the command lime as this impacts the queues names used.  In the following example the default name has been used.  Check the ServiceControl Management Utility if you are unsure of the service name 

```
ServiceControl.exe --serviceName=Particular.ServiceControl
```

3. Examine the output and confirm that there are no critical errors.
3. Shut down the console session. 
3. Start the service.

### Expected Warnings when Running as a Non-Privileged Account

On service start up the Embedded RavenDB attempts to create Windows performance counters. This does not succeed and RavenDB performance counters are not available.
You can safely ignore this warning.
