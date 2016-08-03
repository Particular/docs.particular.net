---
title: Using Outbox with RavenDB persistence
component: Raven
tags:
 - RavenDB
 - Outbox
related:
- nservicebus/outbox
---

The [Outbox](/nservicebus/outbox) feature requires persistence in order to store the messages and enable deduplication.


## What extra documents does RavenDB outbox persistence create

To keep track of duplicate messages, the RavenDB implementation of Outbox creates a special collection of documents called `OutboxRecord`.


## How long are the deduplication records kept

The RavenDB implementation by default keeps deduplication records for 7 days and runs the purge every 1 minute.

These default settings can be changed by specifying new defaults in the settings dictionary:

snippet:OutboxRavendBTimeToKeep
