---
title: Transitioning to the Azure Storage Queues Transport
summary: Demonstrates the required changes to switch your POC from the Learning Transport to the Azure Storage Queues Transport
reviewed: 2019-04-24
component: ASQ
tags:
 - Azure
 - Transport
related:
 - transports/azure-storage-queues
 - samples/azure/storage-queues
---

The Learning transport is not a production-ready transport, but is intended for learning the NServiceBus API and creating demos/proof-of-concepts. A number of changes to endpoint configuration are required to transition an endpoint from the Learning Transport to the Azure Storage Queues Transport.


## Install the NuGet Package

The [NServiceBus.Azure.Transports.WindowsAzureStorageQueues](https://www.nuget.org/packages/NServiceBus.Azure.Transports.WindowsAzureStorageQueues/) must be installed. This can be accomplished using the [NuGet Package Manager UI](https://docs.microsoft.com/en-us/nuget/tools/package-manager-ui) or run this command from the [NuGet Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console):

```
   Install-Package NServiceBus.Azure.Transports.WindowsAzureStorageQueues
```


## Change the Endpoint Configuration

To use the Azure Storage Queues Transport first update the `UseTransport` call:

```c#
-   endpointConfiguration.UseTransport<LearningTransport>();
+   endpointConfiguration.UseTransport<AzureStorageQueueTransport>();
```


### Set a Connection String

The Azure Storage Queues Transport requires a connection string be specified:

snippet: AzureStorageQueueTransportWithAzure


### Enable Installers

The endpoint must enable installers to allow the Azure Storage Queues transport to create the necessary [queues](https://docs.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues#queue-service-concepts) in the Azure Storage Account for your endpoint.

snippet: Installers

### Configure a Persistence

This is because the Azure Storage Queues Transport, unlike the Learning Transport, does not natively support Publish/Subscribe and instead uses [message-driven Pub/Sub](/nservicebus/messaging/publish-subscribe.md#mechanics-message-driven-persistence-based), so the message subscription information must be stored.

snippet: PersistenceWithAzure

[Azure Storage Persistence](/persistence/azure-storage) is shown here as an example, however use the [selecting a persister](/persistence/selecting.md) guide to help determine the appropriate persistence for the endpoint.


include: registerpublishers
