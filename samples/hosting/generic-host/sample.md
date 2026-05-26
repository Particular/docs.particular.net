---
title: Endpoint hosting with the Generic Host
summary: Hosting an endpoint with the Microsoft Generic Host
reviewed: 2026-05-26
component: Core
related:
- nservicebus/hosting/
---

## Code walk-through

The builder [configures NServiceBus on the generic host](/nservicebus/hosting/core-hosting.md#hosting-a-single-endpoint), including the [critical error](/nservicebus/hosting/critical-errors.md) action, which shuts down the application or service in the event of a critical error.

snippet: generic-host-nservicebus

The critical error action:

snippet: generic-host-critical-error

To simulate work, a BackgroundService called `Worker` is registered as a hosted service:

snippet: generic-host-worker-registration

The `IMessageSession` is injected into the `Worker` constructor, and the `Worker` sends messages when it is executed.

snippet: generic-host-worker

For more worker service options like installing as Windows Services, Linux Daemons, etc., [see the Microsoft documentation](https://learn.microsoft.com/en-us/dotnet/core/extensions/workers).
