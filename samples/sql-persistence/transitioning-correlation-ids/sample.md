---
title: Transitioning Saga Correlation Ids
reviewed: 2017-01-03
component: SqlPersistence
related:
 - nservicebus/sagas
---


This sample illustrates an approach for transitioning between different [Correlation Ids](/persistence/sql/saga.md#correlation-ids) in way that requires no endpoint downtime and no migration of saga data stored in sql.

NOTE: This sample uses 3 "Phase" Endpoint Projects to illustrate the iterations of a single endpoint in one solution.


include: sqlpersistence-prereqs


## Scenario

This samples uses a hypothetical "Order" scenario where the requirement is to transition from an an int correlation id `OrderNumber` to a guid correlation id `OrderId`. 


## Phases


### Phase 1

In the initial phase an int `OrderNumber` is used. The saga maps to `StartOrder.OrderNumber` in the `ConfigureMapping` and correlates to `OrderSagaData.OrderNumber` via a `SqlSagaAttribute` with `correlationProperty` of `OrderNumber`.


#### Message

snippet: messagePhase1


#### Saga

snippet: sagaPhase1


#### SagaData

snippet: sagadataPhase1


### Phase 2

In the second phase a guid `OrderId` is added. The saga still maps to `StartOrder.OrderNumber` in the `ConfigureMapping` and correlates on to `OrderSagaData.OrderNumber`. However it also correlates to `OrderSagaData.OrderId` via a `transitionalCorrelationProperty`.


#### Message

snippet: messagePhase2


#### Saga


snippet: sagaPhase2


#### SagaData

snippet: sagadataPhase2


WARNING: Prior to moving to Phase 3 it is necessary to verify that all existing sagas have the `Correlation_OrderId` column populated. This can either be inferred by the business knowledge (i.e. certain saga may have a know and constrained lifetime) or by querying the database.


### Phase 3

In the third phase the int `OrderNumber` is removed leaving only the `OrderId`. The saga now maps to `StartOrder.OrderId` in the `ConfigureMapping` and correlates to `OrderSagaData.OrderId` via a `SqlSagaAttribute` with `correlationProperty` of `OrderNumber`.


#### Message

snippet: messagePhase3


#### Saga

snippet: sagaPhase3


#### SagaData

snippet: sagadataPhase3
