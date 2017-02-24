---
title: Sql Persistence Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is the native Sql Persistence
component: SqlPersistence
reviewed: 2017-02-24
tags:
- Saga
- SagaFinder
related:
- nservicebus/sagas
- nservicebus/sql-persistence
---

include: sagafinder-into


## Prerequisites


include: sql-persistence-prereqs


## Persistence Config

Configure the endpoint to use SQL Persistence.


### MS SQL Server

snippet:sqlServerConfig


### MySql

snippet:MySqlConfig


include: sagafinder-thesaga

snippet: TheSaga

include: sagafinder-process


## SQL Persistence Helpers

DANGER: The current version (1.0) of the SQL Persistence has not been built with Saga Finders considered as a first class citizen. As such there are some hacks required to interface with the SQL Persistence conventions. This will be rectified in a future release.


### Saga Finder Helper

This class has an understanding of the saga storage conventions of the SQL Persistence and applies those conventions to generate SQL queries and return a Saga Data instance.

snippet: SqlPersistenceSagaFinder


### Serializer

Helper for serializing saga data and deserializing Saga Data and the Saga Metadata dictionary.

WANRING: If using [custom saga serialization logic](/nservicebus/sql-persistence/saga.md#json-net-settings) this class will require the same changes applied.

snippet: serializer


## Saga Finders

A Saga Finder is only required for the `PaymentTransactionCompleted` message since the other messages (`StartOrder` and `CompleteOrder`) are correlated based on `OrderSagaData.OrderId`.


### MS SQL Server

snippet:SqlServerSagaFinder


### MySql

snippet:MySqlSagaFinder
