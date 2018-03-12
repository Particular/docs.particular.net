---
title: Appending username using headers
summary: Using message headers to append the current username to every message.
reviewed: 2018-03-12
component: Core
related:
- nservicebus/pipeline/message-mutators
- nservicebus/messaging/headers
---

This sample demonstrates how to append the current username to outgoing messages and then how to extract that value when messages are handled.


### Fake principal

For demonstration purposes, prior to sending a message, the `Thread.CurrentPrincipal` will be replaced with a new instance. In a production scenario, the `Thread.CurrentPrincipal` would be either the impersonated user from IIS or the current user sending a message.

snippet: SendMessage


## Custom header with a mutator

The recommended approach for capturing the current user is to create a transport mutator that extracts the current identity and then adds it to the header of every outgoing message.


### Outgoing mutator

The outgoing mutator extracts `Thread.CurrentPrincipal.Identity.Name` and appends it to a message header.

snippet: Mutator


#### Register the Mutator

snippet: componentRegistration


### The Handler

From within a handler (or saga) this value can be used as follows:

snippet: handler-using-custom-header
