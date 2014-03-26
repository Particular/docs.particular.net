---
title: Setting a Custom Hostname for ServiceControl
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
tags:
- ServiceControl
- ServicePulse
- Configuration
---

To set a custom hostname and IP port for ServiceControl:

1. Open the ServiceControl configuration file. By default it is located in ```[Program Files]\Particular Software\ServiceControl\ServiceControl.dll.config```.  
1. Add the ```ServiceControl/Hostname``` and ```ServiceControl/Port``` settings in the ```<appSettings>``` section. 


The following example configures ServiceControl to listen to localhost + machinename + IP address on port 80:

```xml 
<add key="ServiceControl/Hostname" value="*" />
<add key="ServiceControl/Port" value="80" />
```

The following example configures ServiceControl to listen to http://sc.myspecialdomain.com:8080/api:

```xml
<add key="ServiceControl/Hostname" value="sc.myspecialdomain.com" />
<add key="ServiceControl/Port" value="8080" />
```

**NOTE:** You must set both the ```ServiceControl/Hostname``` and the ```ServiceControl/Port```, even if the value of one remains unchanged.

### Updating URLACL Settings

1. After modifying the config file, update the URLACL configurations accordingly. 
1. Allow access to the ports specified for ServiceControl hostname & port number by configuring firewalls.

For example, the following command line (with the appropriate adjustments for your hostname and port number) adds the relevant URLACL settting:  

`Netsh http add urlacl  url=http://*:33333/  user=everyone  listen=yes`

### Configuring ServiceControl to Use a Virtual Directory

You can customize ServiceControl to expose the API endpoint under a custom virtual directory at the configured URL. To customize the Virtual Directory, add a new setting to the ServiceControl configuration file:

`<add key="ServiceControl/VirtualDirectory" value="MyFolder" />`

After restarting the ServiceControl service, invoke the API by issuing a request to the following URL: `http://localhost:33333/MyFolder/API/`

**NOTE:** The above sample illustrates simplified and default non-secure settings. Apply security and authentication restrictions based on specific usage restrictions and policies. 

### Updating ServicePulse Configuration to ServiceControl Custom Hostname

1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in ```[Program Files]\Particular Software\ServicePulse\app\config.js```.
1. Update the value of the ```service_control_url``` parameter to the specified ServiceControl hostname and IP port number.
1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings. 


