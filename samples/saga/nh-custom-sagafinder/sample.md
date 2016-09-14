---
title: NHibernate Custom Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is a relational database using NHibernate as the O/RM.
component: NHibernate
reviewed: 2016-03-21
tags:
- Saga
- SagaFinder
- NHibernate
related:
- nservicebus/sagas
- nservicebus/nhibernate
---

## Code walk-through

When the default Saga message mappings do not satisfy the requirements, custom logic can be put in place to allow NServiceBus to find a saga data instance based on which logic best suits the  environment.

This sample shows how to perform custom saga finding logic based on custom query logic.


### NHibernate setup

This sample requires [NHibernate persistence](https://www.nuget.org/packages/NServiceBus.NHibernate/) package and a running Microsoft SQL Server instance configured accordingly. The sample NHibernate setup can be configured according to the environment:

snippet: NHibernateSetup


### The Saga

The saga shown in the sample is a very simple order management saga that:

 * handles the creation of an order;
 * offloads the payment process to a different handler;
 * handles the completion of the payment process;
 * completes the order;

snippet: TheSagaNHibernate

From the process point of view it is important to note that the saga is not sending to the payment processor the order id, instead it is sending a payment transaction id. A saga can be correlated given more than one unique attribute, such as `OrderId` and `PaymentTransactionId`, requiring both to be treated as unique.

At start-up the sample will send a `StartOrder` message. Building a saga finder requires to define a class that implements the `IFindSagas<TSagaData>.Using<TMessage>` interface. The class will be automatically picked up by NServiceBus at configuration time and used each time a message of type `TMessage`, that is expected to load a saga of type `TSagaData`, is received. The `FindBy` method will be invoked by NServiceBus. The responsibility to query the saga storage looking for the saga instance is owned by the finder:

snippet:CustomSagaFinderNHibernate

NOTE: In the sample the implementation of the `ConfigureHowToFindSaga` method, that is required, is empty because a saga finder for each message type, that the saga can handle, is being provided. It is not required to provide a saga finder for every message type, a mix of standard saga mappings and custom saga finding is a valid scenario.