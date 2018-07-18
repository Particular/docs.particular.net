---
title: "NServiceBus Sagas"
suppressRelated: true
reviewed: 2018-05-29
summary: An introduction to sagas and how to use them
hidden: true
---

In the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/) we created a functioning demo of a retail system that separates different concerns (like accepting an order, charging the credit card, and shipping the order) into different physical processes called endpoints that communicate by exchanging messages. We saw how the ability to publish events and create multiple subscribers lets us decouple our code so that each endpoint can focus on a single responsibility. Even in the face of errors and endpoint failures, we can fail gracefully and in some cases, even recover to the point where our users don't even know an error occurred.

However, message handlers are stateless and many business processes are not. For example, how do we ship an order when we have to wait for *both* `OrderPlaced` and `OrderBilled` to be processed first? How do we create a delay so that we can trigger an action at some point in the future? How do we make decisions based on what happened minutes, hours, or even years ago?

For these types of scenarios, you need a *stateful* message handler. Other ways to describe what is needed might include a message-driven state machine or an orchestration engine. In NServiceBus, we call these [**sagas**](/nservicebus/sagas/). In this tutorial, we'll build on the solution from the [NServiceBus step-by-step tutorial](/tutorials/nservicebus-step-by-step/) and learn to master NServiceBus sagas.

The tutorial is divided into four lessons, each of which can be accomplished in an hour or less — perfect for your lunch break.

* [**Lesson 1: Getting Started**](1-getting-started/) (20-25 minutes) - learn how to introduce sagas to complete the order shipping process.
* _Coming Soon - Lesson 2: Timeouts_ - learn how to delay processing of an order to implement the *buyer's remorse* pattern.
* _Coming Soon - Lesson 3: Third-party integration_ - learn how to integrate with external services.
* _Coming Soon - Lesson 4: Putting it all together_ - learn to master the effects of time by implementing a saga that never ends.

**Go to [Lesson 1: Getting started](1-getting-started/) to begin.**
