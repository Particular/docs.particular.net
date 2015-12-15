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


### Pull in the NuGets

Install the [NServiceBus.NHibernate](https://www.nuget.org/packages/NServiceBus.NHibernate) NuGet. This has a dependency on the `NHibernate` NuGet so that will automatically be pulled in.


### The Code

The next stage is to actually tell NServiceBus how to use NHibernate for persistence

snippet:ConfiguringNHibernate


## Customizing the configuration

In case you want to customize the NHibernate `Configuration` object used to bootstrap the persistence mechanism, you can either provide a ready-made object via code or use convention-based XML configuration. The code-based approach overrides the configuration-based one when both are used.


### Passing Configuration in code

The following snippet tells NServiceBus to use a given `Configuration` object for all the persistence concerns

snippet:CommonNHibernateConfiguration

If you need specific configuration on a per-concern basis, you can use following

snippet:SpecificNHibernateConfiguration

NOTE: You can combine both approaches to define a common configuration and override it for one specific concern.

WARNING: When using per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern you are enabling. The following code will set up NHibernate persistence only for `GatewayDeduplication` concern but will override the default configuration **for all the concerns**.

snippet:CustomCommonNhibernateConfigurationWarning


### Using configuration convention

NServiceBus then picks up the connection setting from your `app.config` from `connectionStrings` and `appSettings` sections. The convention used for `appSettings` does not allow you to define settings specific for a single persistence concern. If you need this level of granularity, you need to use code-based approach.

NOTE: When using SQL 2012 you need to change the dialect to `MsSql2012Dialect`.

NOTE: Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhibernate.info/doc/)

snippet:NHibernateAppConfig


## Change database schema

The database schema used can be changed by defining the `default_schema` NHibernate property. See the previous *Customizing the configuration* section.


## Subscription caching

The subscriptions can be cached when using NHibernate. This can improve the performance of publishing events as it is not required to request matching subscriptions from storage.

NOTE: Publishing is performed on stale data. This is only advised in high volume environments where latency can be a potential issue.

snippet:NHibernateSubscriptionCaching


## Controlling schema

In some cases it may be necessary to take full control over creating the SQL structure used by the NHibernate persister. In these cases the automatic creation of SQL structures on install can be disabled as follows:


**For all persistence schema updates:**

snippet:DisableSchemaUpdate


**For Gateway schema update:**
           
snippet:DisableGatewaySchemaUpdate


**For Subscription schema update:**

snippet:DisableSubscriptionSchemaUpdate


**For Timeout schema update:**

snippet:DisableTimeoutSchemaUpdate


### Generating scripts for deployment

To create scripts, for execution in production without using the NServiceBus installers, run an install in a lower environment and then export the SQL structure. This structure can then be migrated to production.
