---
title: SQL Azure
summary: How to configure the NHibernate-based persistence for NServiceBus when running on SQL Azure
component: NHibernate
versions: '[6,]'
reviewed: 2016-11-04
tags:
 - Persistence
related:
 - nservicebus/nhibernate/accessing-data
---

When using a relational database 'as a service' for persisting NServiceBus data, it is important to keep in mind that this type of data store has different runtime semantics than a traditional relational database.

 * The data store is potentially located in a remote location and is thus subject to a broad range of potential networking issues.
 * 'As a service' environments such as SQL Azure are typically shared environments, which means that the performance of the database is impacted by the behavior of neighboring databases.

The combination of these characteristics makes that any transaction executed against the database may show intermittent exceptions. Usually these exceptions are transient in nature and can be resolved by retrying the transaction. As these exceptions can occur anywhere, they are best dealt with at the generic infrastructure level, using an NHibernate driver.

The NHibernate community has already gone through this exercise and created a driver specific for SQL Azure called [NHibernate.SqlAzure](https://github.com/MRCollective/NHibernate.SqlAzure/). This driver leverages the Microsoft Transient Fault Handling library to ensure reliable SQL Azure connections.

## NuGet packages

There are two choices of package for this library.

 * [NHibernate.SqlAzure](https://www.nuget.org/packages/NHibernate.SqlAzure). Has the Microsoft Transient Fault Handling library IL-merged into it.
 * [NHibernate.SqlAzure.Standalone](https://www.nuget.org/packages/NHibernate.SqlAzure.Standalone). Has a NuGet dependency on  Microsoft Transient Fault Handling.

## Usage

After installation of the package, enable the driver by setting NHibernate `connection.driver_class` property:

snippet: SqlAzureNHibernateDriverConfiguration
