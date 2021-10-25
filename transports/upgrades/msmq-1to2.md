---
title: MSMQ Transport Upgrade Version 1 to 2
summary: Migration instructions on how to upgrade the MSMQ transport from version 1 to 2.
reviewed: 2021-02-11
component: MsmqTransport
related:
- transports/msmq
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Configuring the MSMQ transport

To use the MSMQ transport for NServiceBus, create a new instance of `MsmqTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of:

```csharp
var transport = endpointConfiguration.UseTransport<MsmqTransport>();
transport.ConnectionString(connectionString);
```

Use:

```csharp
var transport = new MsmqTransport();
endpointConfiguration.UseTransport(transport);
```

include: v7-usetransport-shim-api

## Delayed delivery

Version 2 supports delayed delivery of messages by persisting them in a delayed message store. There is a built-in SQL Server-based store and an extension point for custom stores.

In version 2, explicit configuration is required to enable delayed message delivery. For example:

snippet: delayed-delivery

When upgrading from version 1 to 2, any currently delayed messages must be migrated using the [timeout migration tool](/nservicebus/tools/migrate-to-native-delivery.md).

## Configuration options

The MSMQ transport configuration options have been moved to the `MsmqTransport` class. See the following table for further information:

| Version 1 configuration option | Version 2 configuration option |
| --- | --- |
| ApplyLabelToMessages | ApplyCustomLabelToOutgoingMessages |
| TransactionScopeOptions | ConfigureTransactionScope |
| UseDeadLetterQueueForMessagesWithTimeToBeReceived | UseDeadLetterQueueForMessagesWithTimeToBeReceived |
| DisableInstaller | CreateQueues |
| DisableDeadLetterQueueing | UseDeadLetterQueue |
| DisableConnectionCachingForSends | UseConnectionCache |
| UseNonTransactionalQueues | UseTransactionalQueues |
| EnableJournaling | UseJournalQueue |
| TimeToReachQueue | TimeToReachQueue |
| DisableNativeTimeToBeReceivedInTransactions | UseNonNativeTimeToBeReceivedInTransactions |
| IgnoreIncomingTimeToBeReceivedHeaders | IgnoreIncomingTimeToBeReceivedHeaders |
