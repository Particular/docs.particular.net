---
title: Configuration order for persistence
summary: When configuring persistence, order is very important
tags:
- Persistence
redirects:
- nservicebus/persistence-order
---

When configuring persistence in NServiceBus version 5, order is very important.
The last configured persistence option wins.
Lets have a look at some examples.

### Example 1

In this example the last configuration option will override all previous options.

snippet:PersistenceOrder_Incorrect

### Example 2

In this example all configuration options are explicit.

snippet:PersistenceOrder_Explicit

### Example 3

This example sets the default persistence first thing and then overrides more explicit options.

snippet:PersistenceOrder_Correct
