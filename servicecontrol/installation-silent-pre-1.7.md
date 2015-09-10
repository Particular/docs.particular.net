---
title: Silent Installation Options prior to v1.7
summary: Silent Installation Options for older version of ServiceControl
tags:
- ServiceControl
- Installation
---

### Silent Installation options for older versions of ServiceControl 

The silent install instructions for the latest version can be found [here](installation-silent.md)   

#### Silent Installation Instructions for Versions 1.0 through 1.4.x

The following command line show how to silently install ServiceControl V1.0 through to V 1.4.x

```bat
Particular.ServiceControl.1.4.0.exe /quiet 
```

#### Silent Instructions for versions 1.6.x and 1.5.x

For V1.5.x and V1.6.x the silent install command line must also include the ForwardAuditMessages property, this corresponds to the `ServiceControl/ForwardAuditMessages` setting found in the configuration settings.  Refer [Customizing ServiceControl Configuration](creating-config-file.md). 

The following example enables audit forwarding.

```bat
Particular.ServiceControl.1.5.0.exe /quiet ForwardAuditMessages=true
```

The `ForwardAuditMessages` command line property is not mandatory if an upgrade is being carried out and the existing application configuration file has the `ServiceControl/ForwardAuditMessages` defined and set to true or false.  

Passing the `ForwardAuditMessages` property has no affect when the installer is not running silently  

NOTE: It is recommended to enable logging when running in silent mode as error messages are suppressed. See Troubleshooting


#### Silent UnInstallation 1.0 through to 1.6.x

The following command can be used to uninstall ServiceControl silently:

```bat
wmic product where (name like '%servicecontrol%') call uninstall
```

#### Troubleshooting

The installer will pass any [MSIEXEC command line switches](https://technet.microsoft.com/en-us/library/cc759262%28v=ws.10%29.aspx) through when it is launched.

A typical command line for enabling verbose MSI logging when running interactively would be:

```bat
Particular.ServiceControl.1.5.0.exe /LV* install.log  
```

A typical command line for enabling verbose MSI logging when running silently would be:

```bat
Particular.ServiceControl.1.5.0.exe /quiet /LV* install.log ForwardAuditMessages=true
```