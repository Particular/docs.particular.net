---
title: Installing ServicePulse
summary: Describes how ServicePulse is installed - indepdendently or via the PlatformInstaller - and its basic requirements 
tags:
- ServicePulse
- Installation
---

### Introduction

Particular Software ServicePulse is the operational monitoring tool for distributed applications developed using NServiceBus. 

[An Introduction to ServicePulse for NServiceBus](http://particular.net/blog/an-introduction-to-servicepulse-for-nservicebus) provides a short ~7 minute introductory video and demo of ServicePulse capabilities and main features for this release.

### Prerequisites

* .NET Framework 4.5 or later
* Particular Software ServiceControl 
* Monitored NServiceBus endpoints must use NServiceBus 4.0.0 or higher
   * Support for NServiceBus 3.3.x can be obtained by contacting the [Particular Software support](http://particular.net/support) 

### Installation

You can install ServicePulse using the [Particular Service Platform Installer](/platform/installer) (recommended) or independently using the following procedure:

1. Download and install [ServiceControl](https://github.com/Particular/ServiceControl/releases)
2. Download and install ServicePulse
3. The following installation parameters are used by the ServicePulse installer:
   * ServicePulse Instance URI: 
      * Default is `http://localhost:9090`
      * This is the localhost address and port that will be used for accessing the ServicePulse web application
   * ServiceControl instance URI: 
      * Default is `http://localhost:33333/api/`
      * The URI that will be accessed by ServicePulse web app in order to communicate with the ServiceControl instance
      * Review [ServiceControl release notes](https://github.com/Particular/ServiceControl/releases) for more details on installing and configuring ServiceControl for use by ServicePulse
4. After accepting the license terms and conditions, click "Install" (installer will require elevated privileges)
5. The installation process performs the following actions:
   * Installs the Windows Service "Particular ServicePulse" which hosts the web application
   * Open the ServicePulse web application using the default browser
6. After installing ServicePulse, see the following sections to configure your existing endpoints to be monitored via ServicePulse. 

### Configuring ServicePulse

ServicePulse connects to and relies on ServiceControl as its data source.  
For details on ServiceControl and ServicePulse configuration options, see: 

* [Configuring ServiceControl](/servicecontrol/creating-config-file.md)
* [ServiceControl Guidance](/servicecontrol)
 
NOTE: ServiceControl consumes messages from the Audit queue and stores it temporarily (by default, for 30 days) in its embedded database. You can set the message storage timespan by [setting automatic expiration for ServiceControl data](/servicecontrol/how-purge-expired-data.md).

### Troubleshooting: 

See [Troubleshooting Guide](troubleshooting.md)
