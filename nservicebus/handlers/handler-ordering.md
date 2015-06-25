---
title: Handler Ordering
summary: Handler Ordering
tags: []
redirects:
- nservicebus/how-do-i-specify-the-order-in-which-handlers-are-invoked
- nservicebus/handler-ordering
---

Handler ordering allows you which handlers are grouped first and the order withing that group. 

### How it is actually implemented

1. Find the list of possible handlers for a message
2. If order has been specified for any of those handlers move them at the start of the list
3. Execute the handlers

The inference here is that the remaining handlers (not specified in the order) are executed in a non-deterministic order.   

### With the configuration API

<!-- import HandlerOrderingWithCode -->

### Specifying First with ISpecifyMessageHandlerOrdering

<!-- import HandlerOrderingWithFirst -->

### Specifying multiple to run ordered with ISpecifyMessageHandlerOrdering

<!-- import HandlerOrderingWithMultiple -->
