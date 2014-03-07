---
title: Troubleshooting
summary: ServiceControl installation and common issues troubleshooting
tags:
- ServiceControl
- Troubleshooting
---

### Port 33333 is already in use by another service

1. This is most probably due to a previous install of ServiceControl beta (previously known as "Management API").
1. Uninstall previous ServiceControl beta versions by following this process:
1. In the Windows Computer Management > Services check for the existence of a service named "Particular.Management";
1. If there is a service called "Particular.Management" uninstall previous versions of ServiceInsight beta (can be located and uninstalled in Control Panel > Programs as "Particular Software ServiceInsight";
1. If a version of NServiceBus 4.0 or later is already installed, Open its installer modification settings (in Control Panel > Programs select "Change" for the "Particular Software NServiceBus);
1. In the NServiceBus installer click "Modify";
1. Uncheck "Management API" in the list of components to be installed with NServiceBus;
1. Click "Modify" to apply changed and re-run installation;

### Unable to connect to ServiceControl from either ServiceInsight or ServicePulse

1. In Computer Management > Services, check that the Windows Service "Particular ServiceControl" is running
1. In any browser or HTTP client, enter a GET request for the ServiceControl HTTP API (default URI: `http://localhost:33333/api`). 
1. You should get a valid response with JSON data containing ServiceControl default settings.
1. Verify that firewall settings do not block access to the ServiceControl port (default: 33333) and that the default URI is accessible and responsive from a browser / HTTP client from the machine on which ServicePulse or ServiceInsight is trying to connect to ServiceControl


### Particular.ServiceControl windows service fails to start

* There are various options that may cause this. To narrow down the possible causes, review the ServiceControl log file located in: `%LOCALAPPDATA%\Particular\ServiceControl\logs` or `%APPDATA%\Particular\ServiceControl\logs`
* Most common cause is prerequisites installation and configuration issues;
