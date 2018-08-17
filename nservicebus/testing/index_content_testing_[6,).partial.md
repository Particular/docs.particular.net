## Testing a handler

Given the following handler:

snippet: SimpleHandler

here is a test that verifies a `Reply` happened:

snippet: HandlerTest

## Testing a saga

Here's an example of a saga, that processes an order and gives a 10% discount for orders above an amount of 1000:

snippet: SampleSaga

The following unit test checks that the total amount has the discount applied:

snippet: SagaTest

## Testing a behavior

The following custom behavior adds a header to an outgoing message in case the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a saga by using a testable representation of the context:

snippet: BehaviorTest

Testable representations are provided for all pipeline behavior contexts. See [the pipeline documentation](/nservicebus/pipeline/) for further details.