---
title: SQL Persistence Upgrade Version 1 to 2
summary: Instructions on how to upgrade to SQL Persistence version 2
reviewed: 2017-03-09
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Inheriting from SqlSaga now required

In Version 1 inhering from `NServiceBus.Saga<T>` was partially supported. However this having two competing approaches that deliver the same features caused significant confusion. In Version 2 `NServiceBus.Saga<T>` is no longer supported and either a build error, or an runtime exception for some edge cases, will occur.


## Explicit schema API

An explicit schema API has been added.

snippet: 1to2_Schema

If characters required quoting were previously used in the table prefix, they can be removed and the following used:

snippet: 1to2_Schema_Extended

WARNING: An exception will be thrown if any of ], [ or &grave; are detected in the tablePrefix or the schema.


## SqlSaga.ConfigureMapping made abstract

To simplify implementing a saga using `SqlSaga<T>` the method `SqlSaga<T>.ConfigureMapping` has been made abstract and now always needs to be implemented even if no message mapping is required.