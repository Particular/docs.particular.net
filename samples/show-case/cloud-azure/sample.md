---
title: Azure Cloud Show Case
summary: Implements a fictional store utilizing several features of NServiceBus.
component: Core
---

This sample implements a fictional store that can be deployed to Azure. It is different from most samples in that it shows many features of NServiceBus working together.

1. Start the [Azure Storage Emulator](https://azure.microsoft.com/en-us/documentation/articles/storage-use-emulator/). Ensure [latest version](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) is installed.
2. Run the solution. 4 console windows start and one web-site opens.
3. Order products from the website. Once orders are submitted, there is a window of time allocated for handling cancellations due to buyer's remorse. Once the order has been accepted, they are provisioned and made available for download. If the order is cancelled before the buyer's remorse timeout, nothing is provisioned for download.


## Feature usage


### Azure Storage Queues Transport

All endpoints in the solution communicate using the [Azure Storage Queues transport](/nservicebus/azure-storage-queues/). The sample has been configured to use the Azure Storage Emulator. 


### Azure Storage Persistence

The endpoints in the solution persist data using the [Azure Storage persistence](/nservicebus/azure-storage-persistence/). This persistence is used to store subscription, timeout, and saga data.

NOTE: The Azure Storage persistence does not support collection types. The `ProcessOrderSaga` avoids this issue by combining a collection of `ProductIds` into a single string for persistence.


### Azure Web Sites and Web Jobs

The `Ecommerce` project is configured to deploy as an [Azure Web Site](https://azure.microsoft.com/en-us/services/app-service/web/). The `ContentManagement`, `CustomerRelations`, `Operations`, and `Sales` endpoints are all configured to run as [Azure Web Jobs](https://docs.microsoft.com/en-us/azure/app-service-web/websites-webjobs-resources). The sample has been configured to run on the Azure Storage Emulator. 


### Sagas

Illustrates the use of the Saga pattern to handle the buyer's remorse scenario.


### Request / Response

The request/response pattern is illustrated for the product provisioning between the ContentManagement endpoint and the Operations Endpoint.


### ASP MVC and SignalR

The ECommerce endpoint is implemented as an ASP.NET application which uses SignalR to show feedback to the user.


### Message Mutator

The use of message headers and message mutator is also illustrated when the user clicks on the Check box on the ECommerce web page, which conveniently stops at the predefined breakpoints in the message handler code on the endpoints.


### Encryption

The use of encryption is illustrated by passing in the Credit Card number and the Expiration date from the ECommerce web site. The UnobtrusiveConventions defined in the ECommerce endpoint show how to treat certain properties as encrypted. Both the ECommerce and the Sales endpoint is setup for RijndaelEncryption and the encryption key is provided in the config file. If the messages are inspected in the queue, both the Credit Card number and the Expiration Date will show the encrypted values. 


## Deployment Notes

This sample has been designed to run in a development environment under the Azure Storage Emulator. In order to deploy the sample to the cloud the following should be considered:

1. An [Azure Storage account must be created](https://docs.microsoft.com/en-us/azure/storage/storage-create-storage-account#create-a-storage-account).  
2. Each endpoint is hardcoded to use the development storage. The connection string passed into the Azure Storage Queues transport and the Azure Storage persistence in `CommonConfiguration.ApplyCommonConfiguration()` must be updated to point to the Storage Account configured in step 1.
3. In order to support the elastic scale capabilities of the Azure platform, a [SignalR backplane](https://www.asp.net/signalr/overview/performance/scaleout-in-signalr) must be added to the `ECommerce` endpoint. 