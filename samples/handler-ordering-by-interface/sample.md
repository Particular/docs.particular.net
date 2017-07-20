---
title: Handler ordering by interface
summary: Using interfaces to express dependency between handlers.
reviewed: 2016-10-15
component: HandlerOrdering
---


## Configuring to use HandlerOrdering

snippet: config


## Expressing dependencies


#### MessageHandler1 wants to run after MessageHandler3

snippet: express-order1


#### MessageHandler2 wants to run after MessageHandler1

snippet: express-order2


### Resulting execution order

 1. MessageHandler3
 1. MessageHandler1
 1. MessageHandler2
