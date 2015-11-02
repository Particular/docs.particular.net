---
title: NHibernate Persistence
summary: NHibernate-based persistence for NServiceBus
tags:
 - NHibernate
redirects:
 - nservicebus/relational-persistence-using-nhibernate
 - nservicebus/nhibernate/configuration
related:
- samples/nhibernate
---

Uses the [NHibernate ORM](http://nhibernate.info/) for persistence.


## Supported Persistence Types

 * [Gateway Deduplication](/nservicebus/gateway/)
 * [Sagas](/nservicebus/sagas/)
 * [Subscriptions](/nservicebus/sagas/)
 * [Timeouts](/nservicebus/sagas/#timeouts)
 * [Outbox](/nservicebus/outbox/)


## Usage


### Pull in the nugets

Install the [NServiceBus.NHibernate](https://www.nuget.org/packages/NServiceBus.NHibernate) nuget. This has a dependency on the `NHibernate` nuget so that will automatically be pulled in.


### The Code

The next stage is to actually tell NServiceBus how to use NHibernate for persistence

<!-- import ConfiguringNHibernate  -->


## Customizing the configuration 

In case you want to customize the NHibernate `Configuration` object used to bootstrap the persistence mechanism, you can either provide a ready-made object via code or use convention-based XML configuration. The code-based approach overrides the configuration-based one when both are used.


### Passing Configuration in code

The following snippet tells NServiceBus to use a given `Configuration` object for all the persistence concerns

<!-- import CommonNHibernateConfiguration -->

If you need specific configuration on a per-concern basis, you can use following

<!-- import SpecificNHibernateConfiguration -->

NOTE: You can combine both approaches to define a common configuration and override it for one specific concern.

WARNING: When using per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern you are enabling. The following code will set up NHibernate persistence only for `GatewayDeduplication` concern but will override the default configuration **for all the concerns**. 

<!-- import CustomCommonNhibernateConfigurationWarning -->


### Using configuration convention

NServiceBus then picks up the connection setting from your `app.config` from `connectionStrings` and `appSettings` sections. The convention used for `appSettings` does not allow you to define settings specific for a single persistence concern. If you need this level of granularity, you need to use code-based approach.

NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhibernate.info/doc/) 
 
<!-- import NHibernateAppConfig -->

## Change database schema

The database schema used can be changed by defining the `default_schema` NHibernate property. See the previous *Customizing the configuration* section.


## Subscription caching

The subscriptions can be cached when using NHibernate. This can improve the performance of publishing events as it is not required to request matching subscriptions from storage.

NOTE: Publishing is performed on stale data. This is only advised in high volume environments where latency can be a potential issue.

<!-- import NHibernateSubscriptionCaching -->