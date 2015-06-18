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


The following example configures ServiceControl to listen to on port 33333.  The HostName value in this example is a wildcard. This instructs ServiceControl to respond to requests on all resolvable names for the host ( hostname, IP Address, DNS aliases etc). This change must also be reflected via a URLACL change (see below) 

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

1. After modifying the configuration file, update the URLACL configurations accordingly. 
1. Allow access to the ports specified for ServiceControl hostname & port number by configuring firewalls.

For example, the following command line (with the appropriate adjustments for your hostname and port number) adds the relevant URLACL setting:  

`Netsh http add urlacl  url=http://*:33333/api/  user=everyone  listen=yes`

Listing the current URLACLs can be done using the following command:    

`Netsh http list urlacl`

Ensure that there are no overlapping URLACLs as this can cause ServiceControl to fail on service startup.  For example, if the list command yielded results for `http://localhost:33333/api/` and `http://*:33333/api/` then it is not clear which URL ServiceControl should use.

In the following example the wildcarded URLACL is removed:

`Netsh http delete urlacl  url=http://*:33333/api/` 

#### Security considerations

NOTE: Anyone who can access the ServiceControl URL has complete access to the message data stored within ServiceControl.  This is why by default ServiceControl is configured to only respond to the localhost.  Please consider carefully the implications of exposing ServiceControl via a custom or wildcard URL

It is important to understand that URLACLs do not restrict access to the URL based on the identity of the requestor, what they do is to restrict which user or security group can start to listen for incoming requests on the configured URL. 

In the above sample `everyone` is for demonstration purpose. Be sure to configure your URLACL based on security policies defined in your environment.

ServiceControl will not start if the service account does not have access to listen on the URL specified in the configuration file.


### Configuring ServiceControl to Use a Virtual Directory

You can customize ServiceControl to expose the API endpoint under a custom virtual directory at the configured URL. To customize the Virtual Directory, add a new setting to the ServiceControl configuration file:

`<add key="ServiceControl/VirtualDirectory" value="MyFolder" />`

After restarting the ServiceControl service, invoke the API by issuing a request to the following URL: `http://localhost:33333/MyFolder/API/`

### Updating ServicePulse Configuration to ServiceControl Custom Hostname

1. Update the ServicePulse configuration file to access the updated ServiceControl hostname & port number. By default, the ServicePulse configuration file is located in `[Program Files]\Particular Software\ServicePulse\app\config.js`.
1. Update the value of the `service_control_url` parameter to the specified ServiceControl hostname and IP port number.
1. When next accessing ServicePulse, make sure to refresh the browser cache to allow ServicePulse to access the updated configuration settings. 