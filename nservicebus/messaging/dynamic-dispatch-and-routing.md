---
title: Dynamic dispatch and polymorphic routing
summary: How to use NServiceBus to route messages dynamically based on their types
component: core
reviewed: 2024-04-04
related:
 - nservicebus/messaging/messages-as-interfaces
---

NServiceBus makes it easy to support dynamic dispatch and polymorphic message routing scenarios. This technique makes systems more flexible and maintainable by encouraging messages and handlers to be decoupled from each other. It's useful for evolving systems to allow adding new features without affecting existing functionality. It's best described using an example.

## Using NServiceBus for dynamic dispatch

Consider the following handler stubs for a `UserCreated` message which inherits from the `IEvent` stub interface:

snippet: DynamicDispatchHandlerStubs

When a message of type `UserCreated` is published, both handlers will be invoked independently which in itself is useful. However, this idea becomes more powerful as the system evolves and more functionality is added.

Assume there is a separate process that needs to happen if a user is created as part of a campaign. For example, say the subscriber must register statistics about the campaign. In NServiceBus, this can be done by defining a new event and handler:

snippet: DynamicDispatchEvolution

Here, when a `UserCreatedFromCampaign` message is published, the `RecordStatistics` handler is invoked as expected. But since `UserCreatedFromCampaign` inherits from `UserCreated`, the `SaveUser` handler is also invoked, along with the `Audit` handler (since `UserCreated` implements the `IMessage` stub interface). That is, new functionality has been added without having to change the existing handlers. This is the essence of dynamic dispatch: the system decides at runtime which handlers to invoke based on the message type that is being processed.

In this example, the new event, `UserCreatedFromCampaign`, has a clear relationship with `UserCreated`. However, it's possible that other events could be raised in relation to campaigns, not just users being created. This is where polymorphic routing can help.

## Multiple inheritance to support polymorphic routing

In the updated scenario, there are two concerns: a user is created and a campaign event occurred. The handlers for both events look as follows:

snippet: PolymorphicRouting

Now assume the system should perform some action specifically when a user is created from a campaign. For example, perhaps an email is sent to the user thanking them for engaging. This can be accomplished with an event that inherits from both `UserCreated` and `CampaignActivityOccurred` as follows:

snippet: PolymorphicRoutingMultipleInheritance

When a user is created from a campaign, the publisher can publish the latter event. It will get handled by the `SaveUser` and `RecordCampaignActivity` handlers defined earlier, and perhaps another handler that explicitly handles `UserCreatedFromCampaign` events. That is, the message is routed to multiple handlers based on the interfaces it implements; this is polymorphic routing. Furthermore, other campaign events can be created that inherit from `CampaignActivityOccurred` without interfering with any functionality around creating users as part of a campaign.

Combining this approach with the technique of using [interfaces as messages](./messages-as-interfaces.md) will make your system more resilient, extensible, and maintainable over time as handlers can be defined independently of each other much more easily.
