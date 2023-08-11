---
title: Recoverability
summary: Recoverability techniques to build resilient distributed systems
reviewed: 2023-07-18
---

Failures in message-driven systems don't impact the publisher of the failed message. Due to the persistent nature of messages, even failed messages will remain in the system until the issue is resolved and the message can be successfully processed. This increases the system resiliency as local failures don't spread further and no data is lost. Different strategies can be applied to deal with different types of failures:

## Transient errors

Transient failures are failures of a temporary nature that are not directly caused by the business logic being executed, e.g. network issues, throttling, concurrency conflicts, etc. Resilient applications are able to absorb such failures through [self-healing](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/self-healing). Retries are a good solution to deal with transient errors. There are two common retry patterns:

- Immediate retries: Many transient failures (e.g. concurrency errors) can be resolved by immediately retrying messages again. However, immediate retries might not be the right approach when the root cause is due to overloaded or throttled resources.
- Delayed retries: Infrastructure related transient failures (e.g. network problems) might require more time to resolve. In this case it makes more sense to retry in the near future. Different strategies to determine the retry delay time can be used (e.g. fixed intervals, expontential backoff, exception-based values).

Note: The nature of retries implies that messages might end up in a different order under some circumstances.

{{Note:
The Particular Platform simplifies handling of transient errors:
- NServiceBus has built-in support for immediate and delayed retries for all supported messaging technologies. The automated retries behavior is highly customizable.
- ServicePulse shows retry metrics to monitor the amount of retries being applied at any point in time.
}}

## Persistent errors

If errors cannot be resolved after a certain amount of automated retries, they are considered persistent errors. Persistent errors typically require some human action to resolve the root cause before retrying the failed message.

To avoid persistently failing messages from being retried infinitely, they can be moved dedicated error queues (many message queueing technologies use Dead-letter queues). This puts messages aside to prevent them from further clogging up the system ans where they can be manually inspected. However, this also means error queues need to be actively monitored.

Once the root cause of a persistent error has been resolved, the message can be moved back to the regular system queues to be processed again.

{{Note:
The Particular Platform simplifies handling of persistent errors:
- NServiceBus automatically forwards messages exceeding the configured automated retries to the error queue
- Messages moved to the error queue are enhanced with additional metadata to help with the root cause detection
- Both ServiceInsight and ServicePulse offer monitoring and inspection of failed messages and advanced retry functionalities
}}

## Best practices

- Don't catch exceptions in business logic. With recoverability mechanisms in place, exceptions thrown from the business logic will cause the message to be retried without additional error handling code in place.
- Configure and customize recoverability policies using the dedicated NServiceBus extension points to avoid leaking infrastructure related issues into business logic.
- Review the strategies in the [Consistency page](/architecture/consistency.md) for guidance on how to deal with consistency due to retrying the business logic.
- Don't build custom retry mechanisms. Building custom recoverability logic is risky and error-prone. The NServiceBus retry mechanisms are battle-proven and thoroughly tested.
