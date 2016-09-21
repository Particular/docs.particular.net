---
title: Testing NServiceBus
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2016-03-31
components: Testing
redirects:
 - nservicebus/unit-testing
 - nservicebus/testing/unit-testing
related:
 - samples/unit-testing
---


Testing enterprise-scale distributed systems is a challenge. NServiceBus provides a dedicated NuGet package, `NServiceBus.Testing`, with tools that allow unit testing endpoint handlers, sagas and workflows.

The testing package can be used with any .NET unit testing framework, such as NUnit, xUnit.net or MSBuild.


## Getting started

To install the package with unit testing tools, run the following command in the Package Manager Console:

```ps
Install-Package NServiceBus.Testing
```


## Test structure


partial:teststructure


## Testing message handlers

The service layer in an NServiceBus application consists from message handlers. Each class typically handles one specific message type. 

Testing handlers usually focuses on their externally visible behavior: the types of messages they send or reply with:

snippet: TestingServiceLayer

This test verifies that when a message of the type `RequestMessage` is processed by `MyHandler`, it responds with a message of the type `ResponseMessage`. Also, the test checks that if the request message's String property value is "hello" then that should be the value of the String property of the response message.


## Testing sagas

snippet:TestingSaga


### Testing interface messages

To support testing of interface messages Version 5 introduces a `.WhenHandling<T>()` method where T is the interface type.


## Testing header manipulation

It is the responsibility of the message handlers in the service layer to use data from headers found in the request to make decisions, and to set headers for the response messages. This is how such functionality can be tested:

snippet:TestingHeaderManipulation

This test asserts that the value of the outgoing header has been set.


## Injecting additional dependencies into the service layer

Many of the message handling classes in the service layer make use of other objects to perform their work. When testing these classes, replace those objects with "stubs" so that the class under test is isolated. Here's how:

snippet:TestingAdditionalDependencies


## Constructor injected bus

NOTE: In Versions 6 and higher, the `IBus` interface was deprecated and removed, and replaced with the contextual `IMessageHandlerContext` parameter on the `IHandleMessages<T>.Handle()` methods.

snippet: ConstructorInjectedBus


## Limiting assemblies scanned by the test framework

To limit the assemblies and types scanned by the test framework it is possible to use the `Initialize()` overload that accepts a delegate to customize the `ConfigurationBuilder`. The list of assemblies scanned must include `NServiceBus.Testing.dll`

snippet: TestInitializeAssemblies 
