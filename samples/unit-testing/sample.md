---
title: Unit Testing NServiceBus
summary: Writing unit tests for NServiceBus systems.
reviewed: 2016-03-31
component: Testing
tags:
- Testing
redirects:
related:
- nservicebus/testing
---

This sample shows how to write unit tests for various NServiceBus components with Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses [NUnit](http://www.nunit.org/), and utilizes testable helper implementations from the `NServiceBus.Testing` package.


### Testing a handler

Given the following handler:

snippet:SimpleHandler

The test that verifies a `Reply` happened:

snippet:HandlerTest


### Testing a Saga

Here's an example of a Saga, that processes an order, and gives a 10% discount for orders above the amount of 1000:

snippet:SampleSaga

The following unit tests checks that the total amount has the discount applied:

snippet:SagaTest
