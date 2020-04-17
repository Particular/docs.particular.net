---
title: Transitioning Saga Correlation IDs
summary: An approach for transitioning between different correlation IDs with no downtime
reviewed: 2020-04-17
component: SqlPersistence
related:
 - nservicebus/sagas
---


This sample illustrates an approach for transitioning between different [correlation IDs](/persistence/sql/saga.md#correlation-ids) in a way that requires no endpoint downtime and no migration of saga data stored in sql.

NOTE: The sample uses three "Phase" Endpoint Projects to illustrate the iterations of a single endpoint in one solution.


include: sqlpersistence-prereqs


## Scenario

The sample uses a hypothetical "Order" scenario where the requirement is to transition from an an int correlation ID `OrderNumber` to a GUID correlation ID `OrderId`. 

To move between phases, after running each phase adjust the startup project list in solution properties. E.g. after phase 1, disable endpoints that contain "Phase1" in the name, and enable endpoints that contain "Phase2".


## Phases


### Phase 1

In the initial phase an int `OrderNumber` is used for the correlation ID. In the `ConfigureHowToFindSaga` method, the saga maps `StartOrder.OrderNumber` from the incoming message to `OrderSagaData.OrderNumber` in the saga data.


#### Message

snippet: messagePhase1


#### Saga

snippet: sagaPhase1


#### SagaData

snippet: sagadataPhase1


### Phase 2

In the second phase a GUID `OrderId` is added. The saga still maps `StartOrder.OrderNumber` to `OrderSagaData.OrderNumber` in the `ConfigureHowToFindSaga` method. However it also introduces a correlation to `OrderSagaData.OrderId` via a `transitionalCorrelationProperty` in the `[SqlSaga]` attribute.


#### Message

snippet: messagePhase2


#### Saga


snippet: sagaPhase2


#### SagaData

snippet: sagadataPhase2


WARNING: Prior to moving to Phase 3 it is necessary to verify that all existing sagas have the `Correlation_OrderId` column populated. This can either be inferred by the business knowledge (i.e. certain saga may have a known and constrained lifetime) or by querying the database.


### Phase 3

In the third phase, the int `OrderNumber` is removed leaving only the `OrderId`. The saga now maps `StartOrder.OrderId` to `OrderSagaData.OrderId` in the `ConfigureHowToFindSaga` method.


#### Message

snippet: messagePhase3


#### Saga

snippet: sagaPhase3


#### SagaData

snippet: sagadataPhase3
