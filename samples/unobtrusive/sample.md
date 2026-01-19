---
title: Unobtrusive
summary: Demonstrates NServiceBus operating in unobtrusive mode.
reviewed: 2026-01-19
component: Core
redirects:
- nservicebus/unobtrusive-sample
related:
- nservicebus/messaging/unobtrusive-mode
---

## Code walk-through

There sample consists of the following projects:

 * **Client**: sends a request and a command to the server and handles a published event
 * **Server**: handles requests and commands, and publishes events
 * **Shared**: the shared message definitions

## Configuring conventions for unobtrusive mode

The `ConventionExtensions` class tells NServiceBus how to determine which types are message definitions by passing in custom conventions instead of using the `IMessage`, `ICommand`, or `IEvent` interfaces:

snippet: CustomConvention

It also demonstrates how to configure conventions for the [data bus](/nservicebus/messaging/claimcheck/) and [time-to-be-received](/nservicebus/messaging/discard-old-messages.md) features.
