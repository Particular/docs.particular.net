---
title: ServicePointManager and HTTP connections
summary: When connecting to external resources via HTTP from an endpoint it is important to understand the role of the ServicePointManager class.
tags: []
redirects:
 - nservicebus/gateway-service-point-manager
---

During message handling an endpoint is expected to be able to connect to external resources, such as remote services via HTTP.

If the endpoint is hosted in a process outside IIS, such as a Windows Service, by default the .Net Framework allows 2 concurrent outgoing HTTP requests per process. This can be a limitation on the overall throughput of the endpoint itself that ends up having outgoing HTTP requests queued and thus, as a consequence, a limitation in its ability to process incoming messages.

It is possible to change the default connection limit of a process via the static `DefaultConnectionLimit` property of the `ServicePointManager` class, as in the following sample:

```csharp
ServicePointManager.DefaultConnectionLimit = 10;
```

The above code can be placed in conjunction with the endpoint configuration code, at process startup, or in the `EndpointConfig` class.

#### Resources

ServicePointManager on MSDN: [https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.aspx](https://msdn.microsoft.com/en-us/library/system.net.servicepointmanager.aspx)
