---
title: Installing multiple instances of ServiceControl on a host 
summary: Describes the process to install multiple instances of ServiceControl on a single host
tags:
- ServiceControl
- Configuration
- installation
---

The ServiceControl MSI does not provide an option to install additional instances. This can be done manually once the initial copy of ServiceControl is installed.

### Assumptions

To illustrate the process this procedure assumes the following:
-  An existing "default" instance of ServiceControl has been installed using the default settings. 
-  A second instance of ServiceControl is to be added using the Window Service Name "Particular.ServiceControl.Test" 
-  The second instance URI will be http://localhost33334/api.


### Adding the Instance 

#### Copy the binaries 
To add a second instance, first take a copy of the ServiceControl directory. This is necessary as ServiceControl maintains it's configuration in the ServiceControl.exe.config, which is collocated with the binaries.
For this process we've assumed the new folder will be C:\Program Files (x86)\Particular Software\ServiceControl-Test
Once you have copied the folder ensure the file system ACLs are correct and that the service account you plan to use has read and execute rights.  

#### Register the Service

The following example shows the commands to register the second instance.  These commands must be executed within a command prompt which is running with administrator privileges. 
  
```
CD "C:\Program Files (x86)\Particular Software\ServiceControl-Test"
ServiceControl.exe --install  --serviceName="Particular.ServiceControl.Test" --displayName="Particular.ServiceControl.Test" --d=ServiceControl/Hostname==localhost  --d=ServiceControl/port==33334
```

As shown in the example, it is important to specify the command line options for the host name and port when registering an additional instance.  If these options are not specified the installation will default to port 33333 and 'localhost' which will clash with the default install.  The install option will run the endpoint briefly to create the required message queues and register the appropriate URLACL using the Netsh command.  Refer to [Setting a Custom Hostname](setting-custom-hostname) for more details on setting the URLACL. 


#### Specifying the Service Account
The command line arguments shown above will set the service account for "Particular.ServiceControl.Test to be "Local System".  You can specify a service account on the command line using the username and password command line switches as follows:

```
CD "C:\Program Files (x86)\Particular Software\ServiceControl-Test"
ServiceControl.exe --install  --username="corp\serviceuser" --password="p@ssw0rd!" --serviceName="Particular.ServiceControl.Test" --displayName="Particular.ServiceControl.Test" --d=ServiceControl/Hostname==localhost  --d=ServiceControlport==33334
```

Please review [Configuring a non-privileged Service Account](configure-non-privileged-service-account) for information regarding additional steps that may be required to use a custom service account.   
  

#### Error and Audit Queues

The "install" option on the command line creates a minimum configuration file.  This file should be updated to specify the error and audit queues to monitor. Details on modifying the  configuration settings can be found in  [Customizing ServiceControl configuration](creating-config-file)
 

#### Logging Location

The default install of ServiceControl logs to `%LOCALAPPDATA%\Particular\ServiceControl\logs`

For additional instances the logging location is changed and the "ServiceControl" portion of the path is replaced with the Windows Service name.  So assuming the command lines above were used to  register an addition instance the path to that instances logs would be
`%LOCALAPPDATA%\Particular\Particular.ServiceControl.Test\logs` 

This logging location can be overridden via the configuration file. refer to [Configuring the Log Location](setting-custom-log-location) 


#### Upgrading additional instances to v1.2.3

Upgrading to version 1.2.3 from 1.x is not a XCOPY update like previous version as the command line arguments used to launch ServiceControl changed in this version.  When upgrading to 1.2.3 from an earlier version you must uninstall and re-install the additional instances so the correct command line arguments are set.    
  
To uninstall an additional instance use the following command line:

```
ServiceControl.exe --uninstall --serviceName="<instanceName>"
```
