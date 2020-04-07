---
title: NHibernate Persistence
summary: NHibernate-based persistence for NServiceBus
component: NHibernate
reviewed: 2020-04-07
redirects:
 - nservicebus/relational-persistence-using-nhibernate
 - nservicebus/nhibernate/configuration
 - nservicebus/nhibernate
related:
- samples/nhibernate
- samples/multi-tenant/nhibernate
---

Uses the [NHibernate ORM](https://nhibernate.info/) for persistence.


## Supported persistence types

partial: supported-persistence


## Supported database engines

 * [Microsoft SQL Server](https://www.microsoft.com/en-au/sql-server/) ([Version 2012](https://docs.microsoft.com/en-us/sql/release-notes/sql-server-2012-release-notes) and above).
 * [Oracle Database](https://www.oracle.com/database/index.html) ([Version 11g Release 2](https://docs.oracle.com/cd/E11882_01/readmes.112/e41331/chapter11204.htm) and above).


NOTE: When connecting to an Oracle Database, only the ODP.NET managed driver is supported. The driver is available via the [Oracle.ManagedDataAccess NuGet Package](https://www.nuget.org/packages/Oracle.ManagedDataAccess).

WARNING: Although this persistence will run on the free version of the above engines, i.e. [SQL Server Express](https://www.microsoft.com/en-au/sql-server/sql-server-editions-express) and [Oracle XE](https://www.oracle.com/technetwork/database/database-technologies/express-edition/overview/index.html), it is strongly recommended to use commercial versions for any production system. It is also recommended to ensure that support agreements are in place from either [Microsoft Premier Support](https://www.microsoft.com/en-us/microsoftservices/support.aspx), [Oracle Support](https://www.oracle.com/support/index.html), or another third party support provider.


## Usage

The next stage is to tell NServiceBus how to use NHibernate for persistence

snippet: ConfiguringNHibernate


## Connection strings

It is possible to pass a connection string in the `app.config` file, as described in the [using configuration convention](/persistence/nhibernate/#customizing-the-configuration-using-configuration-convention) section.


### With code 

NHibernate persistence requires specifying a connection string.

The connection string might be passed using code configuration:

snippet: ConnectionStringAPI


## Customizing the configuration

To customize the NHibernate `Configuration` object used to bootstrap the persistence mechanism, either provide a ready-made object via code or use convention-based XML configuration. The code-based approach overrides the configuration-based one when both are used.


### Passing configuration in code

To specify configuration on a per-concern basis:

snippet: SpecificNHibernateConfiguration

NOTE: Combine both approaches to define a common configuration and override it for one specific concern.




To use a given NHibernate `Configuration` object for all the persistence concerns:

snippet: CommonNHibernateConfiguration


WARNING: When using the per-concern API to enable the NHibernate persistence, the `UseConfiguration` method still applies to the common configuration, not the specific concern being enabled. The following code will set up NHibernate persistence only for `Subscriptions` concern but will override the default configuration **for all the concerns**.

snippet: CustomCommonNhibernateConfigurationWarning


### Using configuration convention

NServiceBus picks up the connection setting from the `app.config` from `connectionStrings` and `appSettings` sections. The convention used for `appSettings` does not support defining settings specific for a single persistence concern. If this level of granularity is required, use a code-based approach.

NOTE: When using SQL 2012 or later, change the dialect to `MsSql2012Dialect`. Additional dialects are available in the NHibernate.Dialect namespace, [NHibernate documentation.](https://nhibernate.info/doc/)

snippet: NHibernateAppConfig


## Change database schema

The database schema can be changed by defining the `default_schema` NHibernate property. See the previous *Customizing the configuration* section.


## Subscription caching

The subscriptions can be cached when using NHibernate. This can improve the performance of publishing events as it is not required to request matching subscriptions from storage.

NOTE: Publishing is performed on stale data. This is only advised in high volume environments where latency can be an issue.

snippet: NHibernateSubscriptionCaching



## Controlling schema

In some cases it may be necessary to take full control over creating the SQL structure used by the NHibernate persister. In these cases the automatic creation of SQL structures on install can be disabled as follows:


**For all persistence schema updates:**

snippet: DisableSchemaUpdate


partial: gateway-schema-update


**For subscription schema update:**

snippet: DisableSubscriptionSchemaUpdate


**For timeout schema update:**

snippet: DisableTimeoutSchemaUpdate



## Generating scripts for deployment

To create scripts for execution in production without using the [installers](/nservicebus/operations/installers.md), run an install in a lower environment and then export the SQL structure. This structure can then be migrated to production.
