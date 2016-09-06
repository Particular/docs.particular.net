---
title: SQL Server Transport Connection Settings
reviewed: 2016-08-31
component: SqlServer
tags:
 - SQL Server
redirects:
 - nservicebus/sqlserver/concurrency
 - nservicebus/sqlserver/configuration
---


The SQL Server transport is built on top of ADO.NET and will use connection pooling. This may result in the connection pool being shared by the transport and other parts of the endpoint process. Depending on the situation it might be necessary to adjust the default connection pool size. See also [SQL Server Connection Pooling and Configuration](https://msdn.microsoft.com/en-us/library/8xx3tyca.aspx).


## Connection configuration

Connection string can be configured in several ways:


### Via the configuration API

By using the `ConnectionString` extension method:

snippet:sqlserver-config-connectionstring


### Via the App.Config

By adding a connection named `NServiceBus/Transport` in the `connectionStrings` node.

snippet:sqlserver-connection-string-xml


### Via a named connection string

By using the `ConnectionStringName` extension method:

snippet:sqlserver-named-connection-string

combined with a named connection in the `connectionStrings` node of the `app.config` file:

snippet:sqlserver-named-connection-string-xml


## Multiple connection strings

In [*multi-catalog* and *multi-instance* modes](/nservicebus/sqlserver/deployment-options.md) additional configuration is required for proper message routing:

 * The sending endpoint needs the connection string of the receiving endpoint.
 * The subscribing endpoint needs the connection string of the publishing endpoint, in order to send subscription request.

Connection strings for the remote endpoint can be configured in several ways:

partial:multiple-connection-push


### Via the configuration API - Pull mode

The pull mode can be used when specific information is not available at configuration time. One can pass a function that will be used by the SQL Server transport to resolve connection information at runtime.

snippet:sqlserver-multidb-other-endpoint-connection-pull

Note that in Version 3 the `EnableLagacyMultiInstanceMode` method passes transport address parameter. Transport address conforms to the `endpoint_name@schema_name` convention, e.g. could be equal to `Samples.SqlServer.MultiInstanceSender@[dbo]`.


partial: multiple-appconfig


## Custom database schemas

SQL Server transport uses `dbo` as a default schema. Default schema is used for every queue if no other schema is explictly provided in transport address. That includes all local queues, error, audit and remote queues of other endpoints. 
In Sql Server default schema can be overridden using `DefaultSchema` method:

snippet:sqlserver-non-standard-schema

partial: custom-schema

## Custom schema per destination

If different schemas should be used for different destinations, then additional configuration is required:

 * The sending endpoint needs to know the custom schema of the receiving endpoint's input queue.
 * The sending endpoint needs to know the custom schema of infrastructural queues e.g. error. 
 * The subscriber will need the schema information of the publisher, in order to send subscription request.
 * In Versions 2.1.x to 2.x publisher also needs to know the schema of every subscriber. The same applies to sending reply messages using `ReplyTo()` or callbacks.

The schema for an endpoint can be specified in the following way:

snippet: sqlserver-multischema-config-for-endpoint

The schema for a given queue can be specifed in the following way:

snippet:sqlserver-multischema-config-for-queue

snippet:sqlserver-non-standard-schema-messagemapping

Notice that in Versions 3 and higher the table and schema names can be passed either using common convention with square brackets, or without them. 
 

## Custom SQL Server transport connection factory

In some environments it might be necessary to adapt to database server settings, or to perform additional operations. For example, if the `NOCOUNT` setting is enabled on the server, then it is necessary to send the `SET NOCOUNT OFF` command right after opening the connection.

That can be done by passing the transport a custom factory method which will provide connection strings at runtime, and which can perform custom actions:

snippet:sqlserver-custom-connection-factory


## Circuit Breaker

The SQL transport has a built in circuit breaker to handle intermittent SQL Server connectivity problems.


### Wait time

Overrides the default time to wait before triggering a circuit breaker that initiates the endpoint shutdown procedure in case of [repeated critical errors](/nservicebus/hosting/critical-errors.md).

The default value is 2 minutes.

snippet:sqlserver-TimeToWaitBeforeTriggeringCircuitBreaker


partial: pause-time