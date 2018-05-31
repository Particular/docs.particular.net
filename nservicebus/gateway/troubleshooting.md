---
title: Troubleshooting
summary: How to solve common issues that arise with the gateway
reviewed: 2018-05-30
redirects:
 - nservicebus/gateway-trouble-shooting
 - nservicebus/gateway-service-point-manager
 - nservicebus/gateway/service-point-manager
---

## Persistence support

The qateway requires NServiceBus persistence to operate though not all persistences support it. Currently, the gateway is supported only by InMemory, RavenDB and NHibernate persistences. If the configured persistence doesn't support gateway, the `The selected persistence doesn't have support for gateway deduplication storage.` exception will be thrown at endpoint startup.


## Corrupted urlacls

At startup the gateway will attempt to recreate the urlacls so that the process can listen on a specific port. If they already exist, a warning is logged:

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

To verify this problem navigate to the url (in this case `http://localhost:25894/MyEndpoint/`) in a browser. If the gateway is listening the following will be received:

```
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


## ServicePointManager HTTP connections

During message handling an endpoint is expected to be able to connect to external resources, such as remote services via HTTP.

If the endpoint is hosted in a process outside IIS, such as a Windows Service, by default the .NET Framework allows 2 concurrent outgoing HTTP requests per process. This can be a limitation on the overall throughput of the endpoint itself that ends up having outgoing HTTP requests queued and thus, as a consequence, a limitation in its ability to process incoming messages.

It is possible to change the default connection limit of a process via the static `DefaultConnectionLimit` property of the `ServicePointManager` class, as in the following sample:

```cs
ServicePointManager.DefaultConnectionLimit = 10;
```

The above code can be placed in the process startup.

See [ServicePointManager on MSDN](https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.aspx) for more information.
