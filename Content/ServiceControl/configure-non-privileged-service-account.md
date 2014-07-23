---
title: Configuring a non-privileged Service Account
summary: Describes the changes made to allow ServiceControl V1.2 to run as a low privilege domain account
tags:
- ServiceControl
---

The installation of ServiceControl 1.2 sets up the service to run as under the LocalSystem account.  This configuration works with the default transport (MSMQ) but when working with other transports such as SQL it is preferable to change the service account to a names user account to allow Windows intergrated credential to be used passed to SQL Server. 

Prior to 1.2 of ServiceControl it was possible to change the account to a user account but that account was required to be have local administrator privileges.   In version 1.2 changes have been made to allow a lower privilege domain account to operate as the service account. 

Specifically the changes made were:

- the URLACL registration is only attempted if the service account has Administrator privileges
- the file system ACLs on the folder containing the embedded RavenDB have been changed to allow read/write access by members of the "Users" group
- The default URLACL registration has been changed so members of the Windows "Users" group are authorized to listen on the endpoint URL

### Required Manual configuration
Even with the changes made in v1.2 there are some manual configuration steps that need to be completed to allow a non-privileged account to function. 

In the case of MSMQ, the ACL defaults for a message queues default is to allow Administrators to have full access.  Switching to a low privileged account means that the rights need to be modified to give the account access.

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
If the configuration of ServiceControl is manually changed to listen to an alternate URL as detailed in  [Customizing ServiceControl configuration](creating-config-file) then the  URLACL used should reflect the user account assigned to run the service.  If this is incorrect the service will not start.

### RavenDB Security
The installer will set the permissions to allow any member of the local Windows Users group to have modify rights to files in the embedded Raven DB folder.  These right can be changed manually ot be more restrictive as long as the service account user retains modify rights.  Manual changes to the ACLs may be lost during an upgrade or re-installation of ServiceControl.  

### Testing the configuration
To confirm the configuration is correct is to run ServiceControl interactively from the command line.
**
Before doing this ensure the ServiceControl service is stopped
**

Open a command prompt using "RunAs" as the service account user
```
e.g.   runas /user:MyDomain\MyTestUser cmd.exe

```
Then from the command prompt change to the ServiceControl installation directory and run the ServiceControl.exe. 

```
cd "C:\Program Files (x86)\Particular Software\ServiceControl"
ServiceControl.exe 
```
Examine the output when and confirm that no errors are shown.

Shutdown the console session and then start the service.




