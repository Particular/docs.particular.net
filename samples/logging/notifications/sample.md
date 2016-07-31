---
title: Notifications
reviewed: 2016-03-24
component: Core
related:
- nservicebus/recoverability
---

## Code walk-through

This sample shows how to use the notification API to capture the SLR, FLR and error queue events.


## Custom Settings in this sample

This sample uses several non-standard settings.


### Logging

All errors below Fatal are suppressed to reduce the noise related to raising multiple exceptions

snippet: logging


### SLR Time increase

The time to increase changed to 1 second so the wait for all retries to occur is reduced.

snippet: customSLR


## Plugging to the API

The notifications API is exposed as follows.

snippet:endpointConfig

snippet:subscriptions


partial: usage


include: notificationThread