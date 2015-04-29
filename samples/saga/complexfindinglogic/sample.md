---
title: Complex Saga Finding Logic
summary: This sample shows how to perform custom saga finding logic based on mapping properties between a saga and messages.
tags:
- Saga
- SagaFinder
related:
- nservicebus/sagas
---

## Code walk-through

This sample shows how to perform custom saga finding logic based on mapping properties between a saga and messages.

At startup the sample will send `StartOrder`s with two different `OrderId`s. This will cause two sagas to start because `StartOrder` is configured as a `IAmStartedByMessages` and there is a mapping between `StartOrder.OrderId` and `OrderSagaData.OrderId`.

The output to the console will be

```
2015-02-11 22:34:59.475 INFO  OrderSaga Saga with OrderId 123 received StartOrder with OrderId 123
2015-02-11 22:34:59.526 INFO  OrderSaga Saga with OrderId 456 received StartOrder with OrderId 456
2015-02-11 22:34:59.572 INFO  OrderSaga Saga with OrderId 123 received CompleteOrder with OrderId 123
2015-02-11 22:34:59.606 INFO  OrderSaga Saga with OrderId 456 received CompleteOrder with OrderId 456
``` 

### The Saga

<!-- import thesaga -->
