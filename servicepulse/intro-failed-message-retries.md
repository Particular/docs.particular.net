---
title: Retrying failed messages
summary: Describes the concept and lifecycle of retrying messages in ServicePulse
component: ServicePulse
tags:
- ServicePulse
reviewed: 2016-12-15
---

After addressing the root cause of the message's processing failure, failed messages can be resent for reprocessing by the corresponding endpoint(s). This is referred to as a "retry" (or a manual retry, in contrast to the automated and configurable [Recoverability](/nservicebus/recoverability/)).

Manual retries of failed messages can be sent via the [Failed Messages page in ServicePulse](/servicepulse/intro-failed-messages).

A message that is sent for retry is marked as such, and is not displayed in the failed message list or included in failed message groups, unless the reprocessing fails again. Messages sent for retry appear in the [Pending Retries screen](intro-pending-retries.md) until either an audit message is received indicating success, the message is returned as an error, or the message is manually marked as resolved.

If a message fails repeated retry attempts, an indication is added, including the number of times it has failed.

![Repeated failure indication](images/failed-messages-repeated-failure.png 'width=500')

NOTE: The number of retry attempts for a message can be significant if the handler for that message is not [idempotent](/nservicebus/concept-overview.md#idempotence). Any processing attempt that invokes logic that does not participate in the NServiceBus transactional processing will not be rolled back on processing failure.