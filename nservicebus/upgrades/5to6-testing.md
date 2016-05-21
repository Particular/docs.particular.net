---
title: Upgrade NServiceBus.Testing Version 5 to 6
summary: Instructions on how to upgrade NServiceBus.Testing Version 5 to 6.
tags:
 - upgrade
 - migration
related:
- nservicebus/testing
---


## Upgrade to NServiceBus Version 6

NServiceBus.Testing references NServiceBus Version 6.

All projects which reference NServiceBus.Testing must first be upgraded to NServiceBus Version 6. For the detailed list of changes, refer to the [NServiceBus Version 6 Upgrade Guide](/nservicebus/upgrades/5to6.md). 


## New Unit Testing capabilities

NserviceBus.Testing Version 6 provides an alternative way to write tests using a more traditional *Arrange Act Assert* pattern. This allows for easier extension and tooling integration. For more details on the new testing capabilities, see the [Unit Testing NServiceBus 6 Sample](/samples/unit-testing).


## Testing Framework


### Removed Test.Initialize()

It is no longer required to call `Test.Initialize()` before executing a test. All calls to `Test.Initialize()` can be safely removed.


#### Unobtrusive Message Conventions

With the removal of `Test.Initialize()` it is also no longer required to configure unobtrusive message conventions.


## Testing Message Handlers


### ExpectReturn Method

Use `ExpectReply` instead of `ExpectReturn`.


### SendToSites Methods

`ExpectSendToSites` and `ExpectNotSendToSites` methods have been removed from the Testing Framework. Handlers using the Gateway can still be tested using the `ExpectSend` overload which provides the `SendOption`:

snippet:ExpectSendToSiteV6


## Testing Sagas


### Using When

In NServiceBus Versions 6 and above, message handlers have an additional `IMessageHandlerContext` parameter.  This context parameter needs to be provided when defining the method to invoke. 

snippet: 5to6-usingWhen

A new overload has been added to simplify this:

snippet: 5to6-usingNewOverload

WARNING: It's important to pass the context provided by the delegate arguments to the handle method.

### Configuring a message ID

The message ID can be manually configured using the `ConfigureMessageHandler` option. See [Configuring the context](#configuring-the-context) section below for more details.


## Configuring the context

Both Saga and Handler tests contain a `ConfigureHandlerContext` method to enable custom configuration of the `IMessageHandlerContext` which is passed to the invoked handler methods.

snippet:ConfigureSagaMessageId

