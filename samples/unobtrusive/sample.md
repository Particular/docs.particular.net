---
title: Unobtrusive
summary: Demonstrates NServiceBus operating in unobtrusive mode.
reviewed: 2021-04-29
component: Core
redirects:
- nservicebus/unobtrusive-sample
related:
- nservicebus/messaging/unobtrusive-mode
---

Run the solution. Two console applications should start up, `Client` and `Server`.


## Configuring conventions for unobtrusive mode

Look at the `ConventionExtensions` class. The code tells NServiceBus how to determine which types are message definitions by passing in custom conventions instead of using the `IMessage`, `ICommand`, or `IEvent` interfaces:

snippet: CustomConvention

The code tells NServiceBus to treat all types with a namespace that ends with "Messages" the same as for messages that explicitly implement `IMessage`.

It also shows the unobtrusive way to tell NServiceBus which properties to deliver on a separate channel from the message itself using the [data bus](/nservicebus/messaging/databus/) feature, and which messages have a defined time to be received.

Look at the code. There are a number of projects in the solution:

 * **Client**: sends a request and a command to the server and handles a published event
 * **Server**: handles requests and commands, and publishes events
 * **Shared**: the shared message definitions
