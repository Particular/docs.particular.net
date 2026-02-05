---
title: Event-driven architecture style on AWS
summary: Describes event-driven architecture including the components, challenges, and technology options for AWS
reviewed: 2026-01-13
callsToAction: ['solution-architect', 'ADSD']
---

The AWS Architecture Center describes [event-driven architecture](https://aws.amazon.com/event-driven-architecture/) as using events to trigger and communicate between decoupled services.

The Particular Service Platform implements the [publish/subscribe model](/nservicebus/messaging/publish-subscribe/), with each [NServiceBus endpoint](/nservicebus/endpoints/) acting as a publisher (event producer) and/or subscriber (endpoint consumer).

![A depiction of an event-driven architecture using AWS and NServiceBus](/architecture/aws/images/aws-event-driven-architecture.png)

## Components

- **NServiceBus publisher (event producer)**

    Publishes events with business meaning in a reliable fire-and-forget style and has no knowledge of subscribers (there may be none).
- **NServiceBus subscriber (event consumer)**

    Subscribes to a specific event type and reacts to it. A subscriber may also be an event publisher, since processing an event may lead to publishing more events.
- **Amazon Simple Queue Service (SQS)**

    The messaging service that brings together publisher and subscriber creating a layer of abstraction between the two.

## Challenges

- **Event ordering**

    Subscribers cannot rely on the order they receive published events, which may be affected by many factors such as concurrency, scaling, retries, partitioning, etc. Events and subscribers should be designed so that they [do not rely on strict ordering to execute business processes](https://particular.net/blog/you-dont-need-ordered-delivery).
- **Event payload size**

    Putting too much data on messages couples publishers and subscribers, defeating one of the main benefits of messaging in the first place. Bloated event contracts indicate sub-optimal service boundaries, perhaps drawn along technical constraints rather than business rules, or [data distribution](/architecture/data-distribution.md) over messaging. Well-designed events are [lightweight contracts](https://particular.net/blog/putting-your-events-on-a-diet), focusing on sharing IDs rather than data.

## Technology choices

In event-driven architectures, components are decoupled, allowing choice of the most suitable [compute](/architecture/aws/compute.md) and [data store](./data-stores.md) options for a specific component or set of components.

An event-driven approach requires support for the publish/subscribe model. NServiceBus supports this model for [AWS SQS](./messaging.md), independent of the underlying service capabilities.
