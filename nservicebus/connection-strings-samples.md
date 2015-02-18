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
* SQL Server
* RabbitMQ
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

* `deadLetter`: The `deadLetter` configuration parameter enables or disables dead letter queue support, dead letter queues tracks messages that cannot be delivered (more information: https://msdn.microsoft.com/en-us/library/ms706227.aspx). The default value is `true`;
* `journal`: MSMQ supports the concept of journaling, when the journaling is active a copy of each sent message is stored in the journal queue (more information: https://msdn.microsoft.com/en-us/library/ms702011.aspx). The default value is `false`;
* `useTransactionalQueues`: determines if the generated queue, or the existing ones, must be transactional queues or not. The default value is `true`;
* `cacheSendConnection`: instructs the underlying infrastructure to cache the connection to a remote queue and re-use it as needed instead of creating each time a new connection to send messages. The default value is `true`;
* `timeToReachQueue`: The time limit for the message to reach the destination queue, beginning from the time the message is sent. **This setting is only available in v5.1+**;

NOTE: The MSMQ connection string is optional.

<!--

###ActiveMQ

* NuGet transport package: [NServiceBus.ActiveMQ](https://www.nuget.org/packages/NServiceBus.ActiveMQ/);
* Connection string sample:

```xml
<connectionStrings>
   <add name="NServiceBus/Transport"
        connectionString="ServerUrl=activemq:tcp://localhost:61616"/>
</connectionStrings>
```

-->

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

* For remote host provide username and password because remote hosts don't accept default guest credentials

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport"
        connectionString="host=myremoteserver;
                          username=myusername;
                          password=mypassowrd"/>
</connectionStrings>
```

* For For clustered RabbitMQ 

```xml
<connectionStrings>
   <!-- RabbitMQ -->
   <add name="NServiceBus/Transport" 
			connectionString="host=rabbitNode1,rabbitNode2,rabbitNode3;
				username=myuser;
				password=password" />
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
