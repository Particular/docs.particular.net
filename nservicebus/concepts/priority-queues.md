---
title: Priority Queues
summary: Why priority messages require more than just a queue
reviewed: 2025-10-08
---

An endpoint is a collection of code and configuration that processes a specific set of message types. The thing is, some of the incoming messages might be more important than others. As queues generally work according to the First In, First Out (FIFO) principle, those important messages could end up waiting until all earlier messages are processed.

A common approach to this situation is to let important messages 'jump the queue', bypassing all other messages. Some messaging technologies support this through a special type of queue called a "priority queue".

Another approach that works with regular queues is to subdivide the endpoint, routing those higher-priority messages to a different queue that a separate host processes.

Before reaching for either of these solutions, take some time to think through some additional considerations:

## Different business process

Often, the need to process messages quicker and have the need for priority messages comes from a business standpoint. It is often more than message priority. A different type of process could exist for the priority message. A different kind of fee applies, or even larger differences in business logic exist than for regular messages. Depending on the domain, there can be many deviations from the regular business processes.

The result is that the design of a component is about more than message priority. There could be differences in code or even in design. At the very least, it's usually more than just the message's priority. The design and code should likely be separated completely from the regular business process.

## Different operational considerations

Another aspect of priority messages can be a different Service Level Agreement (SLA) than with regular messages. The SLA might be breached if the message isn't processed in a specific amount of time, which is far more strict than with regular messages.

Other decisions could be using a different datastore, concurrency mode, or caching strategy. Or even decide on a different type of hardware. It should also be possible to scale out differently compared to the regular business process. Have different metrics, monitoring, etc.

## Low coupling

Endpoints should be designed to be autonomous and be able to do their work without having knowledge of how other endpoints work. If an endpoint is required to decide which messages should have a higher priority, it requires knowledge of what the receiving endpoint(s) are responsible for. This requires a tighter coupling than might be desirable. If the Single Responsibility Principle is valued, it is recommended not to introduce more responsibilities into a sender than necessary. As the result would be that if the logic changes to decide if a message has priority, the sender needs to be changed as well.

## Possible solutions

A solution is to place the responsibility of deciding which message is a priority message with the sender. As mentioned, this introduces tight coupling between sender and receivers.

Another option could be using publish/subscribe, where both the regular and high-priority endpoints subscribe to a publisher endpoint.

In a [series of blog posts about priority queues](https://bloggingabout.net/2020/07/16/priority-queues-why-you-dont-need-them/), Dennis van der Stelt goes into more detail about both the problem and possible solutions and their trade-offs. He provides a third solution that is not discussed here, which involves a different view on responsibilities and deployment using concepts from the 4+1 architectural view model.
