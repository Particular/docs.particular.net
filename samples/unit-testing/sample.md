---
title: Unit Testing NServiceBus
summary: Writing unit tests for NServiceBus systems.
reviewed: 2017-07-17
component: Testing
related:
- nservicebus/testing
---

This sample shows how to write unit tests for various NServiceBus components with Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses [NUnit](http://nunit.org/), and utilizes testable helper implementations from the `NServiceBus.Testing` package.


## Testing a handler

Given the following handler:

snippet: SimpleHandler

The test that verifies a `Reply` happened:

snippet: HandlerTest


## Testing a Saga

Here's an example of a Saga, that processes an order, and gives a 10% discount for orders above the amount of 1000:

snippet: SampleSaga

The following unit tests checks that the total amount has the discount applied:

snippet: SagaTest


## Testing a behavior

The following custom behavior adds a header to an outgoing message in case the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a Saga by using a testable representation of the context:

snippet: BehaviorTest


## Testing IEndpointInstance usage

`IEndpointInstance` is the main entry point for messaging APIs when used outside the pipeline (i.e. not Sagas or Handlers). One such common example is sending a message from a webpage controller (or any other web-request handling code). For example a controller, with an injected `IEndpointInstance`, handles a request and sends a message.

snippet: Controller

The test that verifies a `Send` happened:

snippet: EndpointInstanceTest