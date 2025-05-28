---
title: Custom audit transport
summary: Using ASQ for auditing while using ASB as the main transport
reviewed: 2025-05-28
component: Core
---

This sample shows how an endpoint can be using one transport, and utlising a different transport for auditing.
In this instance, Azure Service Bus is the main transport, and Azure Storage Queues is used as the transport for audit.

## Prerequisites

Ensure that an instance of the latest [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) is running.

## Projects

### CustomAuditTransport







