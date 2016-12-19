---
title: Saga Timeouts
summary: Call back into a saga after a defined period of time.
reviewed: 2016-08-22
component: Core
tags:
- Saga
related:
- samples/saga
- nservicebus/sagas
---

In a message-driven environment one can't make any assumptions regarding the order of received messages and when exactly they'll arrive. While the connection-less nature of messaging prevents a system from consuming resources while waiting, there is usually an upper limit to waiting period dictated by the business. 

The upper wait time is modeled in NServiceBus as a `Timeout`:

snippet:saga-with-timeout

NOTE: Timeouts feature is enabled by default. To turn it off it's necessary to disable the `TimeoutManager` feature.

After calling the `RequestTimeout<T>`, the timeout message will be persisted and scheduled to run after a specified delay or at specified time.

NOTE: If the saga does not request a timeout then the corresponding timeout method will never be invoked.

WARNING: Don't assume that other messages haven't arrived in the meantime. If required a Saga can store boolean flags in the SagaData and then check these flags to confirm a given timeout message should be processed based on the current sate.

include: non-null-task


## Revoking timeouts

A timeout that has been scheduled cannot be revoked. This means that when the timeout timestamp has elapsed then this timeout message will be queued and then processed.

The reason for this is that a timeout is a regular message. The timeout message can already be in transit, queued, if there was the ability to revoke (delete) a timeout. Very often a state check is performed to see if the timeout is still applicable for processing.

NOTE: Sending a timeout with the same data at the same time will result in that timeout to be send thus processed multiple times to the saga.


## Completed Sagas

The timeout can potentially be queued after the saga is completed but because a timeout is tied to a specific saga instance it will be ignored as that saga instance will be gone.

NOTE: If a saga is recreated based on a similar message key then this is not the same saga instance and this old timeout message will not be processed by this new saga instance that shares the same key. The timeout message will be ignored.


## Timeout state

The state parameter provides a way to pass state to the Sagas timeout handle method. This is useful when many timeouts of the same "type" that will be active at the same time. One example of this would be to pass in some ID that uniquely identifies the timeout eg: `.RequestTimeout(new OrderNoLongerEligibleForBonus{OrderId = "xyz"})`. With this state passed to the timeout handler it can now decrement the bonus correctly by looking up the order value from saga state using the provided id.

### Using the incoming message as timeout state

As a shortcut the incoming message can be re-used as timeout state by passing it to the `RequestTimeout` method and making the saga implement `IHandleTimeouts<TIncommingMessageType>`.


### Persistence

Some form of [Persistence](/nservicebus/persistence/) is required to store the timestamp and the state of a timeout.

WARNING: A durable persistence (i.e. NOT [InMemory](/nservicebus/persistence/in-memory.md)) should be chosen before moving to production.

In order to learn how delayed delivery works in more detail, refer to the [Delayed Delivery - How it works](/nservicebus/messaging/delayed-delivery.md#how-it-works) section.
