---
title: Using Outbox with NHibernate persistence
summary: Using Outbox with NHibernate persistence
component: NHibernate
tags:
 - NHibernate
 - Outbox
related:
- nservicebus/outbox
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## What extra tables does NHibernate Outbox persistence create

To keep track of duplicate messages, the NHibernate implementation of Outbox requires the creation of `OutboxRecord` table.


## How long are the deduplication records kept

The NHibernate implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the config file using [TimeStamp strings](https://msdn.microsoft.com/en-us/library/ee372286.aspx):

snippet:OutboxNHibernateTimeToKeep
