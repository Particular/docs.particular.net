## Testing a handler

Testing a message handler is done using the `TestableMessageHandlerContext` class provided by the `NServiceBus.Testing` package. This class implements `IMessageHandlerContext` and can be passed to the handler under test. After the handler is executed, the `TestableMessageHandlerContext` can be interrogated to assert that various actions (sending a message, publishing an event, etc.) occurred in the handler as expected.

| Property | Description |
|---------------------|----------------------------------------------------------------------------------|
| `SentMessages` | A list of all messages sent by `context.Send()` |
| `PublishedMessages` | A list of all messages published by `context.Publish()` |
| `RepliedMessages` | A list of all messages sent using `context.Reply()` |
| `TimeoutMessages` | A list of all messages resulting from use of `Saga.RequestTimeout()` |
| `ForwardedMessages` | A list of all forwarding destinations set by `context.ForwardCurrentMessageTo()` |
| `MessageHeaders` | Gets the list of key/value pairs found in the header of the message |
| `HandlerInvocationAborted` | Indicates if `DoNotContinueDispatchingCurrentMessageToHandlers()` was called |

### Example

Given the following handler:

snippet: SimpleHandler

This test verifies that a `Reply` occurred:

snippet: HandlerTest

### Interface messages

When using [interface messages](/nservicebus/messaging/messages-as-interfaces.md), an instance of the message can be created by either defining a custom implementation of the message interface or by using the `MessageMapper` as shown in the following snippet:

snippet: InterfaceMessageCreation

## Testing message session operations

Use `TestableMessageSession` to test message operations outside of handlers. The following properties are available:

| Property | Description |
|---------------------|----------------------------------------------------------------------------------|
| `SentMessages` | A list of all messages sent by `session.Send()` |
| `PublishedMessages` | A list of all messages published by `session.Publish()` |
| `Subscriptions` | A list of all message types explicitly subscribed to using `session.Subscribe()` |
| `Unsubscriptions` | A list of all message types explicitly unsubscribed to using `session.Unsubscribe()` |

### Example

The following code shows how to verify that a message was `Sent` using `IMessageSession`.

snippet: TestMessageSessionSend

## Testing a saga

Testing a saga uses the same `TestableMessageHandlerContext` as testing a handler. The same properties are used to perform assertions after a saga method is invoked.

NOTE: Because timeouts are technically sent messages, any timeout requested from the saga will appear in both the `TimeoutMessages` and `SentMessages` collections of the `TestableMessageHandlerContext`.

### Example

Here's an example of a saga, that processes an order and gives a 10% discount for orders above an amount of 1000:

snippet: SampleSaga

The following unit test checks that the total amount has the discount applied:

snippet: SagaTest

## Testing a behavior

[Message pipeline behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md) also can be tested, but using different testable context objects. Each of the [pipeline stages](/nservicebus/pipeline/steps-stages-connectors.md) uses a specific interface for its context, and each context interface has a testable implementation.

To determine the testable context for a behavior context, replace the `I` at the beginning of the interface name with `Testable`.

For example: 

* A behavior using `IIncomingLogicalMessageContext` can be tested using `TestableIncomingLogicalMessageContext`.
* A behavior using `IInvokeHandlerContext` can be tested using `TestableInvokeHandlerContext`.

Refer to the [pipeline stages document](/nservicebus/pipeline/steps-stages-connectors.md) for a complete list of the available behavior contexts.

Each of these testable types contains properties similar to those found in `TestableMessageHandlerContext` that can be used to assert that a behavior is working as designed.

### Example

The following custom behavior adds a header to an outgoing message in case the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a saga by using a testable representation of the context:

snippet: BehaviorTest

## Testing logging behavior

To test that logging is performed correctly, use the `TestingLoggerFactory`. The factory writes to a `StringWriter` to allow unit tests to assert on log statements.

### Example

NOTE: Using `WriteTo` or `Level` set the provided parameters to the statically cached factory for the lifetime of the application domain. For isolation of logging in concurrent scenarios it is recommended to use `BeginScope` that was introduced in Version 7.2.

The following code show how to verify that logging is performed by the message handler.

snippet: LoggerTestingSetup

The setup fixture above sets the testing logging factory once per assembly because the factory is statically cached during the lifetime of the application domain. Subsequent test executions then clear the logged statements before every test run as shown below.

snippet: LoggerTesting