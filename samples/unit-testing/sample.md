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

This sample shows how to write unit tests for various NServiceBus components using "bare-metal", Arrange-Act-Assert (AAA) style tests. This sample is a test project that uses NUnit, and utilizes testable helper implementations from the `NServiceBus.Testing` package.

### Testing a handler

Given the following handler:

snippet:SimpleHandler

We can write a tests that verifies a `Reply` happened with the following test:

snippet:HandlerTest 
