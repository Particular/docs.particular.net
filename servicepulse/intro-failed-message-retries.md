---
title: Retrying failed messages
summary: Describes the concept and lifecycle of retrying messages in ServicePulse
component: ServicePulse
tags:
- ServicePulse
reviewed: 2016-12-15
---

After resolving the issue that caused message processing failures, failed messages can be resent for reprocessing by the corresponding endpoint(s). This is referred to as a "retry" (or a manual retry, in contrast to the automated and configurable [Recoverability](/nservicebus/recoverability/)).

Manual retries of failed messages can be requested via the [Failed Messages page in ServicePulse](/servicepulse/intro-failed-messages.md).

A message that is sent for retry is marked as such, and is not displayed in the failed message list or included in failed message groups, unless the reprocessing fails again.

ServiceControl keeps track of all retry attempts in the background. If a retry operation fails, then ServicePulse will show the number of failed retry attempts.

![Repeated failure indication](images/failed-messages-repeated-failure.png 'width=500')