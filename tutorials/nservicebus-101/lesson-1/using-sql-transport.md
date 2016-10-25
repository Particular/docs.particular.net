---
title: Using the SQL Server transport
suppressRelated: true
---

NServiceBus is a messaging framework and requires a **message transport** to move messages around. Natively, NServiceBus supports Microsoft Message Queuing (MSMQ), RabbitMQ, Azure Storage Queues, Azure Service Bus, and SQL Server as message transports. (Although SQL Server is not a message queue, the SQL Server Transport emulates queues within database tables.)

This course uses MSMQ as the default message transport, but in some environments, you may not have sufficient privileges on your system to install MSMQ. In this case, If this is the case, the SQL Server transport is a good substitute. NServiceBus largely smoothes over the differences between message transports, allowing you to use the same messaging API no matter what the underlying infrastructure.


## Setting up

You may already have access to a SQL Server instance you can use. Otherwise, you can download and install [SQL Server Express](http://downloadsqlserverexpress.com/) on your local machine.

On your database server, create a database named **NServiceBusAcademy**.


## Modifying each endpoint

Each endpoint must have a reference to the **NServiceBus.SqlServer** NuGet package and be configured to use the SQL Server transport, including the database connection string.


### Adding the NuGet package

The [NServiceBus.SqlServer](https://www.nuget.org/packages/NServiceBus.SqlServer) NuGet package contains the code that allows NServiceBus to use SQL Server tables as queues.

To use the SQL Server transport instead of MSMQ, you will need to add the NuGet package to each Console Application project that is hosting a messaging endpoint. For example, to add the NuGet package to the ClientUI project in [Lesson 1](../):

    Install-Package NServiceBus.SqlServer -ProjectName ClientUI


### Configuring the transport

In each lesson, look for any lines of code that look like this, which configure NServiceBus to use the default MSMQ transport. Replace this line as shown:

```
// Replace this...
endpointConfig.UseTransport<MsmqTransport>();

// ...with this
endpointConfig.UseTransport<SqlServerTransport>()
    .ConnectionString(@"Server=.\sqlexpress;Initial Catalog=NServiceBusAcademy;Trusted_Connection=true;");
```

The connection string provided assumes use of SQL Express. You may need to change the connection string as appropriate to connect to your SQL Server instance.


## Further reading

* [SQL Server Transport documentation](/nservicebus/sqlserver/)
