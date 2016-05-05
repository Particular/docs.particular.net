---
title: RavenDB - Evolving Saga Data
summary: This sample shows how use evolve RavenDB saga data over time.
component: Raven
reviewed: 2016-03-21
tags:
- Saga
related:
- nservicebus/sagas
- nservicebus/ravendb
- nservicebus/ravendb/operations-scripting
---

## Introduction

The aim of this samples to illustrate the API and techniques required gradually evolve saga(s) data structures over time.


## Prerequisites

This sample requires a RavenDB server listening on `http://localhost:8083`.


## Initial Saga

The initial Saga and Data structure


### Data

For simplicity a very simple saga data is being used in this sample.

snippet:dataV1


### Saga

The saga behaves as follows

 * Handles `StartOrder` and `IncrementOrder` messages
 * Started by `StartOrder` which either starts or resets the saga (if it already exists)
 * Handles `IncrementOrder` which increments the order count.
 * Correlates on `OrderId`

snippet:saga


## Mutating an existing Saga Data

One common scenario for evolving a saga data is to change the internal structure of a saga.

In this example the `ItemCount` property will be renamed to `NumberOfItems`.

snippet:dataV2

One way of achieving this is to leverage they [RavenDb Conversion API](http://ravendb.net/docs/search/latest/csharp?searchTerm=IDocumentConversionListener) to convert a saga data when it reads.


### Converter

snippet:Converter


#### Reading

The `AfterConversionToEntity` handles reading the saga data from RavenDB.

In this case the `ItemCount` wont exist on the Version 2 saga data so it needs to be read from the raw `RavenJObject`.

Note that `NumberOfItems` is always overwritten with `ItemCount`. The reason for this is that since Version 1 is only aware of `ItemCount` it will not set `NumberOfItems`. But Version 2 will always set `ItemCount` (see the writing implementation below) it is safe to use `ItemCount` until all Version 1 endpoints have been decommissioned.


#### Writing

The `AfterConversionToDocument` handles writing the saga data back to Raven

To have side-by-side compatible in both Version 1 and Version 2, the `NumberOfItems` property is always duplicated into `ItemCount` via the raw `RavenJObject`.


### Register Converter

The converter must be registered in the `DocumentStore` at startup.

snippet:registerConverter


WARNING: This approach is not suitable for renaming a saga data property that is being used for [mapping a message to a saga](/nservicebus/sagas/#starting-and-correlating-sagas).


## Renaming a Saga

Another common scenario is to rename a saga

So for example take the `OrderSagaData` and it needs to be pluralized to `OrdersSagaData`, which is a completely new type.

snippet:dataV3

RavenDB does not support changing the underlying document id. This means the only way of fully renaming a document is to take the following steps:

 * Read all old documents
 * Copy to the new document 
 * Save the new document
 * Delete the old document

WARNING: This is an offline action in that all old version of the endpoints have to be stopped, the migration performed, and the new version endpoints deployed. As such a [full RavenDB Backup](http://ravendb.net/docs/search/latest/csharp?searchTerm=backup%20restore) should be performed prior to a migration. Also the migration, including a simulated rollback, should be tested in a lower environment prior to preforming these operations in production.


### Rename Helper

Renaming a RavenDB saga data is done in two parts.

1. Migrate the actual saga data collection.
2. Migrate the specific items in the `SagaUniqueIdentities` collection that are used to enforce property uniqueness over multiple instances.

Both of these actions leverage the following Raven APIs:

 * [Batch](http://ravendb.net/docs/search/latest/csharp?searchTerm=Batch)
 * [Bulk Insert](http://ravendb.net/docs/search/latest/csharp?searchTerm=BulkInsert)
 * [Streaming Documents](http://ravendb.net/docs/search/latest/csharp?searchTerm=Stream)
 * [Batch Delete](http://ravendb.net/docs/search/latest/csharp?searchTerm=DeleteCommandData)
 * [Type Conventions](http://ravendb.net/docs/search/latest/csharp?searchTerm=Type%20Conventions)

snippet:renamer


### Using the Rename

This rename code can be invoked as part of a migration. In this case it is bundled in a console application.

snippet:rename

Similar migration code could also be written in PowerShell or [ScriptCS](https://github.com/scriptcs/scriptcs).


### Some helper methods

The above code used some RavenDB helper methods to help simplify the code.

snippet:RavenExtensions


### Performance

Given that this is a migration that requires endpoints to be taken offline the time taken to perform the migration should be tested in a lower environment.

Given the above saga migration, and a 2.7GHz i7 with an SSD, 30,000 saga datas can be converted  in 50 seconds.

Note that the deletion of old saga datas could be done on a running system so as to achieve a smaller migration window.

Raven DB has a variety of API that have different performance characteristics. Based on the type of saga data changes required a different combination of APIs might give better performance or simplify the migration. For example the [Patch API](http://ravendb.net/docs/article-page/3.0/Csharp/client-api/commands/patches/how-to-use-javascript-to-patch-your-documents) support manipulation of documents without needing to load the full document into memory.


## Other Saga Data changes

The approach used for renaming a saga data, and APIs described, can also be used to perform other more complicated saga operations. For example:

 * Deleting sagas that are no longer required
 * Merging multiple saga types into a single new saga
 * Splitting existent sagas into multiple other sagas
