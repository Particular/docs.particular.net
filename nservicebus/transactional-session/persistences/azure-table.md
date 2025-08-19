---
title: Transactional Session with Azure Table Persistence
summary: How to configure a transactional session with Azure Table Persistence
component: TransactionalSession.AzureTable
reviewed: 2025-01-22
related:
- persistence/azure-table
- nservicebus/transactional-session
redirects:
---

In order to use the [transactional session feature](/nservicebus/transactional-session/) with Azure Table Persistence, add a reference to the `NServiceBus.Persistence.AzureTable.TransactionalSession` NuGet package.

## Configuration

To enable the transactional session feature:

snippet: enabling-transactional-session-azurestorage

## Opening a session

To open an Azure Storage Persistence transactional session:

snippet: open-transactional-session-azurestorage

### Configuring the table

The name of the destination table can be specified when opening the session:

snippet: open-transactional-session-azurestorage-table

## Transaction usage

Message and database operations made via the transactional session are committed together once the session is committed:

snippet: use-transactional-session-azurestorage

See the [Azure table persistence transactions documentation](/persistence/azure-table/transactions.md#sharing-the-transaction) for further details about using the transaction.

include: ts-outbox-warning
