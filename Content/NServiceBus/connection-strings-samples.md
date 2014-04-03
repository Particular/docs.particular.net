---
title: NServiceBus Connection String Samples
summary: Provides a list of sample connections strings for different transports supported by NServiceBus.
tags:
- NServiceBus
- Connection strings
- Transports
---

NServiceBus supports the following transports out of the box:

* MSMQ
* ActiveMQ
* RabbitMQ
* SQL Server
* Microsoft Azure Service Bus
* Microsoft Azure Storage Queues

To configure NServiceBus endpoints to use a specific transport:

1. Set a connection string, named `NServiceBus/Transport`, in the endpoint's configuration file.
1. Install the relevant transport NuGet package.

###MSMQ

* NuGet transport package: not required, MSMQ is supported out of the box
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

**NOTE:** The MSMQ connection string is optional.

###ActiveMQ

* NuGet transport package: [NServiceBus.ActiveMQ](https://www.nuget.org/packages/NServiceBus.ActiveMQ/);
* Connection string sample:

```xml
<connectionStrings>
   <!-- ActiveMQ -->
   <add name="NServiceBus/Transport"
        connectionString="ServerUrl=activemq:tcp://localhost:61616"/>
</connectionStrings>
```

###RabbitMQ

* NuGet transport package: [NServiceBus.RabbitMQ](https://www.nuget.org/packages/NServiceBus.RabbitMQ/);
* Connection string sample:

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport"
        connectionString="host=localhost"/>
</connectionStrings>
```

###SQL Server

* NuGet transport package: [NServiceBus.SqlServer](https://www.nuget.org/packages/NServiceBus.SqlServer/);
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

###Microsoft Azure Service Bus

* NuGet transport package: [NServiceBus.Azure.Transports.WindowsAzureServiceBus](https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureServiceBus/);
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

* NuGet transport package: [NServiceBus.Azure.Transports.WindowsAzureStorageQueues](https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/);
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
