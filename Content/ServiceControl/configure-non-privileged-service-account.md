---
title: Configuring a non-privileged Service Account
summary: Describes the changes made to allow ServiceControl V1.2 to run as a low privilege domain account
tags:
- ServiceControl
---

The installation of ServiceControl 1.2 sets up the service to run as under the LOCALSYSTEM account.  This configuration works with the default transport (MSMQ) but when working with other transports such as SQL it is preferable to change the service account to a custom user account to allow Windows integrated credentials to be used. 

Prior to 1.2 of ServiceControl it was possible to change the account to a user account but that account was required to have local administrator privileges.   In version 1.2 changes have been made to allow a lower privilege domain account to operate as the service account. 

Specifically the changes made were:

- the URLACL registration is only attempted if the service account has Administrator privileges
- the file system ACLs on the folder containing the embedded RavenDB have been changed to allow read/write access by members of the "Users" group
- The default URLACL registration has been changed so members of the Windows "Users" group are authorized to listen on the endpoint URL

### Required Manual configuration
Even with the changes made in v1.2 there are some manual configuration steps that need to be completed to allow a non-privileged account to function. 

In the case of MSMQ, the ACL defaults for a queue is to allow Administrators to have full access.  Switching to a low privileged account means that the rights needs to be modified to give full control to the custom account.

At a minimum this would entail modifying the rights to the following queues:

- particular.servicecontrol
- particular.servicecontrol.errors
- particular.servicecontrol.timeouts
- particular.servicecontrol.timeoutsdispatcher
- audit
- error
- error.log

If the service account user does not have appropriate rights the service will stop.

### Configuration Changes
If the configuration of ServiceControl is manually changed to listen to an alternate URL as detailed in  [Customizing ServiceControl configuration](creating-config-file) then the URLACL used should reflect the user account assigned to run the service.  If this is incorrect the service will not start.

### RavenDB Security
The installer will set the permissions to allow any member of the local Windows Users group to have modify rights to files in the embedded Raven DB folder.  These right can be changed manually to be more restrictive as long as the service account user retains modify rights.  Manual changes to the ACLs may be lost during an upgrade or re-installation of ServiceControl.  

### Logging 
Out of the box ServiceControl persists logs and failed message imports to "%LOCALAPPDATA%\Particular\ServiceControl\logs".  The %LOCALAPPDATA% defines a user specific location on disk, so the logging location will be different when the service is configured as a user account.      
The logging location may also have been manually overridden as detailed in [Configuring the Log Location](setting-custom-log-location), in this case the permissions of the logging location should be configured  to give the service account full access to the directory.

If the service was previously running as LOCALSYSTEM you may want to migrate the logs to the new location.  When doing so please ensure that the file ACLs are not copied from the original location.     

### Testing the configuration
There are methods that can be used to confirm that the user account has sufficient rights.
These are 
 - Configure and start the service as the user and then check the log files.   
 - Interactively run ServiceControl as the user

#### Method 1 : Running the service as non-privileged user 
Open computer management
Change the service Account to the non-privileged user and password and apply the change.
The user account will then be given the "logon as a service privilege".
Start the service
Confirm that the service started 

Examine the log file to ensure that the service is operating as expected. If the service is not starting and the log file is not indicating the issue it may be necessary to try Method 2.

#### Method 2 : Running the service interactively as a not-privileged use 
To run the service this way the user account must have rights to logon interactively on the computer.  Firstly logon to the computer with admin privileges and issue the following command after substituting the appropriate domain and user name. Enter the password when prompted

```
e.g.   runas /user:MyDomain\MyTestUser cmd.exe

```
If the command returns the error below then the user account cannot be tested this way without adjusting the logon rights.  Normally this only occurs if the computer is configured as a domain controller or the System Administrator has restricted login access using group policies. 
``` 
1385: Logon failure: the user has not been granted the requested logon type at this computer.
```

Once logon rights are granted then you can proceed. 
Firstly, ensure that the Particular.ServiceControl service is stopped, then from the command prompt running as the service account change to the ServiceControl installation directory and run the ServiceControl.exe. 

```
cd "C:\Program Files (x86)\Particular Software\ServiceControl"
ServiceControl.exe 
```
Examine the output and confirm that no critical errors are shown.
Shutdown the console session and then start the service.

### Expected Warnings when run as non-privileged Account
On service start up the Embedded RavenDB will attempt to create Windows performance counters. This will not succeed and RavenDB performance counters will not be available.
This warning can be safely ignored.
