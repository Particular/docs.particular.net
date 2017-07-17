
### Testing a handler

Given the following handler:

snippet: SimpleHandler

The test that verifies a `Reply` happened:

snippet: HandlerTest


### Testing a Saga

Here's an example of a Saga, that processes an order, and gives a 10% discount for orders above the amount of 1000:

snippet: SampleSaga

The following unit tests checks that the total amount has the discount applied:

snippet: SagaTest


### Testing a behavior

The following custom behavior adds a header to an outgoing message in case the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a Saga by using a testable representation of the context:

snippet: BehaviorTest