---
title: Sagas Timeouts
summary: Call back into a saga after a defined period of time.
tags:
- Sagas
related:
- samples/saga
- nservicebus/sagas
---

When working in a message-driven environment, you cannot make assumptions about when the next message will arrive. While the connection-less nature of messaging prevents our system from bleeding expensive resources while waiting, there is usually an upper limit on how long from a business perspective to wait. At that point, some business-specific action should be taken, as shown:

<!-- import saga-with-timeout -->

The `RequestTimeout<T>` method on the base class tells NServiceBus to send a message to what is called a Timeout Manager(TM) which durably keeps time for us. In NServiceBus, each endpoint hosts a TM by default, so there is no configuration needed to get this up and running.

When the timeout timestamp is elapsed, the Timeout Manager sends a message back to the saga causing its Timeout method to be called with the same state message originally passed.

This timeout message will always be send no matter if any message has been send after requesting a timeout.


### Revoking timeouts

A timeout that has been scheduled cannot be revoked. This means that when the timeout timestamp has elapsed then this timeout message will be queued and then processed.

Reason for this is that a timeout is a regular message. The timeout message can already be in transit, queued, if there was the ability to revoke (delete) a timeout. Very often a state check is performed to see if the timeout is still applicable for processing.


### Completed Sagas

The timeout can potentially be queued after the saga is completed but because a timeout is tied to a specific saga instance it will be ignored as that saga instance will be gone.

NOTE: If a saga is recreated based on a similar message key then this is not the same saga instance and this old timeout message will not be processed by this new saga instance that shares the same key. The timeout message will be ignored.


### Timeout state

The state parameter provides a way to pass state to the timeout handler. This is useful when you have many timeouts of the same "type" that will be active at the same time. One example of this would be to pass in some id that uniquely identifies the timeout eg: `.RequestTimeout(new OrderNoLongerEligibleForBonus{OrderId = "xyz"})`. With this state passed to the timeout handler it can now decrement the bonus correctly by looking up the order value from saga state using the provided id.

**IMPORTANT** : Don't assume that other messages haven't arrived in the meantime. Please note how the timeout handler method first checks if the `Message2` message has arrived.