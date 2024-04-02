---
title: Messages as Interfaces
summary: Define messages as interfaces in NServiceBus to support dynamic dispatch and polymorphic routing scenarios
component: core
reviewed: 2024-03-28
redirects:
- nservicebus/messages-as-interfaces
---

NServiceBus supports using interfaces as messages to allow dynamic dispatch and polymorphic message routing scenarios. This makes systems more flexible and maintainable by encouraging messages and handlers to be decoupled from each other. It's useful for evolving systems to allow adding new features without affecting existing functionality. It's best described using an example.

## Using NServiceBus for dynamic dispatch

Consider the following handler stubs for a `UserCreated` message which inherits from the `IEvent` stub interface:

```csharp
public class SaveUser : IHandleMessages<UserCreated>
{
    public async Task Handle(UserCreated message, IMessageHandlerContext) {
        // Do stuff when the user is created
    }
}

public class Audit : IHandleMessages<IMessage>
{
    public async Task Handle(IMessage message, IMessageHandlerContext context) {
        // Audit the message
    }
}
```

When a message of type `UserCreated` is published, both handlers will be invoked independently which in itself is useful. However, this idea becomes more powerful as the system evolves and more functionality is added.

Assume there is a separate process that needs to happen if a user is created as part of a campaign. For example, say the subscriber must register statistics about the campaign. In NServiceBus, this can be done by defining a new event and handler:

```csharp
public class UserCreatedFromCampaign : UserCreated
{
    public Guid CampaignId { get; set;}
}

public class RecordStatistics : IHandleMessages<UserCreatedFromCampaign>
{
    public async Task Handle(UserCreatedFromCampaign, IMessageHandlerContext context) {
        // Record statistics
    }
}
```

Here, when a `UserCreatedFromCampaign` message is published, the `RecordStatistics` handler is invoked as expected. But since `UserCreatedFromCampaign` inherits from `UserCreated`, the `SaveUser` handler is also invoked, along with the `Audit` handler (since `UserCreated` implements the `IMessage` stub interface). That is, new functionality has been added without having to change the existing handlers.

In this example, the new event, `UserCreatedFromCampaign`, has a clear relationship with `UserCreated`. However, it's possible that other events could be raised in relation to campaigns, not just users being created. This is where polymorphic routing can help.

## Multiple inheritance to support polymorphic routing

In the updated scenario, there are two concerns: a user is created and a campaign event occurred. The handlers for both events look as follows:

```csharp
public class SaveUser : IHandleMessages<UserCreated>
{
    public async Task Handle(UserCreated message, IMessageHandlerContext) {
        // Do stuff when the user is created
    }
}

public class RecordCampaignActivity : IHandleMessages<CampaignActivityOccurred>
{
    public async Task Handle(CampaignActivityOccurred message, IMessageHandlerContext) {
        // Do stuff when an event related to a campaign happened 
    }
}
```

Now assume the system should perform some action specifically when a user is created from a campaign. For example, perhaps an email is sent to the user thanking them for engaging. This can be accomplished with an event that inherits from both `UserCreated` and `CampaignActivityOccurred` as follows:

```csharp
public interface UserCreated : IEvent
{
    Guid UserId { get; set;}
}

public interface CampaignActivityOccurred : IEvent
{
    Guid CampaignId { get; set;}
}

public interface UserCreatedFromCampaign : UserCreated, CampaignActivityOccurred
{
}
```

When a user is created from a campaign, the publisher can publish the latter event. It will get handled by the `SaveUser` and `RecordCampaignActivity` handlers defined earlier, and perhaps another handler that explicitly handles `UserCreatedFromCampaign` events. Furthermore, other campaign events can be created that inherit from `CampaignActivityOccurred` without interfering with any functionality around creating users as part of a campaign.

With this approach, events can support multiple inheritance and the system can create messages on the fly that implement these interfaces as described above.

## Sending interface messages

Interface messages can be sent using the following syntax:

snippet: InterfaceSend

Replies are supported via:

snippet: InterfaceReply

## Publishing interface messages

Interface messages can be published using the following syntax:

snippet: InterfacePublish

## Creating interface messages with IMessageCreator

If an interface message is needed before calling `Send` or `Publish`, use `IMessageCreator` directly to create the message instance:

snippet: IMessageCreatorUsage
