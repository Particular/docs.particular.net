---
title: Unit Testing
summary: Develop your service layers and long-running processes using test-driven development.
tags: []
redirects:
 - nservicebus/unit-testing
 - nservicebus/testing/unit-testing
---

Developing enterprise-scale distributed systems is hard and testing them is just as challenging a task. The architectural approach supported by NServiceBus makes these challenges more manageable. And the testing facilities provided actually make unit testing endpoints and workflows easy. You can now develop your service layers and long-running processes using test-driven development.


## Getting started

NServiceBus ships with a stand alone testing helper NuGet package that makes testing a lot simpler.  
To install this package:
```
Install-Package NServiceBus.Testing
```
Once the package is installed you need ensure that you call `Test.Initialize()` (or any of its overloads) before executing any test method.

NOTE: To limit the assemblies and types scanned by the test framework it is possible to use the `Initialize()` overload that accepts a delegate you can use to customize the `ConfigurationBuilder`.


## Unit testing the service layer

The service layer in an NServiceBus application is made from message handlers. Each class typically handles one specific type of message. Testing these classes usually focuses on their externally visible behavior: the types of messages they send or reply with. This is as simple to test as could be expected:

<!-- import TestingServiceLayer --> 

This test says that when a message of the type `RequestMessage` is processed by `MyHandler`, it responds with a message of the type `ResponseMessage`. Also, the test checks that if the request message's String property value is "hello" then that should be the value of the String property of the response message.


## Testing a Saga

snippet:TestingSaga


## Configuring unobtrusive message conventions

If you are using [unobtrusive mode](/nservicebus/messaging/unobtrusive-mode.md) you need to configure the unit test support with those conventions as shown below.

snippet:SetupConventionsForUnitTests


### Testing interface messages

To support testing of interface messages version 5 introduces a `.WhenHandling<T>()` method where T is the interface type.


## Testing header manipulation

It is the responsibility of the message handlers in the service layer to use data from headers found in the request to make decisions, and to set headers for the response messages. This is how such functionality can be tested:

snippet:TestingHeaderManipulation

This test asserts that the value of the outgoing header has been set.


## Injecting additional dependencies into the service layer

Many of the message handling classes in the service layer make use of other objects to perform their work. When testing these classes, replace those objects with "stubs" so that the class under test is isolated. Here's how:

snippet:TestingAdditionalDependencies
