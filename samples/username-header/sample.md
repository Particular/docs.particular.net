---
title: Passing user identity between endpoints using a custom header
summary: How to pass user identity between sending and receiving endpoints by attaching a custom header to every outgoing message.
reviewed: 2024-12-10
component: Core
related:
- nservicebus/pipeline/message-mutators
- nservicebus/messaging/headers
---

This sample demonstrates how to attach the current user identity (username) to all outgoing messages and how to extract that value when messages are received. User identity is accessed by a current principal accessor, registered through dependency injection.

> [!NOTE]
> This sample doesn't use `Thread.CurrentPrincipal`. When used in asynchronous code, `Thread.CurrentPrincipal` depends on the version of the .NET runtime. Refer to [the Microsoft guidelines](https://docs.microsoft.com/en-us/aspnet/core/migration/claimsprincipal-current) for more details.

### Fake principal

The sample replaces the `principalAccessor.CurrentPrincipal` before sending a message with an ad-hoc value. In a production scenario, the `principalAccessor.CurrentPrincipal` would likely be set by the hosting environment e.g. IIS, and not in the user code.

snippet: send-message

The snippet above uses two asynchronous sends to demonstrate that the current principle is properly propagated into the message session.

## Custom header with a mutator

The recommended approach for capturing user information is to create a transport mutator extracting the current identity that adds it to the header collection of every outgoing message.

### Outgoing message mutator

The outgoing mutator extracts `principalAccessor.CurrentPrincipal.Identity.Name` and adds it to the message headers collection.

snippet: username-header-mutator

#### Register the outgoing message mutator

snippet: component-registration-sender

### Incoming message mutator

The incoming mutator extracts the username header from the message and sets the `principalAccessor.CurrentPrincipal`.

snippet: set-principal-from-header-mutator

#### Register the incoming message mutator

snippet: component-registration-receiver

This sample doesn't register the outgoing message mutator for the receiver. If desired, the outgoing message mutator could be registered on the receiver as well.

### The Handler

From within a handler (or saga), the header value holding user identity can be accessed in the following way:

snippet: handler-using-custom-header
