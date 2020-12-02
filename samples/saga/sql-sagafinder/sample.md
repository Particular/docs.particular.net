---
title: SQL Persistence Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is the native SQL Persistence
component: SqlPersistence
reviewed: 2020-12-02
related:
- nservicebus/sagas
- persistence/sql
- persistence/sql/saga
- persistence/sql/saga-finder
---

include: sagafinder-intro


include: sqlpersistence-prereqs


## Persistence Config

Configure the endpoint to use SQL Persistence.


### MS SQL Server

snippet: sqlServerConfig


### MySql

snippet: MySqlConfig

partial: postgresconfig


include: sagafinder-thesaga

snippet: saga

include: sagafinder-process


## Saga Finders

A Saga Finder is only required for the `PaymentTransactionCompleted` message since the other messages (`StartOrder` and `CompleteOrder`) are correlated based on `OrderSagaData.OrderId`.


### MS SQL Server

include: sql-saga-finder-warning

snippet: SqlServerFinder


### MySql

snippet: MySqlFinder

partial: postgresfinder
