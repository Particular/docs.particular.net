---
title: Custom Saga Finding Logic (NHibernate)
summary: This sample shows how to perform custom saga finding logic based on custom query logic when the Saga storage is a relational database using NHibernate as the O/RM.
tags:
- Saga
- SagaFinder
- NHibernate
related:
- nservicebus/sagas
- nservicebus/nhibernate
---

## Code walk-through

When the default Saga message mappings do not satisfy our needs custom logic can be put in place to allow NServiceBus to find a saga data instance based on which logic best suits our environment.

This sample shows how to perform custom saga finding logic based on custom query logic.


### NHibernate setup

This sample requires [NHibernate persistence](https://www.nuget.org/packages/NServiceBus.NHibernate/) package and a running Microsoft Sql Server instance configured accordingly. The sample NHibernate setup can be configured according to your environment:

snippet: NHibernateSetup


### The Saga

The saga shown in the sample is a very simple order management saga that:

 * handles the creation of an order;
 * offloads the payment process to a different handler;
 * handles the completition of the payment process;
 * completes the order;

snippet: TheSagaNHibernate

From the process point of view is important to notice that the saga is not sending to the payment processor the order id instead is sending a payment transaction id, in this scenario we are simulating the fact that a saga can be correlated given more than one unique attribute, such as `OrderId` and `PaymentTransactionId` requiring both to be treated as unique.

At start-up the sample will send a `StartOrder` message. Building a saga finder requires to define a class that implements the `IFindSagas<TSagaData>.Using<TMessage>` interface. The class will be automatically picked up by NServiceBus at configuration time and used each time a message of type `TMessage`, that is expected to load a saga of type `TSagaData`, is received. The `FindBy` method will be invoked by NServiceBus. It is our responsibility to query the saga storage looking for the saga instance:

snippet:CustomSagaFinderNHibernate

NOTE: In the sample the implementation of the `ConfigureHowToFindSaga` method, that is required, is empty because we are providing a saga finder for each message type that the saga is handling. It is not required to provide a saga finder for every message type, a mix of standard saga mappings and custom saga finding is a valid scenario.