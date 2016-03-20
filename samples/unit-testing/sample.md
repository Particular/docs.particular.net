---
title: Unit Testing NServiceBus 6
summary: How to write unit tests for NServiceBus 6 systems
tags:
- Unit Testing
- Arrange-Act-Assert
- NUnit
redirects:
related:
- nservicebus/testing
---

This sample shows how to write unit tests for various NServiceBus components with Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses NUnit, and utilizes testable helper implementations from the `NServiceBus.Testing` package.

### Testing a handler

Given the following handler:

snippet:SimpleHandler

We can write a tests that verifies a `Reply` happened with the following test:

snippet:HandlerTest 

### Testing a Saga

Here's an example of a Saga, that processes an order, and gives a 10% discount for orders above the amount of 1000:

snippet:SampleSaga

The following unit tests checks that the total amount has the discount applied:

snippet:SagaTest
