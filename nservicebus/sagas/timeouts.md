---
title: Saga Timeouts
summary: Call back into a saga after a defined period of time.
reviewed: 2024-04-08
component: Core
related:
- samples/saga
- nservicebus/sagas
---

Assumptions can not be made in a message-driven environment regarding the order of received messages and exactly when they will arrive. While the connection-less nature of messaging prevents a system from consuming resources while waiting, there is usually an upper limit to a waiting period that the business dictates.

The upper wait time is modeled in NServiceBus as a `Timeout`:

snippet: saga-with-timeout

`RequestTimeout<T>` sends a timeout message which is delivered after the specified delay or at the specified time.

If a saga does not request a timeout then its corresponding timeout method will never be invoked.

> [!WARNING]
> Don't assume that other messages haven't arrived in the meantime. If required, a Saga can store boolean flags in the SagaData and then check these flags to confirm that an incoming timeout message should be processed based on the current state.

include: non-null-task

## Timezones and Daylight Saving Time (DST)

A timeout may be requested specifying either a `DateTime` or `TimeSpan`. When specifying a `DateTime`, the `Kind` property must be set. If the timeout specifies a time of day, the calculation must take into account any change to or from DST. Timezone and DST conversion may be done using [`TimeZoneInfo.ConvertTime`](https://docs.microsoft.com/en-us/dotnet/api/system.timezoneinfo.converttime).

> [!NOTE]
> Timezone and DST information may change in the future, for timeouts that are already set. A saga containing business logic which is dependent on such changes must react to those changes appropriately.

## Requesting multiple timeouts

Multiple timeouts can be requested when processing a message. The individual timeouts can be different types and different timeout durations.

snippet: saga-multiple-timeouts

## Changing or revoking timeouts

After a timeout has been requested, it cannot be changed (i.e. rescheduled) or revoked (i.e. deleted or cancelled). Requesting a timeout with the same state again, but with a different duration or timestamp, will not revoke or ignore the original timeout. The original timeout and the subsequent timeout will both be processed by the saga.

When a saga handles a timeout, it may choose to ignore it, depending on how the saga state has changed since the timeout was requested.

## Completed Sagas

It is possible for a timeout to be queued after its saga has been completed. Because a timeout is tied to a specific saga instance it will be ignored once the saga instance is completed.

> [!NOTE]
> If a saga is created with a previously used saga identifier, the timeout mechanism will not treat it as the previous saga. As a result, any residual timeout messages for the previous, now completed, saga will not be processed by this new saga instance that shares the same identifier. The existing timeout message will be ignored.


## Timeout state

The state parameter provides a way to pass state to the Sagas timeout handle method. This is useful when many timeouts of the same "type" will be active at the same time. One example of this would be to pass in some ID that uniquely identifies the timeout eg: `.RequestTimeout(new OrderNoLongerEligibleForBonus{OrderId = "xyz"})`. With this state passed to the timeout handler it can now decrement the bonus correctly by looking up the order value from the saga state using the provided id.

### Using the incoming message as a timeout state

As a shortcut, an incoming saga message can be re-used as a timeout state by passing it to the `RequestTimeout` method and making the saga implement `IHandleTimeouts<TIncomingMessageType>`.


### Persistence

Some form of [Persistence](/persistence/) is required to store the timestamp and the state of a timeout.

> [!WARNING]
> A durable persistence (i.e. NOT [Non-Durable](/persistence/non-durable/) or [Learning Persistence](/persistence/learning/)) should be chosen before moving to production.

In order to learn how delayed delivery works in more detail, refer to the [Delayed Delivery - How it works](/nservicebus/messaging/delayed-delivery.md#how-it-works) section.
