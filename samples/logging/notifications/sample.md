---
title: Notifications
summary: Using the notifications API.
reviewed: 2016-03-21
tags:
related:
- nservicebus/errors/subscribing-to-error-notifications
- nservicebus/errors/automatic-retries
- nservicebus/errors
---

## Code walk-through

This sample shows how to use the notification API to capture the SLR, FLR and error queue events.


## Custom Setting in this sample

This sample uses several non-standard settings.


### Logging

All errors below Fatal are suppressed to reduce the noise related to raising multiple exceptions

snippet: logging


### SLR Time increase

The time to increase changed to 1 second so the wait for all retries to occur is reduced.

snippet: customSLR


## Plugging to the API

The notifications API is exposed via the `BusNotifications` (NServiceBus Version 5) or `Notifications` (NServiceBus Version 6) class. This class can be injected using the [container](/nservicebus/containers).

snippet:subscriptions


include: notificationThread
