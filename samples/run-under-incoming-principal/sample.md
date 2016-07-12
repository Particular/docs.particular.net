---
title: Run Under Incoming Principal
reviewed: 2016-06-22
component: Core
related:
- samples/username-header
---

WARNING: This approach is only supported in Version 4 and below. For Version 5 and above see [Appending username using headers](/samples/username-header).

This sample show how to use the use the `RunHandlersUnderIncomingPrincipal`.

WARNING: It is important to note that, on the receiving end, this API actually uses a fake windows principle, that has the name from the header, and **not** the real authenticated principal of the user who sent the message.


### Fake Principle

For demonstration purposes, prior to sending a message, the `Thread.CurrentPrincipal` will be replaced with a new instance. Normally in production the `Thread.CurrentPrincipal` would be either the impersonated user from IIS or the current user sending a message.

snippet: SendMessage


### Configure Principal Manipulation

Principal Manipulation is configured on the receiving end as part of the bus startup.

snippet: manipulate-principal


### Consuming the Principal

When a message is handled the `Thread.CurrentPrincipal` is replaced prior to and message handlers being executed. It can then be consumed as such.

snippet: handler-using-manipulated-principal
