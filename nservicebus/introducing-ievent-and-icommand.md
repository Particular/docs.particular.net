---
title: IEvent and ICommand
summary: 'Message interfaces IEvent and ICommand capture the intent of the messages. '
tags: []
---

A feature of NServiceBus V3.X and V4.X are two message interfaces, `IEvent` and `ICommand`, which capture more of the intent of the messages that you define. This helps NServiceBus enforce messaging best practices.

Messages implementing `ICommand`:

-   Are not allowed to be published since all commands should have one logical owner and should be sent to the endpoint responsible for processing
-   Cannot be subscribed and unsubscribed to
-   Cannot implement `IEvent`

Messages implementing `IEvent`:

-   Can be published
-   Can be subscribed and unsubscribed to
-   Cannot be sent using Bus.Send() since all events should be published
-   Cannot implement `ICommand`
-   Cannot be sent using the gateway, i.e., `bus.SendToSites()`

To describe your message intent, use one of these methods:

-   For reply messages in a request and response pattern, you may want to use `IMessage` since these replies are neither a command or an event.
-   These interfaces make your message classes dependent on a specific version of the `NServiceBus.dll`. To avoid this and to make your messages more cross-version compatible, use the unobtrusive mode for defining message intent using the Fluent configuration. See the [Unobtrusive sample](unobtrusive-sample.md) for more information on how to specify a command and an event.


