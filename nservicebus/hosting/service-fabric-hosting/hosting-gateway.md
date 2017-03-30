---
title: Hosting Gateway with Service Fabric
related:
 - nservicebus/service-fabric
reviewed: 2017-03-30
---

To host NServiceBus Gateway in an endpoint deployed to Service Fabric, the following has to be done:

1. Channel address should use URL with wildcards (TODO: xml below should be a snippet)
1. HTTP communication listener defined (TODO: code snippet, or better, point to an existing SF doco)  


```xml
<Channel Address="http://+:25899/RemoteSite/"
             ChannelType="Http"
             Default="true"/>
```