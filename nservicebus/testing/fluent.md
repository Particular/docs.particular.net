---
title: Testing NServiceBus with fluent style
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2021-04-07
component: Testing
redirects:
 - nservicebus/unit-testing
 - nservicebus/testing/unit-testing
related:
 - samples/unit-testing
---

NOTE: Generally it is recommended to use the Arrange-Act-Assert (AAA) style rather than fluent style. To learn how to test NServiceBus using Arrange-Act-Assert, refer to the [sample](/samples/unit-testing/).

WARN: If Xunit test parallization feature is used it is possible that the synchronous variants of `OnMessage` or `WhenXYZ` methods will deadlock under certain conditions. The synchronous methods are available for backward compatibility.

## Structure


partial: teststructure


## Handlers

Handlers should be tested with a focus on their externally visible behavior: the messages they handle, and those they send, publish, or reply with.

snippet: TestingServiceLayer

This test verifies that, when a message of type `RequestMessage` is processed by `MyHandler`, it responds with a message of type `ResponseMessage`. The test also checks that, if the incoming message has a `String` property of `"hello"`, then the outgoing message also has a `String` property of `"hello"`.

partial: asynclayer

## Sagas

Sagas should also be tested with a focus on their externally visible behavior: the types of messages they handle, either immediately or after a timeout has expired, and those they send, publish, or reply with.

snippet: TestingSaga

This test verifies that, when a message of type `StartsSaga` is processed by `MySaga`, the saga replies to the sender with a message of type `MyResponse`, publishes a message of type `MyEvent`, sends a message of type `MyCommand`, and requests a timeout for message of type `StartsSaga`. The test also checks if the saga publishes a message of type `MyOtherEvent`, and that the saga is completed after the timeout expires.

Note that the expectation for the message of type `MyOtherEvent` is set only after the message is sent.

partial: asyncsagas

## Interface messages

To support testing of interface messages, use the `.WhenHandling<T>()` or `WhenHandlingAsync<T>()` (7.2 and above) method, where `T` is the interface type.


## Header manipulation

Message headers are used to communicate metadata about messages. For example, NServiceBus uses headers to store correlation ID's which are used to associate messages with each other. Headers can be also used for communicate [custom information](/nservicebus/messaging/header-manipulation.md).

snippet: TestingHeaderManipulation

This test asserts that the outgoing message contains header named `"MyHeaderKey"` set to `"myHeaderValue"`.

partial: asyncheader

## Injecting additional dependencies

Some handlers require other objects to perform their work. When testing those handlers, replace the objects with "stubs" so that the class under test is isolated.

snippet: TestingAdditionalDependencies


partial: nsbinjection


partial: init
