---
title: Azure Storage Persistence
summary: Using Azure Storage to store Sagas, Timeouts and Subscriptions.
component: ASP
reviewed: 2017-08-01
tags:
- Saga
- Timeout
- Subscription
related:
- nservicebus/sagas
- nservicebus/azure
- persistence/azure-storage
---


## Prerequisites

Ensure that an instance of the latest [Azure Storage Emulator](https://docs.microsoft.com/en-us/azure/storage/storage-use-emulator) is running.


## Azure Storage Persistence

This sample utilizes the [Azure Storage Persistence](/persistence/azure-storage/).


## Code walk-through

This sample shows a simple Client + Server scenario.

 * `Client` sends a `StartOrder` message to `Server`
 * `Server` starts an `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
 * The Server then publishes a message that the client subscribes to.
 * `Client` handles `OrderCompleted` event.


### Azure Storage configuration

The `Server` endpoint is configured to use the Azure Storage persistence in two locations.


#### The endpoint configuration

snippet: Config


partial: AppConfig


### Order Saga Data

snippet: sagadata


### Order Saga

snippet: thesaga


## The Data in Azure Storage

The data in Azure Storage is stored in several locations.


### Reading the data using code

There are several helper methods in `AzureHelper.cs` in the `StorageReader` projects. These helpers are used to output the data seen below.


#### Writing table data

snippet: WriteOutTable


#### Writing blob data

snippet: WriteOutBlobContainer


#### Using the helpers

snippet: UsingHelpers


### The Saga Data

The saga data from the 'OrderSagaData' table contents

```
PartitionKey:= 21a6f7ed-65d2-42ff-a4d3-a50e00ea76ba
  RowKey:= 21a6f7ed-65d2-42ff-a4d3-a50e00ea76ba
  Id:= 21a6f7ed-65d2-42ff-a4d3-a50e00ea76ba
  Originator:= Samples.Azure.StoragePersistence.Client@RETINA
  OriginalMessageId:= 0d574aa7-0d39-4e93-8233-a50e00ea764f
  OrderId:= 79cc2072-c724-4cc0-9202-b6c4918a3de2
  OrderDescription:= The saga for order 79cc2072-c724-4cc0-9202-b6c4918...
```


### The Timeouts

The timeout data from the `TimeoutDataTableName` table

```
  PartitionKey:= 2015090918
    RowKey:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    Destination:= Samples.Azure.StoragePersistence.Server@RETINA
    Headers:= {"NServiceBus.MessageId":"06800d44-9fc4-49b5-a9e9-...
    OwningTimeoutManager:= Samples.Azure.StoragePersistence.Server
    SagaId:= 21a6f7ed-65d2-42ff-a4d3-a50e00ea76ba
    StateAddress:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    Time:= 9/09/2015 6:06:59 PM
```

The timeout serialized message from the `timeoutstate` blob container.

```
'timeoutstate' container contents
  Blob:= 06800d44-9fc4-49b5-a9e9-a50e00ea76c0
    ﻿{"OrderDescription":"The saga for order 79cc2072-c724-4cc0-9202-b6c4918a3de2"}
```


### The Subscriptions

The Client endpoint registered in the `Subscription` table contents

```
PartitionKey:= OrderCompleted, Version=0.0.0.0
  RowKey:= U2FtcGxlcy5BenVyZS5TdG9yYWdlUGVyc2lzdGVuY2UuQ2xpZW50QFJFVElOQQ==
  DecodedRowKey:= Samples.Azure.StoragePersistence.Client@RETINA
```
