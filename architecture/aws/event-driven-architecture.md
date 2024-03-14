---
title: Event-driven architecture style on AWS
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'ADSD']
---

The AWS Architecture Center describes the[ event-driven architecture](https://aws.amazon.com/event-driven-architecture/) as using events to trigger and communicate between decoupled services.

The Particular Service Platform implements[ pub/sub](https://docs.particular.net/nservicebus/messaging/publish-subscribe/), with each[ NServiceBus endpoint](https://docs.particular.net/nservicebus/endpoints/) acting as a publisher (event producer) and/or subscriber (endpoint consumer).

### Components

- NServiceBus publisher (event producer): Publishes events with business meaning in a reliable fire-and-forget style and has no knowledge of subscribers (there may be none).
- NServiceBus subscriber (event consumer): Subscribed to a specific event type and reacts to it. A subscriber may also be an event publisher, since processing an event may lead to publishing more events.
- Simple Queue Service: The messaging service that brings together publisher and subscriber without explicitly referencing or depending on each other.

### Challenges

- Events order: Subscribers cannot rely on the order they receive published events, which may be affected by many factors such as concurrency, scaling, retries, partitioning, etc. Events and subscribers should be designed so that they[ do not rely on strict ordering to execute business processes](https://particular.net/blog/you-dont-need-ordered-delivery).
- Event data: Putting too much data on messages couples publishers and subscribers, defeating one of the main benefits of messaging in the first place. Bloated event contracts indicate sub-optimal service boundaries, perhaps drawn along technical constraints rather than business rules, or[ data distribution](https://docs.particular.net/nservicebus/concepts/data-distribution) over messaging. Well designed events are[ lightweight contracts](https://particular.net/blog/putting-your-events-on-a-diet), focusing on sharing IDs rather than data.

### Technology choices

In event-driven architectures, components are decoupled, allowing choice of the most suitable[ compute](https://docs.particular.net/architecture/aws/compute) and[ data store](https://docs.particular.net/architecture/aws/data-stores) options for a specific component or set of components.

An event-driven approach requires support for the publish-subscribe model. NServiceBus supports the publish-subscribe model for[ AWS SQS](https://docs.particular.net/architecture/AWS/messaging), independent of the underlying service capabilities.