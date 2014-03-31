---
title: NServiceBus Connection Strings samples
summary: Provides a list of samples connections strings for different transports supported by NServiceBus.
tags:
- NServiceBus
- Connection strings
- Transports
---

NServiceBus, out of the box, supports a lot of different transports such as:

* MSMQ;
* ActiveMQ;
* RabbitMQ;
* SqlServer;
* Azure ServiceBus;

All the transports can be configured in the application configuration file using the NServiceBus/Transport` connection string name; The following is a sample list of connection strings for the above transports:

```xml
<connectionStrings>
<!-- MSMQ -->
<add name="NServiceBus/Transport" connectionString="deadLetter=true;
   journal=true;useTransactionalQueues=true;
   cacheSendConnection=true"/>
<!-- ActiveMQ -->
<add name="NServiceBus/Transport" 
   connectionString="ServerUrl=activemq:tcp://localhost:61616"/>
<!-- RabbitMQ-->
<add name="NServiceBus/Transport" 
   connectionString="host=localhost"/>
<!-- SqlServer -->
<add name="NServiceBus/Transport" 
   connectionString="Data Source=.\SQLEXPRESS;Initial Catalog=nservicebus;
   Integrated Security=True"
/>
<!-- Azure ServiceBus -->
<add name="NServiceBus/Transport" 
   connectionString="Endpoint=sb://[namespace].servicebus.windows.net; SharedSecretIssuer=owner;SharedSecretValue=someSecret"
/>
</connectionStrings>
```