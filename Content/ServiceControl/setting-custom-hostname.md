---
title: Setting a Custom Hostname for ServiceControl
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
tags:
- ServiceControl
- ServicePulse
- Configuration
---

### To set a custom hostname and IP port for ServiceControl

The following example configures ServiceControl to listen to localhost + machinename + IP address on port 80:
```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d=ServiceControl/Hostname==* -d=ServiceControl/Port==80
```

The following example configures ServiceControl to listen to `http://sc.myspecialdomain.com:8080/api`:
```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d="ServiceControl/Hostname==sc.myspecialdomain.com" -d=ServiceControl/Port==8080
```

<p class="alert alert-info">
<strong>ServiceControl default installed location</strong><br/>
By default ServiceControl is installed in [Program Files]\Particular Software\ServiceControl.
</p>

### Updating URLACL Settings

1. After modifying the config file, update the URLACL configurations accordingly. 
1. Allow access to the ports specified for ServiceControl hostname & port number by configuring firewalls.

For example, the following command line (with the appropriate adjustments for your hostname and port number) adds the relevant URLACL settting:  

`Netsh http add urlacl  url=http://*:33333/  user=everyone  listen=yes`

### Configuring ServiceControl to Use a Virtual Directory

You can customize ServiceControl to expose the API endpoint under a custom virtual directory at the configured URL. To customize the Virtual Directory, add a new setting to the ServiceControl configuration file:
```bat
x:\Your_Installed_Path\ServiceControl.exe --restart -d="ServiceControl/VirtualDirectory==MyFolder"
```

<p class="alert alert-info">
<strong>ServiceControl default installed location</strong><br/>
By default ServiceControl is installed in [Program Files]\Particular Software\ServiceControl.
</p>

After ServiceControl service restarts, invoke the API by issuing a request to the following URL: `http://localhost:33333/MyFolder/api/`

<p class="alert alert-warning">
<strong>Note</strong><br/>
The above sample illustrates simplified and default non-secure settings. Apply security and authentication restrictions based on specific usage restrictions and policies.
</p>

### Updating ServicePulse configuration to connect to a ServiceControl with a custom hostname

1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\config.js`.
1. Update the value of the `service_control_url` parameter to the specified ServiceControl hostname and IP port number.
1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings. 


