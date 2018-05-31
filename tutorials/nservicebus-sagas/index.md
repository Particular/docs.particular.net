---
title: "NServiceBus sagas"
suppressRelated: true
reviewed: 2018-05-29
summary: to-be-defined.
hidden: true
---

In the [messaging basics tutorial](/tutorials/intro-to-nservicebus/) we created a functioning demo of a retail system that separates different concerns (like accepting an order, charging the credit card, and shipping the order) into different physical processes called endpoints that communicate by exchanging messages. We saw how the ability to publish events and create multiple subscribers lets us decouple our code so that each endpoint can focus on a single responsibility. Even in the face of errors and endpoint failures, we can fail gracefully and in some cases, even recover to the point where our users don't even know an error occurred.

However, message handlers are stateless and many business processes are not. For example, how do we ship an order when we have to wait for *both* `OrderPlaced` and `OrderBilled` to be processed first? How do we create a delay so that we can trigger an action at some point in the future? How do we make decisions based on what happened minutes, hours, or even years ago?

For these types of scenarios, you need a *stateful* message handler. Other ways to describe what is needed might include a message-driven state machine or an orchestration engine. In NServiceBus, we call these [**sagas**](/nservicebus/sagas/). In this tutorial, we'll build on the solution from the [messaging basics tutorial](/tutorials/intro-to-nservicebus/) and learn to master NServiceBus sagas.

Note that sagas require some form of persistence to run. In the tutorial, we use the [learning persistence](/persistence/learning/), but in a live system you should use one of the production persistences, such as [SQL persistence](/persistence/sql/).

The tutorial is divided into four lessons, each of which can be accomplished in an hour or less — perfect for your lunch break.

* Lesson 1: Our first saga (//TODO: define time 1-2 years) - learn how to introduce sagas to complete an order
* `Coming soon` Lesson 2: Timeouts (//TODO: define time 1-2 years) - learn how to delay processing an order to allow for "buyer's remorse"
* `Coming soon` Lesson 3: Third-party integration (//TODO: define time 1-2 years) - learn how to integrate with external services
* `Coming soon` Lesson 4: Putting it all together (//TODO: define time 1-2 years) - implement a new saga that will never end

When you've completed all the exercises, your solution will look like this:

![img](solution image with the flow, endpoints etc.)

//TODO: decide which image to include, and if we should include one till we only have lesson 1

**Let's get started with [Lesson 1](1-getting-started/).**
