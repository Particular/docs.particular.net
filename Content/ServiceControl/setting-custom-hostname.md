---
title: Setting a Custom Hostname for ServiceControl
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
tags:
- ServiceControl
- ServicePulse
- Configuration
---

To set a custom hostname and IP port for ServiceControl:

1. Open the ServiceControl configuration file. By default it is located in ```[Program Files]\Particular Software\ServiceControl\ServiceControl.dll.config```  
1. Add the ```ServiceControl/Hostname``` and ```ServiceControl/Port``` settings under the ```<appSettings>``` section. 


The following example configures ServiceControl to listen on localhost + machinename + IP address on port 80:

```xml 
<add key="ServiceControl/Hostname" value="*" />
<add key="ServiceControl/Port" value="80" />
```

The following example configures ServiceControl to listen on http://sc.myspecialdomain.com:8080/api:

```xml
<add key="ServiceControl/Hostname" value="sc.myspecialdomain.com" />
<add key="ServiceControl/Port" value="8080" />
```

**NOTE:** Both the ```ServiceControl/Hostname``` and the ```ServiceControl/Port``` must be set, even if the value of one remains unchanged.


### Update URLACL settings

1. After modifying the config file, update the URLACL configurations accordingly. 
1. Allow access to the ports specified for ServiceControl hostname & port number by configuring firewalls.

For example, the following command line (with the appropriate adjustments to your hostname and port number) adds the relevant URLACL settting:  

```
Netsh http add urlacl  url=http://*:33333/  user=everyone  listen=yes
```


### Update ServicePulse configuration to ServiceControl custom hostname

1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in ```[Program Files]\Particular Software\ServicePulse\app\config.js```.
1. Update the value of the ```service_control_url``` parameter to the specified ServiceControl hostname and IP port number.
1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings. 


