---
title: Saga Timeouts
summary: Call back into a saga after a defined period of time.
tags:
- Saga
related:
- samples/saga
- nservicebus/sagas
---

When working in a message-driven environment you cannot make assumptions about when the next message will arrive. While the connection-less nature of messaging prevents a system from consuming resources while waiting, there is usually an upper limit on how long from a business perspective to wait. At that point, some business-specific action should be taken, as shown:

snippet:saga-with-timeout

The `RequestTimeout<T>` method tells NServiceBus to send a message to the Timeout Manager which durably keeps time for us. The Timeout manager is enabled by default, so there is no configuration needed to get this up and running.

When the timeout timestamp is elapsed, the Timeout Manager sends a message back to the saga causing its Timeout method to be called with the same state message originally passed.

This timeout message will always be send no matter if any message has been send after requesting a timeout.

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

The state parameter provides a way to pass state to the Sagas timeout handle method. This is useful when you have many timeouts of the same "type" that will be active at the same time. One example of this would be to pass in some id that uniquely identifies the timeout eg: `.RequestTimeout(new OrderNoLongerEligibleForBonus{OrderId = "xyz"})`. With this state passed to the timeout handler it can now decrement the bonus correctly by looking up the order value from saga state using the provided id.


### Persistence

Some form of [Persistence](/nservicebus/persistence/) is required to store the timestamp and the state of a timeout.

WARNING: A durable persistence (i.e. NOT [InMemory](/nservicebus/persistence/in-memory.md)) should be chosen before moving to production.
