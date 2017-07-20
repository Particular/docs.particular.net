---
title: Handler Ordering
summary: Using interfaces to control the order in which Handlers are executed
reviewed: 2017-07-20
component: HandlerOrdering
---

This extension allows a more expressive way to [Order Handlers](/nservicebus/handlers/handler-ordering.md).

HandlerOrdering allows the dependency between handlers to be expressed via interfaces and the resulting order is derived at runtime.


## Configuring to use HandlerOrdering

snippet: Usage


## Expressing dependencies


#### MessageHandler1 wants to run after MessageHandler3

snippet: express-order1


#### MessageHandler2 wants to run after MessageHandler1

snippet: express-order2


### Resulting execution order

 1. MessageHandler3
 1. MessageHandler1
 1. MessageHandler2
