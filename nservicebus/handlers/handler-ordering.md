---
title: Handler Ordering
summary: Controlling the order in which Handlers are executed
tags: []
redirects:
- nservicebus/how-do-i-specify-the-order-in-which-handlers-are-invoked
- nservicebus/handler-ordering
---

You can have several classes that implement `IHandleMessages<T>` for the same message. In that scenario, all the handlers will execute in the same trasaction scope. These handlers can be invoked by NServiceBus in any order. Handler ordering allows you to specify the order of execution of these handlers.

### How it is actually implemented

1. Find the list of possible handlers for a message
2. If order has been specified for any of those handlers move them at the start of the list
3. Execute the handlers

The inference here is that the remaining handlers (not specified in the order) are executed in a non-deterministic order.

### With the configuration API

snippet:HandlerOrderingWithCode

#### Specifying one to run first

snippet:HandlerOrderingWithFirst

#### Specifying multiple to run ordered

snippet:HandlerOrderingWithMultiple
