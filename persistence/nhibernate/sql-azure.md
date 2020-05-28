---
title: SQL Azure
summary: How to configure the NHibernate-based persistence for NServiceBus when running on SQL Azure
component: NHibernate
versions: '[6,]'
reviewed: 2020-03-20
related:
 - persistence/nhibernate/accessing-data
redirects:
 - nservicebus/nhibernate/sql-azure
---

NHibernate persistence works with SQL Azure without a need to do any changes to code or configuration. However, there are considerations that should be taken when using a remote database.

## Relational database as a service

When using a relational database as a service for persisting NServiceBus data, it is important to keep in mind that this type of data store has different runtime semantics than a traditional relational database.

 * The data store is potentially located in a remote location and is thus subject to a broad range of potential networking issues.
 * 'As a service' environments such as SQL Azure are typically shared environments, so performance of the database is impacted by the behavior of other databases on the server.

The combination of these characteristics means that any transaction executed against the database may show intermittent exceptions. Usually these exceptions are transient in nature and can be resolved by retrying the transaction. A good place to start is the [Transient Faults](https://docs.microsoft.com/en-us/azure/architecture/best-practices/transient-faults) documentation from Microsoft.

### NHibernate specifics

As these exceptions can occur anywhere, they are best dealt with at the generic infrastructure level, using an NHibernate driver.

The NHibernate community has created a driver specific for SQL Azure called [NHibernate.SqlAzure](https://github.com/MRCollective/NHibernate.SqlAzure/). This driver leverages the Microsoft Transient Fault Handling library to ensure reliable SQL Azure connections.

#### NuGet packages

There are two choices of package for this library.

 * [NHibernate.SqlAzure](https://www.nuget.org/packages/NHibernate.SqlAzure). Has the Microsoft Transient Fault Handling library IL-merged into it.
 * [NHibernate.SqlAzure.Standalone](https://www.nuget.org/packages/NHibernate.SqlAzure.Standalone). Has a NuGet dependency on Microsoft Transient Fault Handling.

## Usage

After installation of the package, enable the driver by setting the NHibernate `connection.driver_class` property:

snippet: SqlAzureNHibernateDriverConfiguration