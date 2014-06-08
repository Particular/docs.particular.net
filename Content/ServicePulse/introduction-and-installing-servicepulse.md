### Introduction

Particular Software ServicePulse is the new operational monitoring tool for distributed applications developed using NServiceBus. 

[An Introduction to ServicePulse for NServiceBus](http://particular.net/blog/an-introduction-to-servicepulse-for-nservicebus) provides a short ~7 minute introductory video and demo of ServicePulse capabilities and main features for this release.

### Prerequisites

* .NET Framework 4.5 or later
* Particular Software ServiceControl 1.0.0
* Monitored NServiceBus endpoints must use NServiceBus 4.0.0 or higher
   * Support for NServiceBus 3.3.x can be obtained by contacting the [Particular Software](http://particular.net/support) support

### Installation

1. Download and install [ServiceControl 1.0.0](https://github.com/Particular/ServiceControl/releases/tag/1.0.0)
* Download and install ServicePulse 1.0.0
* The following installation parameters are used by the ServicePulse installer:
   * ServicePulse Instance URI: 
      * Default is [http://localhost:9090](http://localhost:9090)
      * This is the localhost address and port that will be used for accessing the ServicePulse web application
   * ServiceControl instance URI: 
      * Default is [http://localhost:33333/api/](http://localhost:33333/api/)
      * The URI that will be accessed by ServicePulse web app in order to communicate with the ServiceControl instance
      * Review [ServiceControl 1.0.0 release notes](https://github.com/Particular/ServiceControl/releases/tag/1.0.0) for more details on installing and configuring ServiceControl for use by ServicePulse
* After accepting the license terms and conditions, click "Install" (installer will require elevated privileges)
* The installation process performs the following actions:
   * Installs the Windows Service "Particular ServicePulse" which hosts the web application
   * Open the ServicePulse web application using the default browser
* After installing ServicePulse, see the following sections to configure your existing endpoints to be monitored via ServicePulse. 


### Troubleshooting: 

See [Troubleshooting Guide](Troubleshooting)
