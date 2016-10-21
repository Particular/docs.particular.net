---
title: NHibernate Persistence
summary: NHibernate-based persistence for NServiceBus
component: NHibernate
reviewed: 2016-10-19
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

The next stage is to actually tell NServiceBus how to use NHibernate for persistence

snippet:ConfiguringNHibernate


## Connection strings

It is possible to pass connection string in the `app.config` file, as described in the [Using configuration convention](/nservicebus/nhibernate/#customizing-the-configuration-using-configuration-convention) section.


partial: code-connection


## Customizing the configuration

To customize the NHibernate `Configuration` object used to bootstrap the persistence mechanism, either provide a ready-made object via code or use convention-based XML configuration. The code-based approach overrides the configuration-based one when both are used.


### Passing Configuration in code

To specific configuration on a per-concern basis, use following

snippet:SpecificNHibernateConfiguration

NOTE: Combine both approaches to define a common configuration and override it for one specific concern.

partial:code


### Using configuration convention

NServiceBus then picks up the connection setting from the `app.config` from `connectionStrings` and `appSettings` sections. The convention used for `appSettings` does not support defining settings specific for a single persistence concern. If this level of granularity is required use a code-based approach.

NOTE: When using SQL 2012 or later, change the dialect to `MsSql2012Dialect`. Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](http://nhibernate.info/doc/)

snippet:NHibernateAppConfig


## Change database schema

The database schema used can be changed by defining the `default_schema` NHibernate property. See the previous *Customizing the configuration* section.


## Subscription caching

The subscriptions can be cached when using NHibernate. This can improve the performance of publishing events as it is not required to request matching subscriptions from storage.

NOTE: Publishing is performed on stale data. This is only advised in high volume environments where latency can be a potential issue.

snippet:NHibernateSubscriptionCaching


partial: schema


## Generating scripts for deployment

To create scripts, for execution in production without using the [installers](/nservicebus/operations/installers.md), run an install in a lower environment and then export the SQL structure. This structure can then be migrated to production.