---
title: Simple Saga Usage
summary: How build and use a saga.
reviewed: 2017-05-10
component: Core
tags:
- Saga
- SagaFinder
redirects:
- samples/saga/complexfindinglogic
related:
- nservicebus/sagas
- persistence/learning
- transports/learning
---


## Introduction

This sample shows a simple saga.

Once starting the sample, press `Enter` to send a `StartOrder` message with a random `OrderId`. Each message will cause a saga instance to start because `StartOrder` is configured to start a saga using the `IAmStartedByMessages` construct. There is also a mapping defined between `StartOrder.OrderId` and `OrderSagaData.OrderId`. This mapping helps to [correlate incoming messages](/nservicebus/sagas/message-correlation.md) to its appropriate saga instances.

This sample also requests a 30 second `CancelOrder` timeout that will mark the saga as complete if the saga is not already complete.

The output to the console will be

```
Storage locations:
Learning Persister: SolutionDir\Sample\bin\Debug\.sagas
Learning Transport: SolutionDir\

Press 'Enter' to send a StartOrder message
Press any other key to exit

Sent StartOrder with OrderId 8d80b684-cc77-4ec6-867b-090bc38d914c.

OrderSaga StartOrder received with OrderId 8d80b684-cc77-4ec6-867b-090bc38d914c
OrderSaga Sending a CompleteOrder that will be delayed by 10 seconds
Stop the endpoint now to see the saga data in:
SolutionDir\Sample\bin\Debug\.sagas\OrderSaga\f5e7ea90-b866-16a0-c911-452f954b95da.json
OrderSaga Requesting a CancelOrder that will be executed in 30 seconds.
Stop the endpoint now to see the timeout data in the delayed directory
SolutionDir\.delayed\20170510054055
```


## Endpoint configuration

snippet: config


## The Saga

snippet: thesaga


## Location Helper

This is a helper class used by the sample to derive the storage locations of the [Learning Transport](/transports/learning/) and the [Learning Persistence](/persistence/learning/).

snippet: LearningLocationHelper