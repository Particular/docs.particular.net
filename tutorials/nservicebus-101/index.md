---
title: "NServiceBus 101: Messaging Basics"
suppressRelated: true
reviewed: 2017-01-26
---

partial: nsb101-intro-paragraph

 * In [Lesson 1: Getting started](lesson-1/) (10-15 minutes) you will learn how to set up your development environment and create your very first messaging endpoint.
 * In [Lesson 2: Sending a command](lesson-2/) (15-20 minutes) you will learn how to define messages and message handlers. In the exercise, you'll send and process a `PlaceOrder` command within the same endpoint.
 * In [Lesson 3: Multiple endpoints](lesson-3/) (15-20 minutes) you will learn how to create multiple endpoints and send messages between them. In the exercise, you'll update your solution to send the `PlaceOrder` command to a separate **Sales** endpoint.
 * In [Lesson 4: Publishing events](lesson-4/) (25-30 minutes) you will learn how to publish events to multiple subscribers, and about the benefits of decoupling business processes using the Publish/Subscribe pattern. In the exercise, you'll modify your **Sales** endpoint to publish an `OrderPlaced` event to two new subscribers: a **Billing** endpoint for processing payments, and a **Shipping** endpoint to manage shipping items to customers.
 * In [Lesson 5: Retrying errors](lesson-5/) (25-30 minutes) you will learn how to deal with exceptions and take advantage of automatic retries. In the exercise, you'll throw throw an exception in a message handler and observe NServiceBus attempting to process the message several times before finally giving up on it and moving it aside to an error queue. Then you'll fix the exception and use the tools in the Particular Service Platform to replay the failed message through the original message handler successfully.

 When you've completed all the exercises, your solution will look like this:

 ![Completed Solution Diagram](lesson-4/diagram.png)

Go to [**Lesson 1: Getting started**](lesson-1/) to begin.
