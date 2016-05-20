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

NServiceBus.Testing Version 6 requires .NET Framework version 4.5.2 or higher.

**All projects which reference NServiceBus.Testing must first be upgraded to .NET 4.5.2 before upgrading to NServiceBus.Testing Version 6**


## New Unit Testing capabilities

NserviceBus.Testing Version 6 provides an alternative way to write tests using a more traditional *Arrange Act Assert* pattern. This allows for easier extension and tooling integration. For more details on the new testing capabilities, see the [Unit Testing NServiceBus 6 Sample](/samples/unit-testing).


## Testing Framework


### Removed Test.Initialize()

It is no longer required to call `Test.Initialize()` before executing a test. All calls to `Test.Initialize()` can be safely removed.


#### Unobtrusive Message Conventions

With the removal of `Test.Initialize()` it is also no longer required to configure unobtrusive message conventions.


## Testing MessageHandlers


### ExpectReturn

Use `ExpectReply` instead of `ExpectReturn`.


### SendToSites Expecations

`ExpectSendToSites` and `ExpectNotSendToSites` have been removed from the Testing Framework. Handlers using the Gateway can still be tested using the `ExpectSend` overload which provides the `SendOption`:

snippet:ExpectSendToSiteV6


## Testing Sagas


### When

In NServiceBus Versions 6 and above, message handlers have an additional `IMessageHandlerContext` parameter.  This context parameter needs to be provided when defining the method to invoke.

`.When(s => s.Handle(new StartsSaga()))`

becomes

`.When((s, context) => s.Handle(new StartsSaga(), context))`

WARNING: It's important to pass the context provided by the delegate arguments to the handle method.

A new overload allows for a more convenient invocation:

`.When(s => s.Handle, new StartsSaga())`


### Configuring a message ID

The message ID can be manually configured using the `ConfigureMessageHandler` option. See [Configuring the context](#configuring-the-context) section below for more details.


## Configuring the context

Both Saga and Handler tests contain a `ConfigureHandlerContext` method to enable custom configuration of the `IMessageHandlerContext` which is passed to the invoked handler methods.

snippet:ConfigureSagaMessageId

