---
title: "NServiceBus Step-by-step"
suppressRelated: true
reviewed: 2024-05-13
summary: Learn how to use NServiceBus quickly with this step-by-step tutorial, including the architectural concepts behind it.
redirects:
- tutorials/intro-to-nservicebus
- tutorials/nservicebus-101
- samples/step-by-step
- nservicebus/nservicebus-step-by-step-guide
- nservicebus/nservicebus-step-by-step-publish-subscribe-communication-code-first
extensions:
- !!tutorial
  nextText: "Next: Getting started"
  nextUrl: tutorials/nservicebus-step-by-step/1-getting-started/
---

include: nsb101-intro-paragraph

- **[Part 1: Getting started](1-getting-started/)** (10-15 minutes) - Set up your development environment and create your very first messaging endpoint.

- **[Part 2: Sending a command](2-sending-a-command/)** (15-20 minutes) - Define messages and message handlers, and send your first message.

- **[Part 3: Multiple endpoints](3-multiple-endpoints/)** (15-20 minutes) - Create multiple endpoints and send messages between them.

- **[Part 4: Publishing events](4-publishing-events/)** (25-30 minutes) - Learn about the publish/subscribe pattern, how to publish events to multiple subscribers, and about the benefits of using this pattern to decouple business processes.

- **[Part 5: Retrying errors](5-retrying-errors/)** (25-30 minutes) - Use the Particular Service Platform tools to gracefully recover from exceptions in your code, allowing you to build systems that are resistant to failure.

When you've completed all the exercises, your solution will look like this:

![Completed Solution Diagram](4-publishing-events/diagram.svg)
