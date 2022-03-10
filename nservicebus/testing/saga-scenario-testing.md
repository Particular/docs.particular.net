---
title: Saga scenario testing
summary: Develop service layers and long-running processes using test-driven development.
reviewed: 2020-05-07
component: Testing
versions: '[7.4,)'
related:
---

While [each handler in a saga can be tested using a unit test](/samples/unit-testing/#testing-a-saga), often it is advantageous to test an entire scenario involving multiple messages.

The `TestableSaga` class allows this type of scenario testing and supports the following features:

* Exercises the `ConfigureHowToFindSaga` method to ensure that mappings are valid.
* Emulates how sagas are processed by NServiceBus, including automatically setting the correlation property in the saga data when the first message is received.
* Stores timeouts internally, which can be triggered by [advancing time](#advancing-time).

## Example

Here's a simple sample of a scenario test of a `ShippingPolicy` saga, including timeouts:

snippet: BasicScenarioTest

## Creating a testable saga

In many cases a testable saga can be created using only the type parameters from the saga.

snippet: TestableSagaCtor

This assumes that the saga has a parameterless constructor. If the saga has a constructor that requires services to be injected, a factory can be specified to create an instance of the saga class for each handled message.

snippet: TestableSagaCtorFactory

By default, the `CurrentTime` for the saga will be set to `DateTime.UtcNow`. The constructor can also be used to set the `CurrentTime` to a different initial value. For more details see [advancing time](#advancing-time).

snippet: TestableSagaCtorTime

## Handling messages

The testable saga is like the saga infrastructure in NServiceBus. Every time it is asked to handle a message, it will instantiate a new instance of the saga class and use the mapping information in the `ConfigureHowToFindSaga` method to locate the correct saga data in the internal storage.

To have the saga infrastructure handle a message, use the `Handle` method:

snippet: TestableSagaSimpleHandle

If necessary, optional parameters exist to allow the use of a custom `TestableMessageHandlerContext` or specify custom message headers:

snippet: TestableSagaHandleParams

## Handler results

The `HandleResult` returned when each message is handled contains information about the message that was handled and the result of that operation which can be used for assertions.

The `HandleResult` class contains:

* `SagaId`: Identifies the `Guid` of the saga that was either created or retrieved from storage.
* `Completed`: Indicates whether the handler invoked the `MarkAsComplete()` method.
* `HandledMessage`: Contains the message type, headers, and content of the message that was handled.
* `Context`: A [`TestableMessageHandlerContext`](/nservicebus/testing/#testing-a-handler) which contains information about messages sent/published as well as any other operations that occurred on the `IMessageHandlerContext` while the message was being handled.
* `SagaDataSnapshot`: Contains a copy of the saga data after the message handler completed.
* Convenience methods for finding messages of a given type inside the `Context`:
  * `FindSentMessage<TMessage>()`
  * `FindPublishedMessage<TMessage>()`
  * `FindTimeoutMessage<TMessage>()`
  * `FindReplyMessage<TMessage>()`

## Advancing time

The testable saga contains a `CurrentTime` property that represents a virtual clock for the saga scenario. The `CurrentTime` defaults to the time when test execution starts, but can be optionally specified [in the `TestableSaga` constructor](#creating-a-testable-saga).

As each message handler runs, any timeouts are collected in an internal timeout storage. By calling the `AdvanceTime` method, these timeouts will come due and the messages they contain will be handled. The `AdvanceTime` method returns an array of `HandleResult`, one for each timeout that is handled.

snippet: TestableSagaAdvanceTime

If a custom `TestableMessageHandlerContext` is needed to process each timeout, an optional parameter allows creating them:

snippet: TestableSagaAdvanceTimeParams

## Simulating external handlers

Many sagas will send commands to external handlers, which do some work and then send a reply message back to the saga so that the saga can move on to the next step of a multi-step process. These reply messages are [auto-correlated](/nservicebus/sagas/message-correlation.md#auto-correlation): the saga includes the saga ID as a message header in the outbound message, and the external handler returns that message when it does a reply.

In a saga scenario test, the external handler's response can be simulated using the `SimulateReply` method:

snippet: TestableSagaSimulateReply

When the saga being tested sends a `DoStep1` command, the reply will be simulated using the provided `Func<TSagaMessage, TReplyMessage>` delegate. The resulting `Step1Response` message will be added to the testable saga's [internal queue](#queued-messages), including the header containing the saga's ID so that a `ConfigureHowToFindSaga` mapping is not required.

Alternatively, a reply message can be handled directly without using a simulator, but then the `SagaId` value must be provided:

snippet: TestableSagaHandleReply

The `HandleReply` method also contains optional parameters for a custom `TestableMessageHandlerContext` or additional message headers:

snippet: TestableSagaHandleReplyParams

## Queued messages

Any message generated of a type that is handled by the saga will be added to the testable saga's internal queue. This includes:

* When a saga handler sends or publishes any message that the saga itself handles. This is commonly done within a saga to create a new transactional scope around a new message.
* When using a [external handler simulator](#simulating-external-handlers), the resulting message is added to the queue.

The testable saga has several methods available to evaluate what is in the queue, which can be used for test assertions

snippet: TestableSagaQueueOperations

The next message in the queue is handled by calling `HandleQueuedMessage()`:

snippet: TestableSagaHandleQueuedMessage

NOTE: Using `HandleQueuedMessage()` allows specific ordering of message processing in order to write tests related to specific ordering and race condition concerns. Whether or not a timeout or a reply message is handled first in a specific scenario is controlled by whether the test calls the `AdvanceTime()` or `HandleQueuedMessage()` method.

## Additional examples

For more examples of what is possible with saga scenario tests, see the [saga tests in the NServiceBus.Testing repository](https://github.com/Particular/NServiceBus.Testing/tree/master/src/NServiceBus.Testing.Tests/Sagas).