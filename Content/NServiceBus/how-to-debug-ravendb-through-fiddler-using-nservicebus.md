---
title: How To Debug RavenDB Through Fiddler Using NServiceBus
summary: Set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler by configuring the proxy for your endpoint.
originalUrl: http://www.particular.net/articles/how-to-debug-ravendb-through-fiddler-using-nservicebus
tags: []
createdDate: 2013-05-22T08:43:40Z
modifiedDate: 2014-01-18T09:43:50Z
authors: []
reviewers: []
contributors: []
---

To set up an NServiceBus endpoint to make all calls to RavenDB through Fiddler configure the proxy for your endpoint by adding the following code to the configuration file:


```XML
<system.net>
  <defaultProxy>
    <proxy usesystemdefault="False" bypassonlocal="True" proxyaddress="http://127.0.0.1:8888"/>
  </defaultProxy>
</system.net>
```

 With this proxy setup, change the RavenDB connection string, to go through Fiddler, by setting the following special host name:


```XML
<connectionStrings>
  <add name="NServiceBus.Persistence" connectionString="url=http://localhost.fiddler:8080"/>
</connectionStrings>
```




