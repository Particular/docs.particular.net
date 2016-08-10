---
title: Enforcement of best practices
summary: How to enable/disable the enforcement of messaging best practices based on Events and Commands
component: Core
versions: '[6.0,)'
tags:
- Convention
- Command
- Event
---

By default NServiceBus will ensure [messaging best practices](messages-events-commands.md) for messages define as either Commands or Events.

NOTE: In Versions 6 and above the default behavior can be overridden.

To enable this feature at the endpoint level using:

snippet:DisableBestPracticeEnforcementPerEndpoint

or at the message level using:

snippet:DisableBestPracticeEnforcementPerMessage
