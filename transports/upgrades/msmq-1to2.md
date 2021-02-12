---
title: MSMQ Transport Upgrade Version 1 to 2
summary: Migration instructions on how to upgrade MSMQ Transport from Version 1 to 2.
reviewed: 2021-02-11
component: MSMQ
related:
- transports/msmq
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Configuring MSMQ Transport

To use the MSMQ Transport for NServiceBus, create a new instance `MsmqTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of

```csharp
var transport = endpointConfiguration.UseTransport<MsmqTransport>();
transport.ConnectionString(connectionString);
```

Use:

```csharp
var transport = new MsmqTransport();
endpointConfiguration.UseTransport(transport);
```

## Configuration Options

The MSMQ Transport configuration options have been moved to the `MsmqTransport` class. See the following table for further information:

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