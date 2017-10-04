---
title: Azure Storage Persistence
summary: Using Azure Storage to store Sagas, Timeouts and Subscriptions.
component: ASP
reviewed: 2017-10-03
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

partial: sagadata


### The Timeouts

partial: timeouts


### The Subscriptions

The Client endpoint registered in the `Subscription` table contents

partial: subscriptions