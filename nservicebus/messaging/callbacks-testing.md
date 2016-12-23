---
title: Client side Callbacks Testing
summary: Shows how to unit test code that uses Callbacks.
reviewed: 
component: CallbacksTesting
related:
- samples/callbacks
- nservicebus/messaging/callbacks
---

Shows the usage of the `NServiceBus.Callback.Testing`.

## Prerequisites for callback testing functionality

The approach shown here only works with the `NServiceBus.Callbacks` NuGet package version 1.1 or greater. Install the `NServiceBus.Callbacks.Testing` NuGet package.


### Int

The integer response scenario allows any integer value to be returned in a strong typed manner.

#### Testing

snippet:IntCallbackTesting

### Enum

The enum response scenario allows any enum value to be returned in a strong typed manner.

#### Testing

snippet:EnumCallbackTesting

### Object

The Object response scenario allows an object instance to be returned.


#### Testing

This feature leverages the message Reply mechanism of the bus and hence the response need to be a message.

snippet:ObjectCallbackTesting

## Cancellation

The asynchronous callback can be canceled by registering a `CancellationToken` provided by a `CancellationTokenSource`.

#### Testing

snippet:CancelCallbackTesting