---
title: Azure Storage Queues Transport Upgrade Version 9 to 10
summary: Migration instructions on how to upgrade Azure Storage Queues Transport from Version 9 to 10.
reviewed: 2021-02-11
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 8
---

## Configuring the transport

Upgrading from NServiceBus.Azure.Transports.WindowsAzureStorageQueues version 9 to 10 requires code changes to use the new transport API.

The transport configuration API has been changed. Instead of the generic based `UseTransport<AzureStorageQueueTransport>` method, create an instance of the transport's configuration class and pass it to `UseTransport`, e.g.

```csharp
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);
var routing = t.Routing();
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

becomes:

```csharp
var transport = new AzureStorageQueueTransport("azure-storage-connection-string")
{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

## Configuration Options

The Azure Queue Storage transport configuration options have been moved to the `AzureStorageQueueTransport` class properties and constructor(s). See the following table for further information:

| Version 9 configuration option                        | Version 10 configuration option          |
| ----------------------------------------------------- | -----------------------------------------|
| MessageInvisibleTime                                  | MessageInvisibleTime                     |
| PeekInterval                                          | PeekInterval                             |
| MaximumWaitTimeWhenIdle                               | MaximumWaitTimeWhenIdle                  |
| SanitizeQueueNamesWith                                | QueueNameSanitizer                       |
| BatchSize                                             | ReceiverBatchSize                        |
| DegreeOfReceiveParallelism                            | DegreeOfReceiveParallelism               |
| SerializeMessageWrapperWith<TSerializationDefinition> | MessageWrapperSerializationDefinition    |
| UnwrapMessagesWith                                    | MessageUnwrapper                         |
| UseQueueServiceClient                                 | use the transport constructor overload   |
| UseBlobServiceClient                                  | use the transport constructor overload   |
| UseCloudTableClient                                   | use the transport constructor overload   |
| DelayedDelivery                                       | DelayedDelivery                          |
| AccountRouting                                        | AccountRouting                           |
| DefaultAccountAlias                                   | AccountRouting.DefaultAccountAlias       |
| UseTableName                                          | DelayedDelivery.DelayedDeliveryTableName |
| DisableDelayedDelivery                                | use the transport constructor overload   |
