---
title: Notifications
summary: Using the notifications API.
reviewed: 2016-03-24
component: Core
related:
- nservicebus/errors/subscribing-to-error-notifications
- nservicebus/errors/automatic-retries
- nservicebus/errors
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


### Versions 6 and above

In Version 6 notifications are manipulated at configuration time.


### Version 5

In Version 5 notifications are manipulated at startup time using a combination of `IWantToRunWhenBusStartsAndStops` and an instance of `BusNotifications` injected via the [container](/nservicebus/containers/).


include: notificationThread