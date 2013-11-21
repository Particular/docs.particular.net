---
title: How To Debug RavenDB Through Fiddler Using NServiceBus
summary: Set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler by configuring the proxy for your endpoint.
originalUrl: http://www.particular.net/articles/how-to-debug-ravendb-through-fiddler-using-nservicebus
tags: []
---

To set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler, configure the proxy for your endpoint by adding the following code to app.config:


```XML
<system.net>
  <defaultProxy>
    <proxy usesystemdefault="False" bypassonlocal="True" proxyaddress="http://127.0.0.1:8888"/>
  </defaultProxy>
</system.net>
```

 With the proxy setup, change the RavenDB connection string to go through Fiddler by adding this:


```XML
<connectionStrings>
  <add name="NServiceBus.Persistence" connectionString="url=http://localhost.fiddler:8080"/>
</connectionStrings>
```




