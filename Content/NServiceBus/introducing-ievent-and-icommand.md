---
title: "IEvent and ICommand"
tags: ""
summary: "A feature of NServiceBus V3 is the introduction of two new message interfaces, IEvent and ICommand, which capture more of the intent of the messages that you define. This helps NServiceBus enforce messaging best practices."
---

A feature of NServiceBus V3 is the introduction of two new message interfaces, IEvent and ICommand, which capture more of the intent of the messages that you define. This helps NServiceBus enforce messaging best practices.

Messages implementing ICommand:

-   Are not allowed to be published since all commands should have one
    logical owner and should be sent to the endpoint responsible for
    processing
-   Cannot be subscribed and unsubscribed to
-   Cannot implement IEvent

Messages implementing IEvent:

-   Can be published
-   Can be subscribed and unsubscribed to
-   Cannot be sent using Bus.Send() since all events should be published
-   Cannot implement ICommand
-   Cannot be sent using the gateway, ie bus.SendToSites()

So use these additional interfaces if you want NServiceBus to help you succeed.

See the [Unobtrusive sample](https://github.com/NServiceBus/NServiceBus/tree/master/Samples/Unobtrusive) for other information on how to specify a command and an event.

