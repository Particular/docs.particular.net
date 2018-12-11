---
title: Installing ServicePulse
summary: Describes how ServicePulse is installed - independently or via the PlatformInstaller - and its basic requirements
component: ServicePulse
reviewed: 2018-04-24
tags:
- Installation
related:
- servicepulse/troubleshooting
---


## Prerequisites

 * .NET Framework 4.5 or later
 * A currently-supported version of Internet Explorer, Chrome, Firefox, or Safari
 * A running instance of [ServiceControl](/servicecontrol)


## Installation

Install ServicePulse using the [Particular Service Platform Installer](/platform/installer) (recommended) or independently using the following procedure:

 1. Download and install [ServiceControl](https://github.com/Particular/ServiceControl/releases)
 1. Download and install [ServicePulse](https://github.com/Particular/ServicePulse/releases)
 1. After accepting the license terms and conditions, click "Install" (the installer will require elevated privileges)
 1. The installation process installs the "Particular ServicePulse" Windows service and opens the ServicePulse web application using the default browser.
 1. After installing ServicePulse, see the following sections to configure the existing endpoints to be monitored via ServicePulse.

### Available installation parameters

- `Quiet`
Allows ServicePulse to be installed in the background. The installation user interface will be unavailable.
- `Log [file location]`
Provides the location on disk for the logfile to be generated.
- `INST_PORT_PULSE [port number]`
Provides the port number that ServicePulse web application will run on.
Default : 9090
- `INST_URI [uri]`
Provides location of the ServiceControl Instance API
Default : `http://localhost:33333/api/`
- `INST_SC_MONITORING_URI [uri]` 
Provides location of the Monitoring Instance API
Default : `http://localhost:33633/`

Example
```
.\Particular.ServicePulse.exe /Quiet /Log C:\temp\servicepulse-installer.log INST_PORT_PULSE=12345 INST_URI=http://localhost:33333/api/
```

## Configuring ServicePulse

ServicePulse connects to and relies on ServiceControl as its data source.
For details on ServiceControl and ServicePulse configuration options, see:

 * [Configuring ServiceControl](/servicecontrol/creating-config-file.md)
 * [ServiceControl Guidance](/servicecontrol)

NOTE: ServiceControl consumes messages from the Audit queue and stores it temporarily (by default, for 30 days) in its embedded database. Set the message storage timespan by [setting automatic expiration for ServiceControl data](/servicecontrol/how-purge-expired-data.md).


## ServicePulse license

ServicePulse will check the current licensing status by querying the ServiceControl API, located by default at `http://localhost:33333/api`. If ServicePulse indicates that the license is invalid or has expired, then the [license must be updated in ServiceControl](/servicecontrol/license.md).
