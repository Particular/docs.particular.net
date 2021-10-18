---
title: Azure Storage Queues Transport Upgrade Version 10 to 11
summary: Instructions on how to upgrade the Azure Storage Queues transport from version 10 to 11.
reviewed: 2021-03-12
component: ASQ
related:
- transports/azure-storage-queues
- nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Configuring the transport

To use the Azure Storage Queues transport for NServiceBus, create a new instance of `AzureStorageQueueTransport` and pass it to `EndpointConfiguration.UseTransport`.

Instead of:

```csharp
var transport = endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
transport.Transactions(TransportTransactionMode.ReceiveOnly);
var routing = t.Routing();
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

Use:

```csharp
var transport = new AzureStorageQueueTransport("azure-storage-connection-string")
{
    TransportTransactionMode = TransportTransactionMode.ReceiveOnly
};
var routing = endpointConfiguration.UseTransport(transport);
routing.RouteToEndpoint(typeof(MyMessage), "DestinationEndpoint");
```

include: v7-usetransport-shim-api

## Configuration options

The Azure Storage Queues transport configuration options have been moved to the `AzureStorageQueueTransport` class properties and constructors. See the following table for further information:

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
| AccountRouting                                        | AccountRouting                           |
| DefaultAccountAlias                                   | AccountRouting.DefaultAccountAlias       |
| DelayedDelivery                                       | DelayedDelivery                          |
| DelayedDelivery.UseTableName                          | DelayedDelivery.DelayedDeliveryTableName |
| DisableDelayedDelivery                                | use the transport constructor overload   |
| UseQueueServiceClient                                 | use the transport constructor overload   |
| UseBlobServiceClient                                  | use the transport constructor overload   |
| UseCloudTableClient                                   | use the transport constructor overload   |
