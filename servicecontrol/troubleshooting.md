---
title: Troubleshooting
summary: Troubleshooting ServiceControl installation and common issues
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


### Unable to start Particular.ServiceControl as a standard user

1. If ServiceControl fails to start and it is set to run with a standard user account (no elevated or specific privileges) this may be due to missing access rights to storage directory in which the internal database is located (by default, this is the `C:\ProgramData\Particular` directory
2. To fix this issue:
   * Grant the user read/write access to the `C:\ProgramData\Particular` location
   * Grant the user access rights to run the service listening on the selected domain and port number by running the following commands (replacing the default URL and USERS parameters):
   
```
netsh http delete urlacl url=http://localhost:33333/api/
netsh http add urlacl url=http://localhost:33333/api/ user=<accountname> Listen=yes
```


### Particular.ServiceControl windows service fails to start

* There are various reasons that can cause the ServiceControl windows service fail to start. To narrow down the possible cause, review the ServiceControl logs files located in:
    * `%LOCALAPPDATA%\Particular\ServiceControl\logs` if the issue relates to the ServiceControl installation process;
    * `%WINDIR%\System32\config\systemprofile\AppData\Local\Particular\ServiceControl\logs` if the issue relates to ServiceControl normal operations. Logs location may vary depending on the user that has been configured to run the ServiceControl service, the above one is the one where the LocalSystem user outputs logs information;
* Most common cause is prerequisites installation and configuration issues;


### Particular.ServiceControl fails to start: EsentInstanceUnavailableException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentInstanceUnavailableException` ensure that ServiceControl [database directory](configure-ravendb-location.md), sub-directory and files, is excluded from any anti-virus and anti-maleware real-time and scheduled scan.


### Particular.ServiceControl fails to start: EsentDatabaseDirtyShutdownException

If ServiceControl fails to start and the logs contain a `Microsoft.Isam.Esent.Interop.EsentDatabaseDirtyShutdownException` you need to run Esent Recovery against the ServiceControl database followed by an Esent Repair.

1. Open an elevated command prompt and navigate to the ServiceControl [database directory](configure-ravendb-location.md)
2. Change to the RavenDB directory (the default is `localhost-33333`)
3. Run `esentutl /r RVN /l "logs"` and wait for it to finish
4. Run `esentutl /p Data` and wait for it to finish
5. Restart ServiceControl


### Particular.ServiceControl crashes when hard disk is busy

ServiceControl can run out of memory and crash when the hard drive is busy. When this happens you will probably see the following error in the logs
```
The version store for this instance (0) has reached its maximum size of 511Mb. It is likely that a long-running transaction is preventing cleanup of the version store and causing it to build up in size. Updates will be rejected until the long-running transaction has been completely committed or rolled back.
``` 
You can increase the size of the version store by adding a new appSetting to the ServiceControl configuration file:

`<add key="Raven/Esent/MaxVerPages" value="1024" />`

The value is the size of the version store in MB.
