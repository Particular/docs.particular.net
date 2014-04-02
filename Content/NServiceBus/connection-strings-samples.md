---
title: NServiceBus Connection Strings samples
summary: Provides a list of samples connections strings for different transports supported by NServiceBus.
tags:
- NServiceBus
- Connection strings
- Transports
---

NServiceBus supports the following transports out of the box:

* MSMQ;
* ActiveMQ;
* RabbitMQ;
* SQL Server;
* Microsoft Azure ServiceBus;
* Microsoft Azure Storage Queues;

You can configure NServiceBus endpoints to use a specific transport by:

* Setting a connection string, named `NServiceBus/Transport`, in the endpoint's configuration file;
* Installing the relevant transport NuGet package;

###MSMQ

* Nuget transport package: not required, MSMQ is supported out of the box;
* Connection string sample: 

```xml
<connectionStrings>
   <!-- MSMQ -->
   <add name="NServiceBus/Transport"
        connectionString="deadLetter=true;
                          journal=true;
                          useTransactionalQueues=true;
                          cacheSendConnection=true"/>
</connectionStrings>
```

###ActiveMQ

* Nuget transport package: []();
* Connection string sample:

```xml
<connectionStrings>
   <!-- ActiveMQ -->
   <add name="NServiceBus/Transport"
        connectionString="ServerUrl=activemq:tcp://localhost:61616"/>
</connectionStrings>
```

###RabbitMQ

* Nuget transport package: []();
* Connection string sample:

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport"
        connectionString="host=localhost"/>
</connectionStrings>
```

###SQL Server

* Nuget transport package: []();
* Connection string sample:
   
```xml
<connectionStrings>
   <!-- SQL Server -->
   <add name="NServiceBus/Transport"
        connectionString="Data Source=.\SQLEXPRESS;
                                      Initial Catalog=nservicebus;
                                      Integrated Security=True"/>
</connectionStrings>
```

###Microsoft Azure ServiceBus

* Nuget transport package: []();
* Connection string sample:

```xml
<connectionStrings>
   <!-- Azure ServiceBus -->
   <add name="NServiceBus/Transport"
        connectionString="Endpoint=sb://[namespace].servicebus.windows.net;
                                      SharedSecretIssuer=owner;
                                      SharedSecretValue=someSecret"/>
</connectionStrings>
```

###Microsoft Azure Storage Queues

* Nuget transport package: []();
* Connection string sample:

```xml
<connectionStrings>
   <!-- Azure Storage Queues -->
   <add name="NServiceBus/Transport"
        connectionString="DefaultEndpointsProtocol=https;
                                      AccountName=myAccount;
                                      AccountKey=myKey;"/>
</connectionStrings>
```
