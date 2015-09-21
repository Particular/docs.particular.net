---
title: Installing Multiple Instances of ServiceControl on a Host 
summary: Describes the process to install multiple instances of ServiceControl on a single host
tags:
- ServiceControl
- Installation
---

The ServiceControl MSI does not provide an option to install additional instances. Instead, do it manually once you have installed an initial copy of ServiceControl.


## Assumptions

To illustrate the process this procedure assumes the following:
-  An existing "default" instance of ServiceControl has been installed using the default settings. 
-  A second instance of ServiceControl is to be added using the Window service name "Particular.ServiceControl.Test" 
-  The second instance URI will be `http://localhost:33334/api`.


## Adding the Instance 


### Copying the Binaries 

To add a second instance, take a copy of the ServiceControl directory. This is necessary because ServiceControl maintains its configuration in the ServiceControl.exe.config, which is located in the same directory as the binaries.
For this example we assume the new directory will be `C:\Program Files (x86)\Particular Software\ServiceControl-Test`.
After copying the directory, ensure the file system ACLs are correct and that the service account you plan to use has read and execute rights.  


### Registering the Service

This example shows the commands to register the second instance.  These commands must be executed in a command prompt that is running with administrator privileges: 
  
```
CD "C:\Program Files (x86)\Particular Software\ServiceControl-Test"
ServiceControl.exe --install  --serviceName="Particular.ServiceControl.Test" --displayName="Particular.ServiceControl.Test" --d=ServiceControl/Hostname==localhost  --d=ServiceControl/port==33334
```

As shown, you must specify the command line options for the host name and port when registering an additional instance. If not, the installation will default to port 33333 and 'localhost', which will clash with the default installation.  The install option will run the endpoint briefly to create the required message queues and register the appropriate URLACL using the Netsh command.  Refer to [Setting a Custom Hostname](setting-custom-hostname.md) for more details on setting the URLACL. 


### Specifying the Service Account

The command line arguments shown above will set the service account for "Particular.ServiceControl.Test to "Local System".  You can specify a service account on the command line using the username and password command line switches as follows:

```
CD "C:\Program Files (x86)\Particular Software\ServiceControl-Test"
ServiceControl.exe --install  --username="corp\serviceuser" --password="p@ssw0rd!" --serviceName="Particular.ServiceControl.Test" --displayName="Particular.ServiceControl.Test" --d=ServiceControl/Hostname==localhost  --d=ServiceControl/Port==33334
```

See [Configuring a non-privileged Service Account](configure-non-privileged-service-account.md) for information on additional steps that may be required to use a custom service account.   


### Updating Error and Audit Queues

The "install" option on the command line creates a minimum configuration file.  Update this file to specify the error and audit queues to monitor. Details on modifying the configuration settings are in  [Customizing ServiceControl configuration](creating-config-file.md).
 

### Logging Location

The default install of ServiceControl logs to `%LOCALAPPDATA%\Particular\ServiceControl\logs`.

For additional instances, the logging location changes, and the "ServiceControl" portion of the path is replaced with the Windows service name. So assuming the command lines above were used to register an addition instance, the path to that instance's logs is `%LOCALAPPDATA%\Particular\Particular.ServiceControl.Test\logs`. 

You can override this logging location in the configuration file. See [ServiceControl Logging](logging.md). 


### Upgrading Multiple Instances 

When upgrading any host machine with multiple instances of ServiceControl to V1.2.3 or greater, you need to uninstall and re-install any manually installed instances of ServiceControl.

First, uninstall the instance using this:

```
ServiceControl.exe --uninstall --serviceName="<serviceName>"
```

Then re-register the service.

This is required because:

1. The command line used to run the service has changed as of V1.2.3. 
2. Additional instances now use their own queues, which are created via the install command. The MSMQ queue names used by the ServiceControl instance are the same as the Windows service name.