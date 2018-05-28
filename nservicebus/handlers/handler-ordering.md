---
title: Handler Ordering
summary: Controlling the order in which handlers are executed
reviewed: 2018-05-23
component: Core
redirects:
- nservicebus/how-do-i-specify-the-order-in-which-handlers-are-invoked
- nservicebus/handler-ordering
---

Multiple classes may implement `IHandleMessages<T>` for the same message. In this scenario, all handlers will execute in the same transaction scope. These handlers can be invoked in any order but the order of execution can be specified in code


### Overview of the implementation

 1. Find the list of possible handlers for a message.
 1. If an order has been specified for any of those handlers, move them to the start of the list.
 1. Execute the handlers.

The remaining handlers (i.e. ones not specified in the ordering) are executed in a non-deterministic order.


### With the configuration API

snippet: HandlerOrderingWithCode


#### Specifying one handler to run first

snippet: HandlerOrderingWithFirst


#### Specifying multiple handlers to run in order

snippet: HandlerOrderingWithMultiple