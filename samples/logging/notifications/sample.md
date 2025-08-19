---
title: Notifications
reviewed: 2024-01-17
component: Core
related:
- nservicebus/recoverability
---

## Introduction

This sample shows how to use the recoverability [error notifications](/nservicebus/recoverability/subscribing-to-error-notifications.md) to capture the following:

* When a [Immediate Retry](/nservicebus/recoverability/#immediate-retries) occurs.
* When a [Delayed Retry](/nservicebus/recoverability/#delayed-retries) occurs.
* When a message fails all retries and is forwarded to the error queue.

## Custom Settings in this sample

This sample uses several non-standard settings.

### Logging

All errors below Critical are suppressed to reduce the noise related to raising multiple exceptions

snippet: logging

### Delayed Retry Time increase

The time to increase changed to 1 second so the wait for all retries to occur is reduced.

snippet: customDelayedRetries

## Plugging into the API

The notifications API is exposed as follows.

snippet: endpointConfig

snippet: subscriptions

Notifications are manipulated at configuration time.

include: notificationThread

## Message Body

include: error-notifications-message-body
