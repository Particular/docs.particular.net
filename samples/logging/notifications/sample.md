---
title: Notifications
reviewed: 2017-10-07
component: Core
related:
- nservicebus/recoverability
---

## Introduction

This sample shows how to use the notification API to capture the following:

 * When a [Immediate Retry](/nservicebus/recoverability/#immediate-retries) occurs.
 * When a [Delayed Retry](/nservicebus/recoverability/#delayed-retries) occurs.
 * When a message fails all retries and is forwarded to the error queue.


## Custom Settings in this sample

This sample uses several non-standard settings.


### Logging

All errors below Fatal are suppressed to reduce the noise related to raising multiple exceptions

snippet: logging


### Delayed Retry Time increase

The time to increase changed to 1 second so the wait for all retries to occur is reduced.

snippet: customDelayedRetries


## Plugging to the API

The notifications API is exposed as follows.

snippet: endpointConfig

snippet: subscriptions


partial: usage


include: notificationThread


## Message Body

include: error-notifications-message-body