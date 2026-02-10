---
title: Saga scenario testing
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2025-05-30
component: Testing
versions: '[7.4,)'
related:
---

While [each handler in a saga can be tested using a unit test](/samples/unit-testing/#testing-a-saga), it's often helpful to test an entire scenario involving multiple messages.

The `TestableSaga` class enables this kind of scenario testing and supports the following:

* Exercises the `ConfigureHowToFindSaga` method to ensure mappings are valid.
* Emulates saga processing behavior in NServiceBus, including automatic correlation property assignment in saga data upon receiving the first message.
* Stores timeouts internally, which can be triggered by [advancing time](#advancing-time).

> [!NOTE]
> Testing [custom finders](/nservicebus/sagas/saga-finding) and [saga not found handlers](https://docs.particular.net/nservicebus/sagas/saga-not-found) is currently not supported.

## Example

A simple scenario test of a `ShippingPolicy` saga, including timeouts:

snippet: BasicScenarioTest

## Creating a testable saga

Often, a testable saga can be created using just the type parameters:

snippet: TestableSagaCtor

This assumes a parameterless constructor. If the saga requires injected services, a factory can be provided to create an instance per handled message:

snippet: TestableSagaCtorFactory

By default, `CurrentTime` is initialized to `DateTime.UtcNow`, but a specific value can be set via the constructor. See [advancing time](#advancing-time) for details:

snippet: TestableSagaCtorTime

## Handling messages

The testable saga mimics NServiceBus saga infrastructure. Each time it handles a message, it:

1. Instantiates a new saga instance.
2. Uses `ConfigureHowToFindSaga` to locate the matching saga data in internal storage.

To handle a message:

snippet: TestableSagaSimpleHandle

Optional parameters allow using a custom `TestableMessageHandlerContext` or providing custom headers:

snippet: TestableSagaHandleParams

## Handler results

Handling a message returns a `HandleResult`, which contains:

* `SagaId`: The `Guid` of the saga instance created or loaded.
* `Completed`: Whether `MarkAsComplete()` was called.
* `HandledMessage`: Type, headers, and body of the message handled.
* `Context`: A [`TestableMessageHandlerContext`](/nservicebus/testing/#testing-a-handler) with sent/published messages and other operations during handling.
* `SagaDataSnapshot`: Copy of saga data after handling.
* Helpers for locating specific messages in the `Context`:
  * `FindSentMessage<TMessage>()`
  * `FindPublishedMessage<TMessage>()`
  * `FindTimeoutMessage<TMessage>()`
  * `FindReplyMessage<TMessage>()`

## Advancing time

`CurrentTime` represents a virtual clock. It defaults to the test start time but can be set in the [constructor](#creating-a-testable-saga).

Timeouts are stored during message handling. Calling `AdvanceTime` processes due timeouts and returns an array of `HandleResult`:

snippet: TestableSagaAdvanceTime

Need custom `TestableMessageHandlerContext` per timeout? Use the overload:

snippet: TestableSagaAdvanceTimeParams

## Simulating external handlers

Sagas often send commands to external handlers, expecting replies. These reply messages are [auto-correlated](/nservicebus/sagas/message-correlation.md#auto-correlation) using a saga ID header.

Simulate such replies with `SimulateReply`:

snippet: TestableSagaSimulateReply

The reply is enqueued internally, with the correlation header set. If you want to manually handle a reply instead:

snippet: TestableSagaHandleReply

You can also supply custom headers or a custom context:

snippet: TestableSagaHandleReplyParams

## Queued messages

Any message sent/published that the saga handles is queued internally. This includes:

* Saga sends/publishes of messages it also handles.
* Messages produced by [external handler simulations](#simulating-external-handlers).

You can inspect and assert against the message queue:

snippet: TestableSagaQueueOperations

To process the next queued message:

snippet: TestableSagaHandleQueuedMessage

> [!NOTE]
> Use `HandleQueuedMessage()` to test specific message ordering or simulate race conditions. Control whether timeouts or replies are processed first by choosing `AdvanceTime()` or `HandleQueuedMessage()` accordingly.

## Additional examples

For more usage patterns, see the [NServiceBus.Testing saga tests](https://github.com/Particular/NServiceBus.Testing/tree/master/src/NServiceBus.Testing.Tests/Sagas).
