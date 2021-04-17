---
title: Unit Testing NServiceBus
summary: Writing unit tests for NServiceBus systems
reviewed: 2021-04-14
component: Testing
related:
- nservicebus/testing
---

This sample shows how to write unit tests for various NServiceBus components with Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses [NUnit](https://nunit.org/) and testable helper implementations from the `NServiceBus.Testing` package.


## Testing a handler

Given the following handler:

snippet: SimpleHandler

The following test verifies that the handler received a `Reply`:

snippet: HandlerTest


## Testing a saga

Here's an example of a saga that processes an order and gives a 10% discount for orders above 1000:

snippet: SampleSaga

The following unit tests checks that the discount was applied to the total amount:

snippet: SagaTest


## Testing a behavior

The following custom behavior adds a header to an outgoing message when the message is of the type `MyResponse`:

snippet: SampleBehavior

The behavior can be tested similar to a message handler or a saga by using a testable representation of the context:

snippet: BehaviorTest


## Testing IEndpointInstance usage

`IEndpointInstance` is the main entry point for messaging APIs when used outside the pipeline (i.e. outside a saga or handler). One common example is sending a message from a webpage controller. For example the following controller has an injected `IEndpointInstance` and handles a request and sends a message.

snippet: Controller

The test that verifies a `Send` happened:

snippet: EndpointInstanceTest
