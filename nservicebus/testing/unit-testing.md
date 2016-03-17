---
title: Unit Testing NServiceBus 6  
summary: Write unit tests for NServiceBus 6 components
tags: []
redirects:
---

In Version 6 of NServiceBus we've redesigned core parts of the system, allowing more granular support for testing various components, such as Handlers, Sagas and even Behaviors, without needing to spin up an actual endpoint for end-to-end test scenarios. This allows you to write very fast "bare-metal" unit tests for those components, utilizing convenience methods and test-doubles.

## Getting started

To get started, create a test project using your favorite framework, like NUnit, XUnit, or MSTest. Add a reference to the NServiceBus 6 NuGet package, and we're ready to go!

## Unit testing a Handler

Here's an example of testing a `Reply`:

```csharp
[Test]
public async Task Should_Reply()
{
  var myDependency = Mock.Of<IDependency>();
  var context = new TestableMessageHandlerContext();
  var handler = new MyMessageHandler(myDependency);
​
  await handler.Handle(new MyMessage(), context);
​
  context.Replies.Count().ShouldBe(1);
  context.Sent.ShouldContain(m => m....);
  context.Published.ShouldBeEmpty();
}
```

## Unit testing a Saga

```csharp
[Test]
public async Task Saga_Should_Store_Data()
{
  var myDependency = Mock.Of<IDependency>();
  var context = new TestableMessageHandlerContext();
  var saga = new MySaga(myDependency);
  saga.Entity = new MySagaData { Value = "test" };
​
  await saga.Handle(new MyMessage(), context);
​
  saga.Entity.Value.ShouldBe("hello world");
  saga.Completed.ShouldBeTrue();
  context.Sent.Should....
  context.ShouldRequestTimeout(...)
  context.Timeouts.Should(...)
}
```