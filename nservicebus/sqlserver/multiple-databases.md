---
title: Multi-broker support
summary: How to configure SQLServer transport to use multiple instances of the database and route messages between them.
tags:
- SQLServer
- Transport
---

The SQLServer transport allows you to use select, on per-endpoint basis, where the table queues should be created. The selection can be done on multiple levels:
 * schemas in a single database
 * databases in a single SQLServer instance
 * different SQLServer instances

The transport will route messages to destination endpoints based on the configuration. If no specific configuration has been provided for a particular destination endpoint, the transport assumes the destination has the same configuration (schema, database and instance name/address) as the sending endpoint. If this assumption turns out to be false (the transport cannot connect to destination queue), an exception is thrown immediately. There is no store-and-forward mechanism (and hence -- no dead-letter queue).

### Current endpoint

SQLServer transport defaults to `dbo` schema and uses `NServiceBus/Transport` connection string from the configuration file to connect to the database. The default schema can be chnaged using following API

```
busConfig.UseTransport<SqlServerTransport>().DefaultSchema("myschema")
```

or via providing additional `Queue Schema` parameter in the connection string

```
<connectionStrings>
   <add name="NServiceBus/Transport" connectionString="Data Source=INSTANCE_NAME; Initial Catalog=some_database; Integrated Security=True; Queue Schema=nsb"/>
</connectionStrings>
```

The second approach has precedence over the first one.

The other parameters (database and instance name/address) can be changed in code using the connection string API

```
busConfig.UseTransport<SqlServerTransport>().ConnectionString(@"Data Source=INSTANCE_NAME;Initial Catalog=some_database;Integrated Security=True")
```

NOTE: `Queue Schema` paramater can also be used in the connection string provided via code.

NOTE: Unlike in the SQLServet transport, the connection string configuration API in NServiceBus core favors code over xml which means that if you configure connection string both in `app.config` and via the `ConnectionString()` method, the latter will win.

### Other endpoints

If a particular remote endpoint requires customization of any part of the routing (schema, database or instance name/address), appropriate values have to be provide either via code or via configuration convention.

#### Push mode

In the push mode the whole collection of endpoint connection information objects is passed during configuration time.

```
busConfig.UseTransport<SqlServerTransport>().UseSpecificConnectionInformation(
   EndpointConnectionInfo.For("RemoteEndpoint")
      .UseSchema("receiver1")
      .UseConnectionString("SomeConnectionString"),
   EndpointConnectionInfo.For("AnotherEndpoint")
      .UseSchema("receiver2")
      .UseConnectionString("SomeConnectionString")
))
```

#### Pull mode

The pull mode can be used when specific information is not available at configuration time. He can pass a `Func<String, ConnectionInfo>` that will be used by the SQLServer transport to resolve connection information at runtime.

```
busConfig.UseTransport<SqlServerTransport>()
   .UseSpecificConnectionInformation(x =>
   {
      return x == "RemoteEndpoint" 
        ? ConnectionInfo.Create().UseConnectionString(ReceiverConnectionString).UseSchema("nsb") 
        : null;
   })
``` 

#### Config

Endpoint-specific connection information is discovered by reading the connection strings from the configuration file with `NServiceBus/Transport/{name of the endpoint in the message mappings}` naming convention. If such a connection string is found, it is used for a given endpoint and this setting has precedence over the code-provided connection information.
Given the following mappings:

<!-- import sqlserver-multidb-messagemapping -->

and the following connection strings:
 
<!-- import sqlserver-multidb-connectionstrings -->

the messages sent to `billing` will go to the specific database `Billing` on server `DbServerB` while the messages to `sales` will go to the database and server set by default i.e. `MyDefaultDB` on server `DbServerA`.
