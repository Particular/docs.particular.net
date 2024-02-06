---
title: Event stream processing
summary: Combining the Particular Service Platform with an event stream processor (Apache Kafka, Azure Event Hubs, or Amazon Kinesis)
callsToAction:
  - interest-apache-kafka
  - interest-azure-event-hubs
  - interest-amazon-kinesis
reviewed: 2024-02-06
related:
  - samples/azure-functions/service-bus-kafka
---

_Event stream processing_ is a practice of taking action on a series of datapoints that originate from a system that continuously creates data.

There are great tools that facilitate event streaming including:

- [Apache Kafka](https://kafka.apache.org/)
- [Azure Event Hubs](https://azure.microsoft.com/en-us/services/event-hubs/)
- [Amazon Kinesis](https://aws.amazon.com/kinesis/).

Although these tools and the Particular Service Platform both use the term _event_, [an event stream is not a queue](https://particular.net/blog/lets-talk-about-kafka).

Event streaming tools are great for situations where a large amount of data needs to be processed looking for patterns or changes in state. Individual events are not important on their own, and only by processing many of them can interesting events be extracted.

When something interesting is detected, that is a business event, and _it is time to use a message queue_. Message queues excel at handling important events, where each individual event is important. Business events should be successfully processed once, and only once, and should be retried until they are successfully processed.

To facilitate this, message stream processors that detect interesting patterns can turn them into NServiceBus messages and events.

```mermaid
graph LR
  Producer(Event Stream<br/>Producer) --> Log
  subgraph Event Stream
    Log
  end
  Log --> Processor(Event Stream<br/>Processor)
  Processor -- Send Message --> NServiceBus(NServiceBus<br/>Endpoint)
```

An example of this can be seen in the [Using NServiceBus and Kafka in Azure Functions sample](/samples/azure-functions/service-bus-kafka/).
