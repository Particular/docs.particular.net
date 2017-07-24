---
title: SQL Persistence Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is the native SQL Persistence
component: SqlPersistence
reviewed: 2017-04-16
tags:
- Saga
- SagaFinder
related:
- nservicebus/sagas
- persistence/sql
---

include: sagafinder-into


include: sqlpersistence-prereqs


## Persistence Config

Configure the endpoint to use SQL Persistence.


### MS SQL Server

snippet: sqlServerConfig


### MySql

snippet: MySqlConfig


include: sagafinder-thesaga

snippet: saga

include: sagafinder-process


## Saga Finders

A Saga Finder is only required for the `PaymentTransactionCompleted` message since the other messages (`StartOrder` and `CompleteOrder`) are correlated based on `OrderSagaData.OrderId`.


### MS SQL Server

WARNING: On Microsoft SQL Server, the saga finder feature requires the `JSON_VALUE` function that is only available starting with SQL Server 2016.

snippet: SqlServerFinder


### MySql

snippet: MySqlFinder
