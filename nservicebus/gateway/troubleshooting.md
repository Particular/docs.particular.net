---
title: Troubleshooting
redirects:
 - nservicebus/gateway-trouble-shooting
---

## Corrupted urlacls

At startup NServiceBus will attempt to recreate the urlacls so that the process can listen on a specific port. If it already exists warning is logged. In this case the following will appear in the configured log.

```no-highlight
WARN NServiceBus.Installation.GatewayHttpListenerInstaller
Failed to grant to grant user 'Machine\UserName' HttpListener permissions. Processing will continue.
Try running the following command from an admin console:
netsh http add urlacl url=http://localhost:25894/MyEndpoint/ user="Machine\MyEndpointServiceAccount"

The error message from running the above command is:
Url reservation add failed, Error: 183
Cannot create a file when that file already exists.
```

This warning can usually be safely ignored. However when urlacls are corrupted the same behaviors occurs. This problem will manifest in that everything seems to be working correctly except that the gateway will not be listening on the port.

To verify this problem navigate to the url (in this case `http://localhost:25894/MyEndpoint/`) in a browser. If the gateway is listening the following will be received:

```no-highlight
EndpointName:MyEndpoint - Status: Ok
```

If the gateway is not listening no response will be received.

To clean an recreate the url acl manually from an admin console run the following commands

```dos
netsh http delete urlacl YourUrl
netsh http add urlacl url=YourUrl user="YourMachine\EndpointUsername"
```

For example

```dos
netsh http delete urlacl http://localhost:25894/MyEndpoint/
netsh http add urlacl url=http://localhost:25894/MyEndpoint/ user="Machine\MyEndpointServiceAccount"
```

To list all urlacls on a machine run

```dos
netsh http show urlacl
```

For more information see the relevant [Netsh commands for HTTP
](https://msdn.microsoft.com/en-us/library/windows/desktop/cc307236).