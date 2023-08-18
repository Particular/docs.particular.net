---
title: Recoverability
summary: Recoverability techniques for building resilient distributed systems
reviewed: 2023-07-18
---

In message-driven systems, failures in messages receivers do not impact the message senders. Messages are persistent, and even failed messages remain in the system until issues are resolved and they can be successfully processed. This increases system resiliency as failures in one component do not affect other components and no data is lost. Different strategies can be applied to deal with different types of failures:

## Transient errors

Transient failures are temporary, and are not caused by errors in business logic. They may be network issues, throttling, concurrency conflicts, etc. Resilient applications absorb such failures through [self-healing](https://learn.microsoft.com/en-us/azure/architecture/guide/design-principles/self-healing). Retries are a good solution for transient errors. There are two common retry patterns:

- Immediate retries: Many transient failures (e.g. concurrency errors) can be resolved by immediately retrying messages. However, immediate retries might not be the best approach when the root cause is due to overloaded or throttled resources, as they may exacerbate these problems.
- Delayed retries: Infrastructure related transient failures (e.g. network problems) might require more time to resolve. In this case it makes more sense to retry in the near future. Different delayed retry strategies can be used such as fixed intervals, [exponential backoff](https://en.wikipedia.org/wiki/Exponential_backoff), or exception-based values.

Note: The nature of retries implies that messages might be processed in a different order under some circumstances.

{{Note:
The Particular Platform simplifies handling of transient errors:

- NServiceBus has built-in support for immediate and delayed retries for all supported messaging technologies. The automated retries behavior is highly customizable.
- ServicePulse shows retry metrics for monitoring the amount of retries happening at any point in time.
}}

## Persistent errors

If errors cannot be resolved after a certain amount of automated retries, they are considered persistent errors. Persistent errors typically require manual intervention, to resolve the root cause, before retrying the failed messages.

To avoid persistently failing messages from being retried infinitely, they can be moved dedicated error queues (many message queueing technologies use [dead-letter queues](https://en.wikipedia.org/wiki/Dead_letter_queue)). This puts messages aside, to prevent them from clogging up the system, in a place where they can be manually inspected. However, this also means error queues need to be actively monitored.

Once the root cause of a persistent error has been resolved, messages can be moved back to their intended queues to be retried.

{{Note:
The Particular Platform simplifies handling of persistent errors:

- NServiceBus automatically forwards messages exceeding the configured number of automated retries to the error queue
- Email notifications for dead-letter messages
- Messages moved to the error queue are enhanced with additional metadata to help with root cause detection
- Both ServiceInsight and ServicePulse offer monitoring and inspection of failed messages and advanced retry functionality
}}

## Best practices

- Don't catch exceptions in business logic invoked by messages. With message-level recoverability mechanisms in place, exceptions thrown from business logic cause messages to be retried without additional error handling code (e.g. `try/catch` blocks, or [Polly](https://github.com/App-vNext/Polly)).
- Configure and customize recoverability policies using the dedicated NServiceBus extension points to avoid leaking infrastructure related issues into business logic.
- Review [consistency strategies](/architecture/consistency.md) for guidance on dealing with consistency while re-running business logic during retries.
- Don't build custom retry mechanisms. Building custom recoverability logic is risky and error-prone. The NServiceBus retry mechanisms are battle-proven and thoroughly tested.

[**Blog: I caught an exception. Now what? →**](https://particular.net/blog/but-all-my-errors-are-severe)

[**Video: An exception occurred... Try again →**](https://www.youtube.com/watch?v=gSQxtgw1Qz4)
