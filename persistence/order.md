---
title: Configuration order for persistence
summary: When configuring persistence, order is very important
component: Core
reviewed: 2016-08-24
versions: '[5.0,)'
tags:
- Persistence
redirects:
- nservicebus/persistence-order
- nservicebus/persistence/order
---

When configuring persistence order is very important. The last configured persistence option wins.


### Example 1

In this example the last configuration option will override all previous options.

snippet: PersistenceOrder_Incorrect


### Example 2

In this example all configuration options are explicit.

snippet: PersistenceOrder_Explicit


### Example 3

This example sets the default persistence first thing and then overrides more explicit options.

snippet: PersistenceOrder_Correct
