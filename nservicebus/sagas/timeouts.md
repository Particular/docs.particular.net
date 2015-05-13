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

When the time is up, the Timeout Manager sends a message back to the saga causing its Timeout method to be called with the same state message originally passed.

### Timeout state

The state parameter provides a way to pass state to the timeout handler. This is useful when you have many timeouts of the same "type" that will be active at the same time. One example of this would be to pass in some id that uniquely identifies the timeout eg: `.RequestTimeout(new OrderNoLongerEligibleForBonus{OrderId = "xyz"})`. With this state passed to the timeout handler it can now decrement the bonus correctly by looking up the order value from saga state using the provided id.

**IMPORTANT** : Don't assume that other messages haven't arrived in the meantime. Please note how the timeout handler method first checks if the `Message2` message has arrived.