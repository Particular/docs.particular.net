---
title: NHibernate Persistence
summary: To store data in a relational database, NServiceBus provides a separate assembly with support for NHibernate-based storage.
tags: []
---

If you require that your data persist in a relational database, NServiceBus provides a nuget ([NServiceBus.NHibernate](https://www.nuget.org/packages/NServiceBus.NHibernate)) for NHibernate-based persistence 

```
PM> Install-Package NServiceBus.NHibernate
```

This automatically sets up all the dependencies and is the recommended way of using NHibernate support.

## Subscriptions, Sagas, Timeouts and Gateway persistence

To use Subscriptions, Sagas, Timeouts and Gateway persistence using NHibernate, use this configuration:

<!-- import ConfiguringNHibernate  -->

## Additional Configuration 

NServiceBus then picks up the connection setting from your `app.config`. 

NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig) 
 
<!-- import NHibernateAppConfig -->






