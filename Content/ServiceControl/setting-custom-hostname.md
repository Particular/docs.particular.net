---
title: Setting custom hostname for ServiceControl
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
tags:
- ServiceControl
- Configuration
---

1. Open the ServiceControl configuration file
   * By default it is located in ```[Program Files]\Particular Software\ServiceControl\ServiceControl.dll.config```  
* Under the ```<appSettings>``` section, add the ```ServiceControl/Hostname``` and ```ServiceControl/Port``` settings
* After modifying the config file, update the urlacl configurations, accordingly (see examples below) 
* Configure relevant firewalls to allow access to the ports specified for ServiceControl hostname & port number.
* Update ServicePulse configuration file to access the updated ServiceControl hostname & port number
   * By default, the ServicePulse configuration file is located in ```[Program Files]\Particular Software\ServicePulse\app\config.js```
   * update the value of ```service_control_url``` parameter to the specified ServiceControl hostname and IP port number
   * When next accessing ServicePulse, make sure to refresh the browser cache in order to allow ServicePulse to access the updated configuration settings 

### Examples:

#### Updating the config settings:

* The following example configures ServiceControl to listen on localhost + machinename + IP address on port 80:

```xml 
<add key="ServiceControl/Hostname" value="*" />
<add key="ServiceControl/Port" value="80" />
```

* The following example configures ServiceControl to listen on http://sc.myspecialdomain.com:8080/api:


```xml
<add key="ServiceControl/Hostname" value="sc.myspecialdomain.com" />
<add key="ServiceControl/Port" value="8080" />
```

#### Updating the urlacl configurations 

* Add  URLACL rule using command line 

```
Netsh http add urlacl  url=http://*:33333/  user=everyone  listen=yes
```
