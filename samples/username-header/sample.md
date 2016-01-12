---
title: Appending username using headers
summary: Shows how to use message headers to append the current username to every message.
related:
- nservicebus/pipeline/message-mutators
- nservicebus/messaging/headers
---

This sample show two approaches appending the current username to outgoing messages and then extracting that value during message handling.


### Fake Principle

For demonstration purposes, prior to sending a message, the `Thread.CurrentPrincipal` will be replaced with a new instance. Normally in production the `Thread.CurrentPrincipal` would be either the impersonated user from IIS or the current user sending a message. 

snippet: SendMessage


## Custom Header with a Mutator

The recommended approach to capturing the current user is to create a transport mutator that extracts the current identity and adds that to the header of every outgoing message.


### Outgoing Mutator

The outgoing mutator extracts `Thread.CurrentPrincipal.Identity.Name` and appends it to a message header.

snippet:Mutator


#### Register the Mutator

snippet: componentregistartion


### The Handler

From within a Handler (or Saga) this value can be used as such.

snippet: handler-using-custom-header


## Manipulating the incoming Thread.CurrentPrincipal

WARNING: This approach is only supported in Version 4 and lower. For Version 5 and higher use the above approach.

This approach uses the "Principal Manipulation" API that exists in both Versions 3 and 4. 

WARNING: It is important to note that, on the receiving end, this API actually uses a fake windows principle, that has the name from the header, and **not** the real authenticated principal of the user who sent the message.


### Configure Principal Manipulation

Principal Manipulation is configured on the receiving end as part of the bus startup.

snippet: manipulate-principal


### Consuming the Principal

When a message is handled the `Thread.CurrentPrincipal` is replaced prior to and message handlers being executed. It can then be consumed as such.

snippet: handler-using-manipulated-principal
