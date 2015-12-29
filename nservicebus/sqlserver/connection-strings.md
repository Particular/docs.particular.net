---
title: SqlServer connection strings
summary: Detailed connection string information for SqlServer transport.
tags:
- Connection strings
- Transports
---

Connection string can be configured in several ways


### Via the App.Config

By adding a connection named `NServiceBus/Transport` in the `connectionStrings` node.
  

snippet:sqlserver-named-connection-string-appconfig


### Via the configuration API

By using the `ConnectionString` extension method.

snippet:sqlserver-config-connectionstring


### Via a named connection string

By using the `ConnectionStringName` extension method.

snippet:sqlserver-named-connection-string

Combined with a named connection in the `connectionStrings` node of you `app.config`.

snippet:sqlserver-named-connection-string-xml
