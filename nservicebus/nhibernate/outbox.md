---
title: Outbox with NHibernate persistence
versions: '[6.0,)'
component: NHibernate
reviewed: 2016-10-26
tags:
 - NHibernate
 - Outbox
related:
- nservicebus/outbox
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## Table

To keep track of duplicate messages, the NHibernate implementation of Outbox requires the creation of `OutboxRecord` table.

partial:customizing

## Deduplication record lifespan

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://msdn.microsoft.com/en-us/library/ee372286.aspx):

snippet:OutboxNHibernateTimeToKeep
