---
title: "NServiceBus Sagas"
suppressRelated: true
reviewed: 2020-11-24
summary: An introduction to sagas and how to use them
---

In the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/) we created a functioning demo of a retail system that separates different concerns (like accepting an order, charging the credit card, and shipping the order) into different physical processes called endpoints that communicate by exchanging messages. We saw how the ability to publish events and create multiple subscribers lets us decouple our code so that each endpoint can focus on a single responsibility. Even in the face of errors and endpoint failures, we can fail gracefully and in some cases, even recover to the point where our users don't even know an error occurred.

However, message handlers are stateless and many business processes are not. For example, how do we ship an order when we have to wait for *both* `OrderPlaced` and `OrderBilled` to be processed first? How do we create a delay so that we can trigger an action at some point in the future? How do we make decisions based on what happened minutes, hours, or even years ago?

For these types of scenarios, you need a *stateful* message handler. Other ways to describe what is needed might include a message-driven state machine or an orchestration engine. In NServiceBus, we call these [**sagas**](/nservicebus/sagas/). In this tutorial, we'll build on the solution from the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/) and learn to master NServiceBus sagas.

The lessons make the most sense when tackled in order, but it's not required. Each tutorial can be accomplished in an hour or less.

1. [**Saga basics**](1-saga-basics/) (20-25 minutes) - learn how to introduce sagas to complete the order shipping process.
1. [**Timeouts**](2-timeouts/) (20-25 minutes) - learn how to delay processing of an order to implement the *buyer's remorse* pattern.
1. [**Third-party integration**](3-integration/) (20-25 minutes) - learn how to execute multiple steps in a workflow and use timeouts to trigger compensating actions.

**Go to [Saga basics](1-saga-basics/) to begin.**
