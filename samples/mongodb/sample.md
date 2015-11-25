---
title: MongoDB persistence Sample
summary: This sample shows how use MongoDB to store Sagas and Timeouts.
tags:
- Saga
- Timeouts
related:
- nservicebus/sagas
---


## Prerequisites 

Ensure you have an instance of [MongoDB](https://www.mongodb.org/) running on `localhost:27017`. See [Install MongoDB on Windows](http://docs.mongodb.org/getting-started/shell/tutorial/install-mongodb-on-windows/). 


### MongoDB Persistence for NServiceBus 

This sample utilizes the community run [NServiceBus.MongoDB project](https://github.com/sbmako/NServiceBus.MongoDB).


### MongoDB Management UI

To visualize the data in MongoDB it is useful to have install a [MongoDB administration tool](http://docs.mongodb.org/ecosystem/tools/administration-interfaces/). The screen shots shown in this sample use [Robomongo](http://www.robomongo.org/).


## Code walk-through

This sample shows a simple Client + Server scenario. 

* `Client` sends a `StartOrder` message to `Server`
* `Server` starts an `OrderSaga`. 
* `OrderSaga` requests a timeout with a `CompleteOrder` data.
* When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
* The Server then publishes a message that the client subscribes to.
* `Client` handles `OrderCompleted` event.


### MongoDb configuration

The `Server` endpoint is configured to use the MongoDB persistence with a connection string of `mongodb://localhost:27017`.

snippet:mongoDbConfig


### Order Saga Data

The NServiceBus.MongoDB persistence [requires that saga data types implement IHaveDocumentVersion](https://github.com/sbmako/NServiceBus.MongoDB#sagas).

> Saga data needs to be defined the normal way NSB requires with the additional interface IHaveDocumentVersion to work appropriately with NServiceBus.MongoDB. All this interface adds is a version property. Alternatively you can just inherit from ContainMongoSagaData.

snippet:sagadata


### Order Saga

snippet:thesaga


## The Data in MongoDB

The data in MongoDb is stored in three different collections.


### The Saga Data 

 * `IContainSagaData.Id` maps to the native MongoDB document `_id`
 * `IContainSagaData.Originator` and `IContainSagaData.OriginalMessageId` map to simple properties pairs.
 * Custom properties on the SagaData, in this case `OrderDescription` and `OrderId`, are also mapped to simple properties.
 * `_t` is type serialization metadata use by the underlying MongoDB Driver.
 * `DocumentVersion` used by NServiceBus.MongoDB to prevent concurrency issues.

![](sagadata.png)


### The Timeouts 

  * The subscriber is stored in a `Destination` with the nested properties `Queue` and `Machine`.
  * The endpoint that initiated the timeout is stored in the `OwningTimeoutManager` property
  * The connected saga id is stored in a `SagaId` property.
  * The serialized data for the message is stored in a `State` property.
  * The scheduled timestamp for the timeout is stored in a `Time` property.
  * Any headers associated with the timeout are stored in an array of key value pairs.  

![](timeouts.png)


### The Subscriptions

Note that the message type maps to multiple subscriber endpoints.

 * The Subscription message type and version are stored under the `MessageType` property.
 * `DocumentVersion` used by NServiceBus.MongoDB to prevent concurrency issues.
 * The list of subscribers is stored in a array of objects each containing `Queue` and `MachineName` properties. 

![](subscriptions.png)
