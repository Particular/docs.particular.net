---
title: RavenDB Custom Saga Finding Logic
summary: Perform custom saga finding logic based on custom query logic when the Saga storage is RavenDB and how to use multiple Unique attributes.
component: Raven
tags:
 - Saga
 - SagaFinder
related:
 - nservicebus/sagas
 - persistence/ravendb
reviewed: 2017-02-24
---

include: raven-resourcemanagerid-warning

include: sagafinder-into

This sample also use multiple Unique attributes using the default [RavenDB Unique Constraint bundle](https://ravendb.net/docs/search/latest/csharp?searchTerm=extending%20bundles%20unique-constraints).


## RavenDB setup

This sample requires [RavenDB persistence](/persistence/ravendb/) package and a running RavenDB instance configured accordingly.

NServiceBus out of the box does not support saga data with multiple `Unique` attributes, in order to achieve that it is possible to utilize the default RavenDB `UniqueConstraint` Bundle. Follow the [instructions on the RavenDB site](https://ravendb.net/docs/search/latest/csharp?searchTerm=extending%20bundles%20unique-constraints) to correctly install the bundle in the RavenDB server. It is also required to configure the client side of the bundle by registering the `UniqueConstraintsStoreListener` as shown above.

snippet: config

NOTE: If running this sample against an external RavenDB server ensure that the `RavenDB.Bundles.UniqueConstraints` [bundle](https://ravendb.net/docs/search/latest/csharp?searchTerm=extending%20bundles%20unique-constraints) is currently installed according to the [extending RavenDB](https://ravendb.net/docs/search/latest/csharp?searchTerm=server%20extending%20plugins) documentation. If the server side of the plugin is not correctly loaded, notice that the [`SagaNotFoundHandler`](/nservicebus/sagas/saga-not-found.md) will be invoked.

include: raven-dispose-warning


## In Process Raven Host

It is possible to self-host RavenDB so that no running instance of RavenDB server is required to run the sample.

snippet: ravenhost


include: sagafinder-thesaga


snippet: saga


include: sagafinder-process

snippet: data

A properties uniqueness can be expressed by using the `UniqueConstraint` attribute provided by the RavenDB bundle.

At start-up the sample will send a `StartOrder` message, since the saga data class is decorated with custom attributes it is required to also plug custom logic to find a saga data instance:

snippet: finder


include: sagafinder-ifindsagas

include: sagafinder-configurehowtofindsaga
