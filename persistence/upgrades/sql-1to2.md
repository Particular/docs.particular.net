---
title: SQL Persistence Upgrade Version 1 to 2
summary: Instructions on how to upgrade to SQL Persistence version 2
reviewed: 2017-03-09
component: SqlPersistence
isUpgradeGuide: true
redirects:
 - nservicebus/upgrades/sqlpersistence-1to2
 - persistence/upgrades/sqlpersistence-1to2
upgradeGuideCoreVersions:
 - 6
---


## Subscription Caching configuration now required

[Subscription Caching](/persistence/sql/subscriptions.md) is now a required configuration option. 

Ether configure a period of time cache for:

snippet: 1to2_subscriptions_CacheFor

Or explicitly disable subscription caching.

snippet: 1to2_subscriptions_Disable


## Inheriting from SqlSaga now required

In Version 1 inheriting from `NServiceBus.Saga<T>` was partially supported. However this having two competing approaches that deliver the same features caused significant confusion. In Version 2 `NServiceBus.Saga<T>` is no longer supported and either a build error, or an runtime exception for some edge cases, will occur.


## Deep nested Saga hierarchies are no longer supported

In Version 1 having deep class hierarchies from under `SqlSaga<T>` was supported. This scenario is no longer supported and all sagas **must** inherit directly from `SqlSaga<T>`. This change was done to bring the SQL persistence approach inline with the future approach being taken by NServiceBus.


## Correlation Property

The API for definition a Correlation Property has been moved from an attribute to a property at the saga class level.

snippet: 1to2_Correlation


## Message Mapping

The API for definition message mapping has been changed to bring it inline with the future approach being taken by NServiceBus:

 * `MapMessage` renamed to `ConfigureMapping`.
 * `MessagePropertyMapper<T>` renamed to `IMessagePropertyMapper`.

snippet: 1to2_Mapping


## SqlSaga.ConfigureMapping made abstract

To simplify implementing a saga using `SqlSaga<T>` the method `SqlSaga<T>.ConfigureMapping` has been made abstract and now always needs to be implemented even if no message mapping is required.


## SqlPersistenceSettingsAttribute move to use properties

Attribute have been moved to use properties instead of optional parameters in the constructor.

snippet: 1to2_SqlPersistenceSettings


## SqlSagaAttribute made obsolete

The `[SqlSagaAttribute]` has been made obsolete and replaced by property overrides on the `SqlSaga<T>` class.

snippet: 1to2_SagaAttribute


## Explicit schema API

An explicit schema API has been added.

snippet: 1to2_Schema

If characters required quoting were previously used in the table prefix, they can be removed and the following used:

snippet: 1to2_Schema_Extended

WARNING: An exception will be thrown if any of ], [ or &grave; are detected in the `tablePrefix` or the schema.


## Missing Indexes

Some missing indexes have been added. These indexes will be added the next time the [installers](/persistence/sql/#installation) are executed. No explicit SQL migration is required.


### TimeoutData

 * Add missing non-unique index on `Time`, for query to find expired timeouts.
 * Add missing non-unique index on `SagaId`, used for clearing timeouts from completed sagas.


### OutboxData

* Add missing index on `Dispatched (bool)`
* Add missing index on `DispatchedAt (datetime)`