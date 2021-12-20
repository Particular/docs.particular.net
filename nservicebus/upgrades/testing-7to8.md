---
title: NServiceBus Testing Upgrade Version 7 to 8
summary: Instructions on how to upgrade NServiceBus.Testing Version 7 to 8.
reviewed: 2021-09-30
component: Testing
related:
 - nservicebus/testing
 - nservicebus/upgrades/7to8
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 7
 - 8
---

## Fluent-style API is not supported

Starting with NServiceBus version 8, the fluent-style testing API is not supported. Tests should be written in an [Arrange-Act-Assert (AAA) style](https://docs.microsoft.com/en-us/visualstudio/test/unit-test-basics#write-your-tests). Tests written this way will create the handler or saga to be tested, and call methods on them directly, passing in a testable message handler context that will capture outgoing operations that can be asserted on afterwards.

### Testing a handler

To test a handler, create it with the necessary dependencies, then call the `Handle` method directly. Pass in an instance of `TestableMessageHandlerContext` which will collect all outgoing operations. This context allows customization of metadata about the incoming message, including headers.

An example of the same test written in both the Arrange-Act-Assert style and the fluent style:

snippet: 7to8-testhandler

See [the handler unit testing documentation](/nservicebus/testing/#testing-a-handler) for more information.

### Testing a saga

To test a saga, create it with the necessary dependencies (including the `Data` property), then call the `Handle` method directly. Pass in an instance of `TestableMessageHandlerContext` which will collect all outgoing operations. This context allows customization of metadata about the incoming message, including headers.

Fluent-style saga tests will often include multiple state changes. Arrange-Act-Assert (AAA) tests should test a single state change in isolation. The state of the saga can be configured manually before each test as part of the Arrange step.

This is an example showing two state changes. The first starts the saga that triggers a timeout, then sends a reply, publishes an event, and sends a message. The second state change happens when the timeout occurs, causing another event to be published, and the saga to be completed. These can be split into two tests (one for each state change) when using the Arrange-Act-Assert style.

snippet: 7to8-testsaga

See [the saga unit testing documentation](/nservicebus/testing/#testing-a-saga) for more information.

### Uniform Session

The `NServiceBus.UniformSession.Testing` package provided the `WithUniformSession` method to configure fluent-style tests to work with `IUniformSession`. With AAA-style tests, a new instance of the `TestableUniformSession` class can be created and passed to any class with a dependency on `IUniformSession`:

snippet: 7to8-uniformsession

For scenarios where the tested code path both invokes operations of `IUniformSession` and a pipeline context like `IMessageHandlerContext`, the `TestableUniformSession` can be configured to wrap the context:

snippet: 7to8-uniformsessioncontextwrapping

This approach also works for code that use `IUniformSession` and `IMessageSession` in the same code path:

snippet: 7to8-uniformsessionmessagesessionwrapping
