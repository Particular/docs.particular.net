---
title: Transactional Session with Azure Storage Persistence
summary: How to configure the transactional session with Azure Storage Persistence
component: TransactionalSession
reviewed: 2022-09-12
related:
- persistence/azure-table
redirects:
---

In order to use the TransactionalSession feature with Azure Storage Persistence, add a reference to the `NServiceBus.Persistence.AzureTable.TransactionalSession` NuGet package.

## Configuration

To enable the TransactionalSession feature:

snippet: enabling-transactional-session-azurestorage

## Opening a session

To open a Azure Storage Persistence transactional session:

snippet: open-transactional-session-azurestorage

## Configuring the table

The name of the destination table can be specified when opning the session:

snippet: open-transactional-session-azurestorage-table