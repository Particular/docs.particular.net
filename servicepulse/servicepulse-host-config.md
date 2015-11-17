---
title: Configuring ServicePulse Host
summary: Describes how to configure the ServicePulse Host
tags:
- ServicePulse
---

To modify the port used by ServicePulse the commandline specified in the registry must be updated.  

To change it:

1. Open Regedit.exe
2. Goto `HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Particular.ServicePulse`
3. Edit the value of `ImagePath`. The value contains the full path to the ServicePulse.exe and a commandline of the URL to use:  

The default value for ImagePath is:

`"C:\Program Files (x86)\Particular Software\ServicePulse\ServicePulse.Host.exe" --url="http://localhost:9090"`

Change the value after `--url=` and restart the service.


## Changing the ServiceControl URL

ServicePulse is configured to connect to the ServiceControl REST API.  To specify the URL to connect do  the following

### For Versions before 1.3.0

- Go to the installation folder for ServicePulse.  Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the `app` folder and edit `config.js`
- Change the value of the url after `service_control_url`

### For Versions 1.3.0 and after

- Go to the installation folder for ServicePulse.  Typically this is `C:\Program Files (x86)\Particular Software\ServicePulse\`
- Go to the `app\js` folder and edit `app.constants.js`
- If there are any `*.js` in the `app` folder delete them
- Change the value of the url after `service_control_url`


## ServicePulse commandline configuration

Below is the commandline help output of `ServicePulse.Host.exe`.

NOTE: Commandline arguments are case-sensitive.

```
USAGE:
   ServicePulse.Host.exe --install [options]
   ServicePulse.Host.exe --uninstall [options]
   ServicePulse.Host.exe --extract [options]
   ServicePulse.Host.exe [--url="http://localhost:9090"] [--serviceControlUrl="http://localhost:8080/api"]

INSTALL OPTIONS:

  -?, -h, --help             Help about the command line options.
  -i, --install              Install the endpoint as a Windows service.
      --serviceName=VALUE    Specify the service name for the installed
                               service.
      --displayName=VALUE    Friendly name for the installed service.
      --description=VALUE    Description for the service.
      --username=VALUE       Username for the account the service should run
                               under.
      --password=VALUE       Password for the service account.
      --localservice         Run the service with the local service account.
      --networkservice       Run the service with the network service
                               permission.
      --user                 Run the service with the specified username and
                               password. Alternative the system will prompt for
                               a valid username and password if values for both
                               the username and password are not specified.
      --delayed              The service should start automatically (delayed).
      --autostart            The service should start automatically (default).
      --disabled             The service should be set to disabled.
      --manual               The service should be started manually.
      --serviceControlUrl=VALUE
                             Configures the service control url.
      --url=VALUE            Configures ServicePulse to listen on the
                               specified url.


UNINSTALL OPTIONS:

  -?, -h, --help             Help about the command line options.
  -u, --uninstall            Uninstall the endpoint as a Windows service.
      --serviceName=VALUE    Specify the service name for the installed
                               service.


EXTRACT OPTIONS:

  -?, -h, --help             Help about the command line options.
  -e, --extract              Extract files to be installed in a Web Server.
      --serviceControlUrl=VALUE
                             Configures the service control url.
      --outPath=VALUE        The output path to extract files to. By default
                               it extracts to the current directory.


EXAMPLES:
   ServicePulse.Host.exe --install
     --serviceName="MyServicePulse"
     --displayName="My Pulse"
     --description="Service for monitoring"
     --username="corp\serviceuser"
     --password="p@ssw0rd!"
         --url="http://localhost:9090"
         --serviceControlUrl="http://localhost:8080/api"

   ServicePulse.Host.exe --uninstall --serviceName="MyServicePulse"

   ServicePulse.Host.exe --extract --serviceControlUrl="http://localhost:8080/api"
```
