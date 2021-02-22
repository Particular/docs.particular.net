---
title: Client-side Callbacks Testing
summary: Shows how to unit test code that uses Callbacks.
reviewed: 2021-02-22
component: CallbacksTesting
related:
- samples/callbacks
- nservicebus/messaging/callbacks
---

Shows the usage of the `NServiceBus.Callbacks.Testing` package.


## Prerequisites for callback testing functionality

The approach shown here works with the `NServiceBus.Callbacks` NuGet package version 1.1.0 or above. Install the `NServiceBus.Callbacks.Testing` NuGet package.


### Int

The integer response scenario allows any integer value to be returned in a strongly typed manner.


#### Testing

The response type returned by the `When` definition needs to be of type `int`.

snippet: IntCallbackTesting


### Enum

The enum response scenario allows any enum value to be returned in a strongly typed manner.


#### Testing

The response type returned by the `When` definition needs to be of the enum type expected.

snippet: EnumCallbackTesting


### Object

The object response scenario allows an object instance to be returned.


#### Testing

The response type returned by the `When` definition needs to be of the object response type expected.

snippet: ObjectCallbackTesting


#### Testing with SendOptions

The `When` definition provides a matcher overload which allows matching against the response and the send options passed into the callback function.

snippet: ObjectCallbackTestingWithOptions


## Cancellation

The asynchronous callback can be canceled by registering a `CancellationToken` provided by a `CancellationTokenSource`.


#### Testing

snippet: CancelCallbackTesting
