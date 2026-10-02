---
title: Installing ServicePulse using the standalone installer
summary: Describes how to install ServicePulse
component: ServicePulse
reviewed: 2026-10-01
related:
- servicepulse/troubleshooting
redirects:
- servicepulse/installation
---

> [!IMPORTANT]
> This guide explains how to install ServicePulse using the standalone installer. It is also possible to run ServicePulse [in ServiceControl](/servicecontrol/servicecontrol-instances/integrated-servicepulse.md) or in a [container](containerization).

## Prerequisites

 * .NET Framework 4.8 or later
 * A currently-supported version of:
     - Microsoft Edge
     - Chrome
     - Firefox
     - Safari
 * A running error instance of [ServiceControl](/servicecontrol)


## Installation

 1. Download the latest version of [ServicePulse](https://github.com/Particular/ServicePulse/releases).
 1. Run the installer using elevated privileges.
 1. After accepting the license terms and conditions, click "Install" 
 1. The installation process creates the `Particular ServicePulse` Windows service and opens the `ServicePulse` web application using the default browser.

### Available installation parameters

The executable accepts some parameters to customize the installation experience.

- `Quiet`: Allows ServicePulse to be installed in the background. The installation user interface will be unavailable.
- `Log [file location]`: Provides the location on disk for the log file to be generated.
- `INST_PORT_PULSE [port number]`: Provides the port number that ServicePulse web application will run on. The default value is 9090.
- `INST_URI [uri]`: Provides location of the ServiceControl Instance API. The default value is: `http://localhost:33333/api/`.
- `INST_SC_MONITORING_URI [uri]`: Provides location of the Monitoring Instance API. The default value is: `http://localhost:33633/`.

For example:
```
.\Particular.ServicePulse.exe /Quiet /Log C:\temp\servicepulse-installer.log INST_PORT_PULSE=12345 INST_URI=http://localhost:33333/api/ INST_SC_MONITORING_URI=http://localhost:33633/
```

## Configuring ServicePulse

See [connection configuration in ServicePulse](/servicepulse/host-config.md#configuring-connections-via-the-servicepulse-ui) for ServicePulse configuration options.

## Moving ServicePulse to a new location

ServicePulse does not store any message data, so the only information that needs to be moved is its configuration.
By default, ServicePulse is installed in `C:\Program Files (x86)\Particular Software\ServicePulse` and its configuration values are stored in the file `\app\js\app.constants.js` relative to the installation folder. 

In order to move ServicePulse to a new location, the ServicePulse installer must be run there and then the `\app\js\app.constants.js` file should be copied over.

## ServicePulse license

ServicePulse will check the current licensing status by querying the ServiceControl error instance. If ServicePulse indicates that the license is invalid or has expired, then the [license must be updated in ServiceControl](/servicecontrol/license.md).
