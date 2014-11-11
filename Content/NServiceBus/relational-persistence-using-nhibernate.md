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

#### Version 3

<!-- import ConfiguringNHibernateV3 -->

#### Version 4
 
<!-- import ConfiguringNHibernateV4 -->

#### Version 5 

<!-- import ConfiguringNHibernateV5 -->

## Additional Configuration 

NServiceBus then picks up the connection setting from your `app.config`. 

NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig)

#### Version 3
 
<!-- import NHibernateAppConfigV3 -->

There are equivalent config sections for named `DBSubscriptionStorageConfig` and `TimeoutPersisterConfig`.

#### Version 4 and 5

app.config for using NHibernate is simplified in 4.x.

<!-- import NHibernateAppConfigV5 --> 





