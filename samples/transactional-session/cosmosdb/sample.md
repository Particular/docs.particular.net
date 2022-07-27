---
title: Using Transactional Session with CosmosDB
reviewed: 2022-07-20
component: Core
related:
- nservicebus/messaging/transactionalsession
---

This sample uses the transactional session package with the CosmosDB persistence package to achieve transactionally consistent database changes and message operations.

downloadbutton

## Prerequisties

The sample is intended to be used with the CosmosDB emulator to run locally. Alternatively, a connection string to an Azure CosmosDB instance can be provided.

## Configuration

To enable transactional consistency between database operations and message operations, the endpoint needs to be configured with Outbox enabled:

TODO

Using multi-document transactions requires the endpoint to be configured with a partition key mapping to ensure that documents and outbox data are stored on the same partition key:

TODO

The transactional session also requires information about the partition key:

WARN: These settings are mandatory and not configuring the paritition keys correctly can lead to message loss!