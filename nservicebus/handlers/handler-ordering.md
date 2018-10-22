---
title: Handler Ordering
summary: Controlling the order in which handlers are executed
reviewed: 2018-05-23
component: Core
redirects:
- nservicebus/how-do-i-specify-the-order-in-which-handlers-are-invoked
- nservicebus/handler-ordering
---

In the past, message handlers used to be the only way to implement cross-cutting concerns like authentication, authorization, and certain kinds of validation. As those concerns needed to be applied before any other logic, it was necessary to run those handlers before all other message handlers. These message handler classes implemented `IHandleMessage<IMessage>` or `IHandleMessage<Object>` so that they would handle all incoming messages.

Warning: It is now recommended to plug into the [Message handling pipeline](/nservicebus/pipeline/) to implement these cross cutting concerns.

If it is not possible to migrate this kind of functionality out of message handlers, there are a number of ways to specify the order in which they will be executed. 

Note: All message handlers in the endpoint will execute in the same transaction scope. 

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
