---
title: NHibernate Persistence installation and configuration
summary: How to install and configure the NHibernate-based persistence for NServiceBus
tags:
 - NHibernate
 - Persistence
 - Configuration
---

If you require that your data persist in a relational database, NServiceBus provides a nuget ([NServiceBus.NHibernate](https://www.nuget.org/packages/NServiceBus.NHibernate)) for NHibernate-based persistence 

```
PM> Install-Package NServiceBus.NHibernate
```

This automatically sets up all the dependencies and is the recommended way of using NHibernate support.

## Subscriptions, Sagas, Timeouts and Gateway persistence

The next stage is to actually tell NServiceBus how to use the added reference

<!-- import ConfiguringNHibernate  -->

## Customizing the configuration 

In case you want to customize the NHibernate `Configuration` object used to bootstrap the persistence mechanism, you can either provide a ready-made object via code or use convention-based XML configuration. The code-based approach overrides the configuration-based one when both are used.

### Passing Configuration in code

The following snippet tells NServiceBus to use a given `Configuration` object for all the persistence concerns

<!-- import CustomCommonConfiguration -->

If you need specific configuration on a per-concern basis, you can use following

<!-- import CustomSpecificConfiguration -->

NOTE: You can combine both approaches to define a common configuration and override it for one specific concern.

WARNING: When using per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern you are enabling. The following code will set up NHibernate persistence only for `GatewayDeduplication` concern but will override the default configuration **for all the concerns**. 

<!-- import CustomCommonConfigurationWarning -->


### Using configuration convention

NServiceBus then picks up the connection setting from your `app.config` from `connectionStrings` and `appSettings` sections. The convention used for `appSettings` does not allow you to define settings specific for a single persistence concern. If you need this level of granularity, you need to use code-based approach.

NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhforge.org/doc/nh/en/index.html#configuration-xmlconfig) 
 
<!-- import NHibernateAppConfig -->

#### Further reading

 * [Accessing data](accessing-data.md)







