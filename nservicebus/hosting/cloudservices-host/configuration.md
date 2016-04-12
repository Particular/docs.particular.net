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