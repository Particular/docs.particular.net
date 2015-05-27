---
title: Custom Saga Finding Logic (RavenDB)
summary: This sample shows how to perform custom saga finding logic based on custom query logic when the Saga storage is RavenDB and how to use multiple Unique attributes.
tags:
- Saga
- SagaFinder
- RavenDB
related:
- nservicebus/sagas
- nservicebus/ravendb
---

## Solution Structure

The solution contains two projects. 

### RavenServer

This is a simple console app that starts a Raven Embedded server. This only exists so the sample can run without needing a RavenDB instance installed on your machine. This project also includes the `RavenDB.Bundles.UniqueConstraints` [bundle](http://ravendb.net/docs/article-page/2.5/csharp/server/extending/bundles/unique-constraints).

### Sample

Contains the actual endpoint code.

## Code walk-through

When the default Saga message mappings do not satisfy our needs custom logic can be put in place to allow NServiceBus to find a saga data instance based on which logic best suites our environment.

This sample shows:

* how to perform custom saga finding logic based on custom query logic;
* how to use multiple Unique attributes using the default [RavenDB Unique Constraint bundle](http://ravendb.net/docs/article-page/2.5/csharp/server/extending/bundles/unique-constraints).

### RavenDB setup

This sample requires [RavenDB persistence](http://www.nuget.org/packages/NServiceBus.RavenDB/) package and a running RavenDB instance configured accordingly. The sample RavenDB setup can be configured according to your environment:

<!-- import RavenDBSetup --> 

NServiceBus out of the box does not support saga data with multiple `Unique` attributes, in order to achieve that it is possible to utilize the default RavenDB `UniqueConstraint` Bundle. Follow the [instructions on the RavenDB site](http://ravendb.net/docs/article-page/2.5/csharp/server/extending/bundles/unique-constraints) to correctly install the bundle in your RavenDB server.

### The Saga

The saga shown in the sample is a very simple order management saga that:

* handles the creation of an order;
* offloads the payment process to a different handler;
* handles the completition of the payment process;
* completes the order;

<!-- import TheSagaRavenDB -->

From the process point of view is important to notice that the saga is not sending to the payment processor the order id instead is sending a payment transaction id, in this scenario we are simulating the fact that a saga can be correlated given more than one unique attribute, such as `OrderId` and `PaymentTransactionId` requiring both to be treated as unique and guaranteed to be so:

<!-- import OrderSagaDataRavenDB -->

We can express attributes uniqueness using the `UniqueConstraint` attribute provided by the RavenDB bundle.

At start-up the sample will send a `StartOrder` message, since we are decorating the saga data class with custom attributes we also need to plug our own logic to find a saga data instance:

<!-- import CustomSagaFinderWithUniqueConstraintRavenDB -->

Building a saga finder requires to define a class that implements the `IFindSagas<TSagaData>.Using<TMessage>` interface. The class will be automatically picked up by NServiceBus at configuration time and used each time a message of type `TMessage`, that is expected to load a saga of type `TSagaData`, is received. The `FindBy` method will be invoked by NServiceBus.

NOTE: In the sample the implementation of the `ConfigureHowToFindSaga` method, that is required, is empty because we are providing a saga finder for each message type that the saga is handling. It is not required to provide a saga finder for every message type, a mix of standard saga mappings and custom saga finding is a valid scenario.
