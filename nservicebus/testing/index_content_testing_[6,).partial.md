## Testing a handler

Testing a message handler is done using the `TestableMessageHandlerContext` class provided by the `NServiceBus.Testing` package. This class implements `IMessageHandlerContext` and can be passed to the handler under test. After the handler is executed, the `TestableMessageHandlerContext` can be interrogated to assert that various actions (sending a message, publishing an event, etc.) occurred in the handler as expected.

| Property | Description |
|---------------------|----------------------------------------------------------------------------------|
| `SentMessages` | A list of all messages sent by `context.Send()`. |
| `PublishedMessages` | A list of all messages published by `context.Publish()`. |
| `RepliedMessages` | A list of all messages sent using `context.Reply()`. |
| `TimeoutMessages` | A list of all messages sent with a saga timeout header. |
| `ForwardedMessages` | A list of all forwarding destinations set by `context.ForwardCurrentMessageTo()`. |
| `MessageHeaders` | Gets the list of key/value pairs found in the header of the message. |
| `HandlerInvocationAborted` | Indicates if `DoNotContinueDispatchingCurrentMessageToHandlers()` was called. |

### Example

Given the following handler:

snippet: SimpleHandler

This test verifies that a `Reply` occurred:

snippet: HandlerTest



## Testing a saga

Testing a saga uses the same `TestableMessageHandlerContext` as testing a handler. The same properties are used to perform assertions after a saga method is invoked.

NOTE: Because timeouts are technically sent messages, any timeout requested from the saga will appear in both the `TimeoutMessages` and `SentMessages` collections of the `TestableMessageHandlerContext`.

### Example

Here's an example of a saga, that processes an order and gives a 10% discount for orders above an amount of 1000:

snippet: SampleSaga

The following unit test checks that the total amount has the discount applied:

snippet: SagaTest


## Testing a behavior

Behaviors also can be tested, but using a different testable context objects:

| Behavior Context | Testable Implementation |
|-|-|
| `IncomingLogicalMessageContext` | `TestableIncomingLogicalMessageContext` |
| `IncomingPhysicalMessageContext` | `TestableIncomingPhysicalMessageContext` |
| `OutgoingLogicalMessageContext` | `TestableOutgoingLogicalMessageContext` |
| `OutgoingPhysicalMessageContext` | `TestableOutgoingPhysicalMessageContext` |

### Example

The following custom behavior adds a header to an outgoing message in case the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a saga by using a testable representation of the context:

snippet: BehaviorTest

Testable representations are provided for all pipeline behavior contexts. See [the pipeline documentation](/nservicebus/pipeline/) for further details.
