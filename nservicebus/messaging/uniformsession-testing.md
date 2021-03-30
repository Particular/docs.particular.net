---
title: Uniform Session Testing
summary: Shows how to unit test code that uses Uniform Session.
reviewed: 2021-03-30
component: UniformSessionTesting
related:
- nservicebus/messaging/uniformsession
---

Shows the usage of the `NServiceBus.UniformSession.Testing` package.


## Prerequisites for uniform session testing functionality

The approach shown here works with the `NServiceBus.UniformSession` NuGet package version X.Y.Z or above. Install the `NServiceBus.UniformSession.Testing` NuGet package.


### Testing a handler

Construct the handler under test with an instance of `TestableUniformSession` and pass that same instance to the test.

```csharp
var session = new TestableUniformSession();
var handler = new SomeMessageHandler(session);
Test.Handler(handler)
  .WithUniformSession(session)
  .ExpectPublish<SomeEvent>()
  .OnMessage<SomeMessage>();
```


### Testing a saga

Construct the saga under test with an instance of `TestableUniformSession` and pass that same instance to the test.

```csharp
var session = new TestableUniformSession();
var saga = new SomeSaga(session);
Test.Saga(saga)
  .WithUniformSession(session)
  .ExpectPublish<SomeEvent>()
  .WhenHandling<SomeCommand>();
```


### Testing another service

Construct the service under test with an instance of `TestableUniformSession`. Call the methods being tested and make assertions about the messages sent and published.

```csharp
var session = new TestableUniformSession();
var someService = new SomeService(session);

someService.DoTheThing();

Assert.AreEqual(1, session.SentMessages.Length);
```