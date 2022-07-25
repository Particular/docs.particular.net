---
title: NServiceBus Testing Upgrade Version 5 to 6
summary: Instructions on how to upgrade NServiceBus.Testing from version 5 to 6.
reviewed: 2022-06-01
component: Testing
related:
 - nservicebus/testing
 - nservicebus/upgrades/5to6
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

NServiceBus.Testing requires NServiceBus version 6.

As part of upgrading to NServiceBus.Testing version 6, projects will also require an upgrade to [NServiceBus version 6](/nservicebus/upgrades/5to6/).

## New unit testing capabilities

NServiceBus.Testing version 6 provides an alternate way to write tests using a more traditional *Arrange Act Assert* pattern. This allows for easier extension and tooling integration. For more details on the new testing capabilities, see the [Unit Testing NServiceBus 6 Sample](/samples/unit-testing).

## Testing framework

### Removed Test.Initialize()

It is no longer necessary to call `Test.Initialize()` before executing a test. All calls to `Test.Initialize()` can be safely removed.

#### Unobtrusive message conventions

With the removal of `Test.Initialize()`, it is also no longer necessary to configure unobtrusive message conventions.

## Testing message handlers

### ExpectReturn method

Use `ExpectReply` instead of `ExpectReturn`.

### SendToSites methods

`ExpectSendToSites` and `ExpectNotSendToSites` methods have been removed from the testing framework. Handlers using the gateway can still be tested using the `ExpectSend` overload which provides the `SendOption`:

```csharp
// For Testing version 7.x
[Test]
public void ExpectSendToSite()
{
    Test.Handler<MyHandler>()
        .ExpectSend<GatewayMessage>(
            check: (message, options) =>
            {
                return options.GetSitesRoutingTo().Contains("myFavouriteSite");
            })
        .OnMessage(new MyMessage());
}

class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var options = new SendOptions();
        options.RouteToSites("myFavoriteSite");

        return context.Send(new GatewayMessage(), options);
    }
}

// For Testing version 6.x
[Test]
public void ExpectSendToSite()
{
    Test.Handler<MyHandler>()
        .ExpectSend<GatewayMessage>(
            check: (message, options) =>
            {
                return options.GetSitesRoutingTo().Contains("myFavouriteSite");
            })
        .OnMessage(new MyMessage());
}

class MyHandler :
    IHandleMessages<MyMessage>
{
    public Task Handle(MyMessage message, IMessageHandlerContext context)
    {
        var options = new SendOptions();
        options.RouteToSites("myFavoriteSite");

        return context.Send(new GatewayMessage(), options);
    }
}
```

## Testing sagas

### Using When

In NServiceBus version 6 and above, message handlers have an additional `IMessageHandlerContext` parameter. This context parameter must be provided when defining the method to invoke.

```csharp
.When(
    sagaIsInvoked: (saga, context) =>
    {
        return saga.Handle(new StartsSaga(), context);
    })
```

A new overload has been added to simplify this:

```csharp
.When(
    handlerSelector: saga =>
    {
        return saga.Handle;
    },
    message: new StartsSaga())
```

WARNING: It's important to pass the context provided by the delegate arguments to the handle method.

### Configuring a message ID

The message ID can be configured manually using the `ConfigureMessageHandler` option. See [Configuring the context](#configuring-the-context) section below for more details.

## Configuring the context

Both saga and handler tests contain a `ConfigureHandlerContext` method to enable custom configuration of the `IMessageHandlerContext` which is passed to the invoked handler methods.

```csharp
// For Testing version 7.x
Test.Saga<MySaga>()
    .ConfigureHandlerContext(c =>
    {
        c.MessageId = "my message ID";
    })
    .ExpectPublish<MyEvent>()
    .When(s => s.Handle, new StartsSaga());

// For Testing version 6.x
Test.Saga<MySaga>()
    .ConfigureHandlerContext(c =>
    {
        c.MessageId = "my message ID";
    })
    .ExpectPublish<MyEvent>()
    .When(s => s.Handle, new StartsSaga());
```
