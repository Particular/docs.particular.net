---
title: Installing ServicePulse
summary: Describes how ServicePulse is installed - and its basic requirements
component: ServicePulse
reviewed: 2020-02-24
related:
- servicepulse/troubleshooting
---


## Prerequisites

 * .NET Framework 4.5 or later
 * A currently-supported version of Microsoft Edge, Chrome, Firefox, or Safari
 * A running instance of [ServiceControl](/servicecontrol)

## Installation

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
.\Particular.ServicePulse.exe /Quiet /Log C:\temp\servicepulse-installer.log INST_PORT_PULSE=12345 INST_URI=http://localhost:33333/api/ INST_SC_MONITORING_URI=http://localhost:33633/
```

## Configuring ServicePulse

ServicePulse connects to and relies on ServiceControl and optionally ServiceControl Monitoring as its data source.
For details on ServiceControl, ServiceControl Monitoring and ServicePulse configuration options, see:

 * [Configuring ServiceControl](/servicecontrol/creating-config-file.md)
 * [ServiceControl Guidance](/servicecontrol)
 * [Configuring ServiceControl Monitoring](/servicecontrol/monitoring-instances/installation/creating-config-file.md)
 * [ServiceControl Monitoring Guidance](/servicecontrol/monitoring-instances)
 * [Connection Configuration in ServicePulse](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui)

NOTE: ServiceControl consumes messages from the Audit queue and stores it temporarily (by default, for 30 days) in its embedded database. Set the message storage timespan by [setting automatic expiration for ServiceControl data](/servicecontrol/how-purge-expired-data.md).

## Migrating / Moving

ServicePulse does not contain any message data; it has only a few configuration values stored in the following file `\app\js\app.constants.js`. By default ServicePulse is intalled in `C:\Program Files (x86)\Particular Software\ServicePulse`.

Run the ServicePulse installer on the new server manually or via [scripting powershell or a batch file](#installation-available-installation-parameters) and copy the `\app\js\app.constants.js` to the new location.

## ServicePulse license

ServicePulse will check the current licensing status by querying the ServiceControl API, located by default at `http://localhost:33333/api`. If ServicePulse indicates that the license is invalid or has expired, then the [license must be updated in ServiceControl](/servicecontrol/license.md).
