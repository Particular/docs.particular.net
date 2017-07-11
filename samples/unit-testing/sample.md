---
title: Unit Testing NServiceBus
summary: Writing unit tests for NServiceBus systems.
reviewed: 2016-03-31
component: Testing
related:
- nservicebus/testing
---

This sample shows how to write unit tests for various NServiceBus components with Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses [NUnit](http://nunit.org/), and utilizes testable helper implementations from the `NServiceBus.Testing` package.


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

The behavior can be tested similar to a message handler or a Saga by using a testable represantion of the context:

snippet: BehaviorTest