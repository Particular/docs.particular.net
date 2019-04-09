---
title: Testing NServiceBus with fluent style
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2017-07-17
component: Testing
redirects:
 - nservicebus/unit-testing
 - nservicebus/testing/unit-testing
related:
 - samples/unit-testing
---

NOTE: Generally it is recommended to use Arrange-Act-Assert style tests. To learn how to test NServiceBus using Arrange-Act-Assert (AAA) approach, refer to the [dedicated article](/samples/unit-testing/).

## Structure


partial: teststructure


## Handlers

Testing handlers focuses on their externally visible behavior - the types of messages they send or reply with.

snippet: TestingServiceLayer

The test verifies that when a message of the type `RequestMessage` is processed by `MyHandler`, it responds with a message of the type `ResponseMessage`. Also, the test checks that if the request message's String property value is "hello" then that should be the value of the String property of the response message.


## Sagas

Testing sagas focuses on their externally visible behavior - the types of messages they send or reply with, but it's also possible to verify that saga requested a timeout or was completed.

snippet: TestingSaga

The test verifies that when a message of the type `StartsSaga` is processed by `MySaga`, the saga replies to the sender with the `MyResponse` message, publishes `MyEvent`, sends `MyCommand` and requests a timeout for message `StartsSaga`. Also it checks if the saga publishes `MyOtherEvent` and is completed, after the timeout expires.

Note that the expectation for `MyOtherEvent` is set only after the message is sent.



### Interface messages

To support testing of interface messages use `.WhenHandling<T>()` method, where `T` is the interface type.


## Header manipulation

Message handlers retrieve information from the incoming message headers and set headers for the outgoing messages, for example NServiceBus uses that set correlation Id or address for reply. Headers can be also used for passing [custom information](/nservicebus/messaging/header-manipulation.md).

snippet: TestingHeaderManipulation

This test asserts that the value of the outgoing header has been set.


## Injecting additional dependencies into the service layer

Many of the message handling classes in the service layer make use of other objects to perform their work. When testing these classes, replace those objects with "stubs" so that the class under test is isolated.

snippet: TestingAdditionalDependencies


partial: nsbinjection


partial: init
