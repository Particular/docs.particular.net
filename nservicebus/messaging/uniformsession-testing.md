---
title: Uniform Session Testing
summary: How to unit test code that uses Uniform Session.
reviewed: 2021-03-30
component: UniformSessionTesting
related:
- nservicebus/messaging/uniformsession
---

Shows the usage of the `NServiceBus.UniformSession.Testing` package.

## Prerequisites for uniform session testing functionality

The approach shown here works with the `NServiceBus.UniformSession` NuGet package version 2.2.0 or above. Install the `NServiceBus.UniformSession.Testing` NuGet package.

### Testing a handler

Construct the handler under test with an instance of `TestableUniformSession` and pass that same instance to the test.

snippet: UniformSessionHandlerTesting

### Testing a saga

Construct the saga under test with an instance of `TestableUniformSession` and pass that same instance to the test.

snippet: UniformSessionSagaTesting

### Testing another service

Construct the service under test with an instance of `TestableUniformSession`. Call the methods being tested and make assertions about the messages sent and published.

snippet: UniformSessionServiceTesting
