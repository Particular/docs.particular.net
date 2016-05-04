---
title: Enforcement of best practices
summary: How to enable/disable the enforcement of messaging best practices based on Events and Commands
tags:
- Convention
- Command
- Event
---

By default NServiceBus will ensure [messaging best practices](messages-events-commands.md) for messages define as either Commands or Events. While this worked it caused other features like [auto subscribe](publish-subscribe/controlling-what-is-subscribed.md) to stop working since only `Events` are auto subscribed.

These enforcements can be bypassed by defining all messages as plain messages.

NOTE: In Versions 6 and above the default behavior can be overridden.

To enable this feature at the endpoint level using:

snippet:DisableBestPracticeEnforcementPerEndpoint

or at the message level using:

snippet:DisableBestPracticeEnforcementPerMessage