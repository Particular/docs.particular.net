---
title: Setting a Custom Hostname
summary: How to configure ServiceControl to be exposed through a custom hostname and IP port
tags:
- ServiceControl
- ServicePulse
---

To set a custom hostname and IP port for ServiceControl:

1. Open the ServiceControl configuration file (see [Customizing ServiceControl configuration](creating-config-file.md))  
1. Add the `ServiceControl/Hostname` and `ServiceControl/Port` settings in the `<appSettings>` section. 


The following example configures ServiceControl to listen to localhost + machinename + IP address on port 80:

```xml 
<add key="ServiceControl/Hostname" value="*" />
<add key="ServiceControl/Port" value="33333" />
```

The following example configures ServiceControl to listen to `http://sc.myspecialdomain.com:33333/api`:

```xml
<add key="ServiceControl/Hostname" value="sc.myspecialdomain.com" />
<add key="ServiceControl/Port" value="33333" />
```

NOTE: You must set both the `ServiceControl/Hostname` and the `ServiceControl/Port`, even if the value of one remains unchanged.

### Updating URLACL Settings

1. After modifying the config file, update the URLACL configurations accordingly. 
1. Allow access to the ports specified for ServiceControl hostname & port number by configuring firewalls.

For example, the following command line (with the appropriate adjustments for your hostname and port number) adds the relevant URLACL setting:  

`Netsh http add urlacl  url=http://*:33333/  user=everyone  listen=yes`

NOTE: Ensure that there is only one URLACL defined.

#### Security considerations

URL ACL defines which user or security group can start to listen for incoming requests on the configured Url. In the above sample `everyone` is simply for demonstration purpose. Be sure to configure your URL ACL based on security policies defined in your environment.

ServiceControl requires that the user running the Windows Service process is allowed, by the URL ACL setting above, to listen to the configured URL.

### Configuring ServiceControl to Use a Virtual Directory

You can customize ServiceControl to expose the API endpoint under a custom virtual directory at the configured URL. To customize the Virtual Directory, add a new setting to the ServiceControl configuration file:

`<add key="ServiceControl/VirtualDirectory" value="MyFolder" />`

After restarting the ServiceControl service, invoke the API by issuing a request to the following URL: `http://localhost:33333/MyFolder/API/`

NOTE: The above sample illustrates simplified and default non-secure settings. Apply security and authentication restrictions based on specific usage restrictions and policies. 

### Updating ServicePulse Configuration to ServiceControl Custom Hostname

1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\config.js`.
1. Update the value of the `service_control_url` parameter to the specified ServiceControl hostname and IP port number.
1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings. 