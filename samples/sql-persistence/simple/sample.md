---
title: Simple Sql Persistence Usage
summary: Using Sql Persistence to store Sagas and Timeouts.
reviewed: 2016-03-21
component: SqlPersistence
tags:
 - Saga
 - Timeout
related:
 - nservicebus/sagas
reviewed: 2016-10-05
---


## Code walk-through

This sample shows a simple Client + Server scenario.

 * `Client` sends a `StartOrder` message to `Server`.
 * `Server` starts an `OrderSaga`.
 * `OrderSaga` requests a timeout with a `CompleteOrder` data.
 * When the `CompleteOrder` timeout fires the `OrderSaga` publishes a `OrderCompleted` event.
 * The Server then publishes a message that the client subscribes to.
 * `Client` handles `OrderCompleted` event.


### Sql Persistence Config

Configure the endpoint to use Sql Persistence persistence.

snippet:config


### Order Saga Data

snippet:sagadata


### Order Saga

snippet:thesaga
