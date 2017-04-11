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
- nservicebus/sql-persistence
---

include: sagafinder-into


## Prerequisites

partial: prereqs


## Persistence Config

Configure the endpoint to use SQL Persistence.


### MS SQL Server

snippet: sqlServerConfig


### MySql

snippet: MySqlConfig


include: sagafinder-thesaga

snippet: TheSaga

include: sagafinder-process


## Saga Finders

A Saga Finder is only required for the `PaymentTransactionCompleted` message since the other messages (`StartOrder` and `CompleteOrder`) are correlated based on `OrderSagaData.OrderId`.


### MS SQL Server

snippet: SqlServerSagaFinder


### MySql

snippet: MySqlSagaFinder
