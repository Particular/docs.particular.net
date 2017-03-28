---
title: Simple Saga Usage
summary: How build and use a saga.
reviewed: 2017-02-17
component: Core
tags:
- Saga
- SagaFinder
redirects:
- samples/saga/complexfindinglogic
related:
- nservicebus/sagas
---

## Code walk-through

This sample shows a simple saga.

At startup the sample will send two `StartOrder` messages with different identifers for `OrderId`. This will cause two saga instances to start because `StartOrder` is configured to start a saga using the `IAmStartedByMessages` construct. There is also a mapping defined between `StartOrder.OrderId` and `OrderSagaData.OrderId`. This mapping helps to [correlate incoming messages](/nservicebus/sagas/message-correlation.md) to its appropriate saga instances. 

This sample also requests a 30 minute `CancelOrder` timeout that will mark the saga as complete if the saga is not already complete.

The output to the console will be

```no-highlight
2015-02-11 22:34:59.475 INFO  OrderSaga Saga with OrderId 1 received StartOrder with OrderId 1
2015-02-11 22:34:59.526 INFO  OrderSaga Saga with OrderId 2 received StartOrder with OrderId 2
2015-02-11 22:34:59.572 INFO  OrderSaga Saga with OrderId 1 received CompleteOrder with OrderId 1
2015-02-11 22:34:59.606 INFO  OrderSaga Saga with OrderId 2 received CompleteOrder with OrderId 2
```


### The Saga

snippet: thesaga
