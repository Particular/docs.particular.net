---
title: Handler Ordering
summary: Controlling the order in which Handlers are executed
reviewed: 2016-09-21
component: Core
redirects:
- nservicebus/how-do-i-specify-the-order-in-which-handlers-are-invoked
- nservicebus/handler-ordering
---

Multiple classes may implement `IHandleMessages<T>` for the same message. In that scenario, all the handlers will execute in the same transaction scope. These handlers can be invoked in any order. Handler Ordering allows the the order of execution of Handlers to be specified in code.


### How it is actually implemented

 1. Find the list of possible handlers for a message.
 1. If order has been specified for any of those handlers move them at the start of the list.
 1. Execute the handlers.

The inference here is that the remaining handlers (not specified in the order) are executed in a non-deterministic order.


### With the configuration API

snippet: HandlerOrderingWithCode


#### Specifying one to run first

snippet: HandlerOrderingWithFirst


#### Specifying multiple to run ordered

snippet: HandlerOrderingWithMultiple