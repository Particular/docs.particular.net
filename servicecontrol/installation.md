---
title: Installing ServiceControl
summary: Installation Options for ServiceControl
tags:
- ServiceControl
- Installation 
---

The ServiceControl installation file consists of an embedded MSI bootstrapper EXE and an embedded MSI.  This installation can be executed standalone or via the Particular Software Platform Installer.
 
### Prerequisites
The ServiceControl Installation has the following prerequisites:

1. The Microsoft .Net 4.5 Runtime 
2. The Microsoft Message Queueing (MSMQ) service
3. NServiceBus Performance Counters

If ServiceControl is installed via the Particular Software Platform Installer then the installation and configuration of these pre-requisites are managed for you. 


#### MSMQ 
ServiceControl is configured to use MSMQ as the out of the box transport. Even if you intend to use an alternative transport as detailed in [Multi Transport Support](multi-transport-support.md) you must have the MSMQ service running for the installation to succeed.    
      
To enabling and configuring MSMQ without the Platform Installer refer to the MSMQ section off [Installing The Platform Components Manually](/platform/installer/offline.md#msmq)

Alternatively, if [Chocolatey](https://chocolatey.org) is installed you can install and configure MSMQ by running the following Chocolatey command:

	cinst NServicebus.Msmq.install

#### Performance Counter 
ServiceControl reports metrics via the NServiceBus Performance Counters.     
      
For instructions on how to install the Performance Counters without the Platform Installer refer to [Installing The Platform Components Manually](/platform/installer/offline.md)

Alternatively, if [Chocolatey](https://chocolatey.org) is installed you can install the performance counters running the following Chocolatey command:

	cinst NServicebus.PerfCounters.install

## Silent Installation

Prior to V1.5 of ServiceControl a silent install required the standard MSI command lines switches such of '/qn' or '/quiet' to suppress the installer UI.  

```bat
Particular.ServiceControl.1.4.0.exe /quiet
```

From V1.5 the silent install command line must also include the ForwardAuditMessages property, this corresponds to the "ServiceControl/ForwardAuditMessages" setting found in the configuration settings.  Refer [Customizing ServiceControl Configuration](creating-config-file.md). The following example enables audit forwarding.

```bat
Particular.ServiceControl.1.5.0.exe /quiet ForwardAuditMessages=true 
```

The ForwardAuditMessages command line property is not mandatory if an upgrade is being carried out and the existing application configuration file has the "ServiceControl/ForwardAuditMessages" defined and set to true or false.  

Passing the ForwardAuditMessages property has no affect when the installer is not running silently  

NOTE: It is recommended to enable logging when running in silent mode as error messages are suppressed. See Troubleshooting 

#### Troubleshooting 

The installer will pass any [MSIEXEC command line switches](https://technet.microsoft.com/en-us/library/cc759262%28v=ws.10%29.aspx) through when it is launched. 

A typical command line for enabling verbose MSI logging when running interactivly would be: 

```bat
Particular.ServiceControl.1.5.0.exe /LV* install.log  
``` 

A typical command line for enabling verbose MSI logging when running silently would be:

```bat
Particular.ServiceControl.1.5.0.exe /quiet /LV* install.log ForwardAuditMessages=true 
```

    
