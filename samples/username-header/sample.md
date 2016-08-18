---
title: Appending username using headers
summary: Using message headers to append the current username to every message.
reviewed: 2016-06-22
component: Core
related:
- nservicebus/pipeline/message-mutators
- nservicebus/messaging/headers
- samples/run-under-incoming-principal
---

This sample shows appending the current username to outgoing messages and then extracting that value during message handling.


### Fake Principal

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
