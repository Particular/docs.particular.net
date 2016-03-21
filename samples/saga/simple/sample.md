---
title: Saga Sample
summary: How build and use a saga.
reviewed: 2016-03-21
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

At startup the sample will send two `StartOrder`s with different `OrderId`s. This will cause two sagas to start because `StartOrder` is configured as a `IAmStartedByMessages` and there is a mapping between `StartOrder.OrderId` and `OrderSagaData.OrderId`.

The ample also requests a 30min `CancelOrder` timeout that will mark the saga as complete if the saga is not already complete.

The output to the console will be

```
2015-02-11 22:34:59.475 INFO  OrderSaga Saga with OrderId 123 received StartOrder with OrderId 123
2015-02-11 22:34:59.526 INFO  OrderSaga Saga with OrderId 456 received StartOrder with OrderId 456
2015-02-11 22:34:59.572 INFO  OrderSaga Saga with OrderId 123 received CompleteOrder with OrderId 123
2015-02-11 22:34:59.606 INFO  OrderSaga Saga with OrderId 456 received CompleteOrder with OrderId 456
```


### The Saga

snippet:thesaga