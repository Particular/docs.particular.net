---
title: Azure Cloudservices Host Endpoint Configuration
summary: Configuring your endpoint when hosting in azure cloud services
tags:
- Azure
- Cloud
---

## Enabling the Transport

When using one of the NServiceBus provided hosting processes, the `UseTransport<T>` should be called on the endpoint configuration. For example using Azure Service Bus transport

snippet:AzureServiceBusTransportWithAzureHost


Example using azure storage queues transport:

snippet:AzureStorageQueueTransportWithAzureHost


## Enabling the Persistence

The Azure storage persistence can be enabled by specifying the `UsePersistence<AzureStoragePersistence>` on the endpoint config.

snippet:PersistenceWithAzureHost

NOTE: In Version 4, when hosting in the Azure role entrypoint provided by `NServiceBus.Hosting.Azure`, these persistence strategies will be enabled by default.