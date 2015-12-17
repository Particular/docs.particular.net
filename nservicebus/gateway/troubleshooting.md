---
title: Gateway Troubleshooting
summary: Gateway Troubleshooting
tags: []
redirects:
 - nservicebus/gateway-trouble-shooting
---

## Corrupted urlacls

At startup NServicebus will attempt to recreate the urlacls so that the process can listen on a specific port. If it already exists we log a warning and continue on. In this case you will see something like this in your log file.

```
WARN NServiceBus.Installation.GatewayHttpListenerInstaller
Failed to grant to grant user 'Machine\UserName' HttpListener permissions. Processing will continue.
Try running the following command from an admin console:
netsh http add urlacl url=http://localhost:25894/MyEndpoint/ user="Machine\MyEndpointServiceAccount"

The error message from running the above command is:
Url reservation add failed, Error: 183
Cannot create a file when that file already exists.
```

This warning can usually be safely ignored. However when urlacls are corrupted the same behaviors occurs. This problem will manifest in that everything seems to be working correctly except that the gateway will not be listening on the port.

To verify this problem you can navigate to the url (in this case `http://localhost:25894/MyEndpoint/`) in a browser. If the gateway is listening you will receive

    EndpointName:MyEndpoint - Status: Ok

If the gateway is not listening you will get no response.

To clean an recreate the url acl manuall from an admin console run the following commans

    netsh http delete urlacl YourUrl
    netsh http add urlacl url=YourUrl user="YourMachine\EndpointUsername"

For example

    netsh http delete urlacl http://localhost:25894/MyEndpoint/
    netsh http add urlacl url=http://localhost:25894/MyEndpoint/ user="Machine\MyEndpointServiceAccount"

Also to list all urlacls on a machine you can run

    netsh http show urlacl

For more information see the relevant [Netsh commands for HTTP
](https://msdn.microsoft.com/en-us/library/windows/desktop/cc307236)
