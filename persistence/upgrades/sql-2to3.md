---
title: SQL Persistence Upgrade Version 2 to 3
summary: Instructions on how to upgrade to SQL Persistence version 3
reviewed: 2021-04-30
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
---


## Accessing business data

When [accessing business data](/persistence/sql/accessing-data.md) in Versions 2 and below, the SQL persistence session information was injected into the storage session state irrespective of the selected [storage type](/persistence/#features-that-require-persistence). In Versions 3 and above, the SQL persistence session information will only be injected if the SQL persistence is used for either [Sagas](/nservicebus/sagas/) or the [Outbox](/nservicebus/outbox/). If the SQL persistence is for one of those and another persistence is used for the other, then an exception will be thrown. For example, it is not possible to mix SQL persistence for Sagas with any other persistence for Outbox. The same applies for the inverse.


## Variant renamed to Dialect

To more accurately reflect SQL vernacular "Variant" has been renamed to "Dialect":

snippet: 2to3_VariantToDialect

The `SqlDialect` method returns a typed settings instance that can be used to apply configuration specific to that dialect:

snippet: 2to3_DialectSettings


## Dialect selection made mandatory

Versions 2 and below defaulted to SQL Server when a call to `SqlVariant` was omitted. In Versions 3 and above a call to `SqlDialect` is mandatory.


## Schema moved to SqlDialect

Schema configuration has been moved to extend the result of the `SqlDialect` method:

snippet: 2to3_Schema
