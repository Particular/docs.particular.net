---
title: Enforcement of best practices
summary: How to enable/disable the enforcement of messaging best practices based on events and commands
component: Core
reviewed: 2020-06-08
versions: '[6.0,)'
---

By default, [messaging best practices](messages-events-commands.md) are enforced for messages defined as either commands or events.

NOTE: In NServiceBus versions 6 and above, the default behavior can be overridden.

This feature can be disabled at the endpoint level using:

snippet: DisableBestPracticeEnforcementPerEndpoint

or at the message level using:

snippet: DisableBestPracticeEnforcementPerMessage
