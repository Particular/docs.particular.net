---
title: SQL Persistence Upgrade Version 2 to 3
summary: Instructions on how to upgrade to SQL Persistence version 3
reviewed: 2017-08-06
component: SqlPersistence
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 6
 - 7
---


## Accessing business data

When [accessing business data](/persistence/sql/accessing-data.md) in Versions 2 and below the SQL persistence session information was injected into the storage session state irrespective of the selected [storage type](/persistence/#storage-types). In Versions 3 and above the SQL persistence session information will only be injected if the SQL persistence is used for either [Sagas](/nservicebus/sagas/) or the [Outbox](/nservicebus/outbox/). If the SQL persistence is for one of those and another persistence is used for the other than an exception will be thrown. So for example it is not possible to mix the SQL persistence for Sagas with there any other persistence for Outbox. The same applies for the inverse.
