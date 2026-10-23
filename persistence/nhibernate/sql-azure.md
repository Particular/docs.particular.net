---
title: SQL Azure
summary: How to configure the NHibernate-based persistence for NServiceBus when running on SQL Azure
component: NHibernate
versions: '[6,]'
reviewed: 2026-10-21
related:
 - persistence/nhibernate/accessing-data
redirects:
 - nservicebus/nhibernate/sql-azure
---

NHibernate persistence works with SQL Azure without needing to make any changes to code or configuration. However, there are considerations that should be taken into account when using a remote database.

## Relational database as a service

When using a relational database as a service for persisting NServiceBus data, it is important to keep in mind that this type of data store has different runtime semantics than a traditional relational database.

 * The data store is potentially located in a remote location and is thus subject to a broad range of potential networking issues.
 * 'As a service' environments such as SQL Azure are typically shared environments, so performance of the database can be impacted by the behavior of other databases on the server.

The combination of these characteristics means that any transaction executed against the database may show intermittent exceptions. Usually these exceptions are transient in nature and can be resolved by retrying the transaction. A good place to start is the [Transient Fault Handling](https://learn.microsoft.com/en-us/azure/architecture/best-practices/transient-faults) documentation from Microsoft.

## Handling transient failures

Message processing that fails because of a transient database error is retried by [recoverability](/nservicebus/recoverability/).

To reduce failures caused by dropped connections, add `ConnectRetryCount` and `ConnectRetryInterval` to the connection string. These settings control connection retries and the reconnection of idle connections that were broken. See [Troubleshoot transient connection errors in Azure SQL](https://learn.microsoft.com/en-us/azure/azure-sql/database/troubleshoot-common-connectivity-issues) for details and defaults.

#if-version [6,10)

### SQL Azure driver

On .NET Framework, the community-maintained [NHibernate.SqlAzure](https://github.com/MRCollective/NHibernate.SqlAzure/) driver can be used instead. The driver uses the Microsoft Transient Fault Handling library to retry failed SQL Azure connections. The package targets .NET Framework 4.5 and has not been updated since 2020.

There are two choices of package for this library:

- [NHibernate.SqlAzure](https://www.nuget.org/packages/NHibernate.SqlAzure) has the Microsoft Transient Fault Handling library IL-merged into it.
- [NHibernate.SqlAzure.Standalone](https://www.nuget.org/packages/NHibernate.SqlAzure.Standalone) has a NuGet dependency on Microsoft Transient Fault Handling.

After installation of the package, enable the driver by setting the NHibernate `connection.driver_class` property:

snippet: SqlAzureNHibernateDriverConfiguration

#end-if
