---
title: Configuring enforcement of best practices
summary: How to enable/disable the enforcement of messaging best practices based on Events and Commands
tags: 
- Conventions, Message Semantics, Command, Event
---

By default NServiceBus will make sure that you are following [messaging best practices](messages-events-commands.md) for the messages you defines as either commands or events. In versions prior to v6 the only way to bypass those enforcements was to just define all your messages as plain messages. While this worked it caused other features like [auto subscribe](publish-subscribe/how-to-pub-sub.md) to stop working since only `Events` are auto subscribed.

As of version 6 you can now turn this feature on and off on the endpoint level using:

<!-- import DisableBestPracticeEnforcementPerEndpoint -->

or at the message level using:

<!-- import DisableBestPracticeEnforcementPerMessage -->

