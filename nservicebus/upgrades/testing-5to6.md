---
title: Upgrade NServiceBus.Testing Version 5 to 6
summary: Instructions on how to upgrade NServiceBus.Testing Version 5 to 6.
tags:
 - upgrade
 - migration
related:
- nservicebus/testing
---


## Move to .NET 4.5.2

The minimum .NET version for NServiceBus.Testing Version 6 is .NET 4.5.2.

**Users must update all projects (that reference NServiceBus.Testing) to .NET 4.5.2 before updating to NServiceBus.Testing Version 6.**

It is recommended to update to .NET 4.5.2 and perform a full migration to production **before** updating to NServiceBus.Testing Version 6.


## New Unit Testing capabilities

Version 6 and above of NserviceBus.Testing provide an alternative way to write tests using a more traditional *Arrange Act Assert* pattern. This allows for easier extension and tooling integration compared to the NServiceBus Testing Framework. See [Unit Testing NServiceBus 6](/samples/unit-testing) documentation to find more about the new testing capabilities.


## Testing Framework


### Removed Test.Initialize()

It is no longer required to call `Test.Initialize()` before executing a test. All calls to `Test.Initialize()` can be safely removed.


#### Unobtrusive Message Conventions

With the removal of `Test.Initialize()` it is also no longer required to configure unobtrusive message conventions.


## Testing MessageHandlers


### ExpectReturn

Use `ExpectReply` instead of `ExpectReturn`.


### SendToSite Expecations

`ExpectSendToSites` and `ExpectNotSendToSites` have been removed from the Testing Framework. Handlers using the Gateway can still be tested using the `ExpectSend` overload which provides the `SendOption`:

snippet:ExpectSendToSiteV6


## Testing Sagas


### When

Since Version 6 and above receives a `IMessageHandlerContext` as an additional parameter, this context needs to be provided when defining the method to invoke.

`.When(s => s.Handle(new StartsSaga()))`

becomes

`.When((s, context) => s.Handle(new StartsSaga(), context))`

WARN: It's important to pass the context provided by the delegate arguments to the handle method.

A new overload allows for a more convenient invocation:

`.When(s => s.Handle, new StartsSaga())`


### Configuring a message ID

The message ID can be manually configured using the `ConfigureMessageHandler` option. See [Configuring the context](#configuring-the-context) section below for more details.


## Configuring the context

Both Saga and Handler tests contain a `ConfigureHandlerContext` method enables custom configuration of the `IMessageHandlerContext` which is passed to the invoked handler methods.

snippet:ConfigureSagaMessageId

