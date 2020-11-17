---
title: Azure Cloud Showcase
summary: Implements a fictional store utilizing several features of NServiceBus.
reviewed: 2020-11-16
component: Core
redirects:
  - samples/show-case/cloud-azure
---
INFO: The sample targets WebJobs SDK 2.0

This sample implements a fictional store that can be deployed to Azure. It is different from most samples in that it shows many features of NServiceBus working together.

 1. Start the [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator). Ensure [latest version](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) is installed.
 1. Run the solution. 4 console windows start and one web-site opens.
 1. Order products from the website. Once orders are submitted, there is a window of time allocated for handling cancellations due to buyer's remorse. Once the order has been accepted, they are provisioned and made available for download. If the order is cancelled before the buyer's remorse timeout, nothing is provisioned for download.

include: showcase-walkthrough

## Feature usage

In implementing the above workflow various aspects are highlighted:


### Azure Storage Queues transport

All endpoints in the solution communicate using the [Azure Storage Queues transport](/transports/azure-storage-queues/). The sample has been configured to use the Azure Storage Emulator.


### Azure Table persistence

The endpoints in the solution persist data using the [Azure Table persistence](/persistence/azure-table/). This persistence is used to store subscription, timeout, and saga data.

NOTE: The Azure Table persistence does not support collection types. The `ProcessOrderSaga` avoids this issue by combining a collection of `ProductIds` into a single string for persistence.


### Azure Web Sites and Web Jobs

The `Ecommerce` project is configured to deploy as an [Azure Web Site](https://azure.microsoft.com/en-us/services/app-service/web/). The `ContentManagement`, `CustomerRelations`, `Operations`, and `Sales` endpoints are all configured to run as [Azure Web Jobs](https://docs.microsoft.com/en-us/azure/app-service-web/websites-webjobs-resources). The sample has been configured to run on the Azure Storage Emulator.

include: showcase-featureusage


## Deployment notes

This sample has been designed to run in a development environment under the Azure Storage Emulator. In order to deploy the sample to the cloud the following should be considered:

 1. Application settings to be created:
    * Application setting with the key `AzureWebJobsEnv`; must be set to a value other than `Development`.
    * Application setting with the key `AzureWebJobsDashboard`; must be set to a value other than `Development`.
    * Application setting with the key `AzureWebJobsStorage`; must be set to a value other than `Development`.
 1. An [Azure Storage account must be created](https://docs.microsoft.com/en-us/azure/storage/storage-create-storage-account#create-a-storage-account).
 1. By default, each endpoint is using the development storage. Application setting with the key `NServiceBus.ConnectionString` must be defined and its value set to the Storage Account connection string configured in step 1.

 * In order to support the elastic scale capabilities of the Azure platform, a [SignalR backplane](https://docs.microsoft.com/en-us/aspnet/signalr/overview/performance/scaleout-in-signalr) must be added to the `ECommerce` endpoint.
