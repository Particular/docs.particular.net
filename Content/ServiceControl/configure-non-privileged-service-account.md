---
title: Configuring a Non-Privileged Service Account
summary: Describes the changes made to allow ServiceControl V1.2 to run as a low privilege domain account
tags:
- ServiceControl
- Configuration
---

ServiceControl 1.2 installation sets up the service to run under the LOCALSYSTEM account.  This configuration works with the default MSMQ transport, but when working with other transports such as SQL, you should change the service account to a custom user account to allow use of Windows integrated credentials. 

Prior to ServiceControl V1.2 it was possible to change the account to a user account but that account required local administrator privileges. From V1.2, a lower privilege domain account can operate as the service account. 

Specifically, these are the changes:

- The URLACL registration is only attempted if the service account has Administrator privileges
- The file system ACLs on the folder containing the embedded RavenDB allow read/write access by members of the "Users" group
- For the default URLACL registration, members of the Windows "Users" group are authorized to listen on the endpoint URL

### Required Manual Configuration
To allow a non-privileged account to function, you must configure some steps manually. 

For MSMQ, the ACL default for a queue allows Administrators full access.  Switching to a low privileged account means that you need to modify the rights to give full control to the custom account.

At a minimum, you should modify the rights to these queues:

- particular.servicecontrol
- particular.servicecontrol.errors
- particular.servicecontrol.timeouts
- particular.servicecontrol.timeoutsdispatcher
- audit
- error
- error.log

If the service account user does not have appropriate rights the service will stop.

### Configuration Changes
If the ServiceControl configuration is manually changed to listen to an alternate URL as detailed in  [Customizing ServiceControl configuration](creating-config-file), then update the URLACL to reflect the user account assigned to run the service.  Otherwise, the service will not start.

### RavenDB Security
The installer will set the permissions to allow any member of the local Windows Users group to modify files in the embedded Raven DB folder.  You can change these rights manually to be more restrictive as long as the service account user retains modify rights.  Note that manual changes to the ACLs may be lost during an upgrade or re-installation of ServiceControl.  

### Logging 
Out of the box ServiceControl persists logs and failed message imports to "%LOCALAPPDATA%\Particular\ServiceControl\logs".  The %LOCALAPPDATA% defines a user-specific location on disk, so the logging location will be different when the service is configured as a user account.      
The logging location may also have been manually overridden as detailed in [Configuring the Log Location](setting-custom-log-location). If so, configure the permissions of the logging location to give the service account full access to the directory.

If the service was previously running as LOCALSYSTEM you may want to migrate the logs to the new location.  When you do so,  ensure that the file ACLs are not copied from the original location.     

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
3. Ensure that the Particular.ServiceControl service is stopped. 
3. From the command prompt running as the service account, change to the ServiceControl installation directory and run ServiceControl.exe. 

```
cd "C:\Program Files (x86)\Particular Software\ServiceControl"
ServiceControl.exe 
```
3. Examine the output and confirm that there are no critical errors.
3. Shut down the console session. 
3. Start the service.

### Expected Warnings when Running as a Non-Privileged Account
On service start up the Embedded RavenDB attempts to create Windows performance counters. This does not succeed and RavenDB performance counters are not available.
You can safely ignore this warning.
