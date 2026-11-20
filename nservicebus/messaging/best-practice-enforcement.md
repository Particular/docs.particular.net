---
title: Enforcement of best practices
summary: How to disable the enforcement of messaging best practices for commands and events at the endpoint or message level
component: Core
reviewed: 2026-11-17
versions: '[6.0,)'
---

By default, [messaging best practices](messages-events-commands.md) are enforced for messages defined as either commands or events. For example, NServiceBus throws an exception when an endpoint tries to publish a command or send an event.

Disable the enforcement for the whole endpoint using:

snippet: DisableBestPracticeEnforcementPerEndpoint

or for a single operation using the options of that message:

snippet: DisableBestPracticeEnforcementPerMessage
