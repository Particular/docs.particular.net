---
title: Bridge performance
summary: Performance considerations and scaling the bridge
reviewed: 2022-05-04
component: Bridge
---
This page describes performance considerations when using the `NServiceBus.Transport.Bridge` component and how to scale out. There are several reasons to scale out, such as to achieve higher message throughput, or to provide high availability.

## Performance tuning

The bridge moves messages using [the same default concurrency as NServiceBus endpoints](/nservicebus/operations/tuning.md#configuring-concurrency-limit) which is `max(Number of logical processors, 2)`.

Customize the concurrency level as follows:

snippet: custom-concurrency

## Scaling out

The performance of regular endpoints is impacted by many factors, including databases containing business data, or third party services like an SMTP server or a web service. In contrast, the performance of the bridge is affected mainly by resources like CPU, network and disk. This makes it an easier decision to scale out the bridge.

### Competing consumers

As with [regular NServiceBus endpoints](/nservicebus/architecture/scaling.md#scaling-out-to-multiple-nodes-competing-consumers), the easiest way to scale out is with [brokered transports](/transports/types.md#broker-transports), as those can make use of the *[competing consumer pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/CompetingConsumers.html)*. This is done by deploying multiple instances of the bridge, all with the same configuration. Every instance will start processing messages from the same queue. When a message must be processed by the bridge, any of the instances could potentially process it.

The bridge accesses messages using NServiceBus transport packages. Each transport on its own will try to ensure that only one instance will actually process the message.

NOTE: MSMQ is not a brokered transport and cannot benefit from the competing consumer pattern.

### Splitting up the bridge

It is possible to use different logical instances of the bridge. Each logical instance can be configured so that it mimics only certain endpoints. In the diagram below, one bridge only takes care of Endpoint B and Endpoint C, while the other takes care of Endpoint A and Endpoint D. The result (which is not visible in the diagram) is that Endpoint B can still send messages to Endpoint D, but those will be processed by Bridge 2. When Endpoint A sends a message to Endpoint C, those messages will be processed by Bridge 1.

This can be useful when resources such as a CPU or disk are fully utilized and the competing consumers pattern isn't the appropriate solution.

NOTE: It may be practical to split up logical endpoints to increase throughput for *specific messages*. For the bridge this benefit is lost, as each endpoint that is mimicked by the bridge has its own message processing pipeline, separate from other mimicked endpoints.



```mermaid
graph LR
  Br2[Bridge 2<br /><sup>Endpoint A<br />Endpoint D</sup>]
  A[Endpoint A] --- Br2
  B[Endpoint B]
  Br2 --- D[Endpoint D]
  Br1[Bridge 1<br /><sup>Endpoint B<br />Endpoint C</sup>] --- C[Endpoint C]
  B --- Br1
  subgraph msmq
    A
    B
  end
  subgraph sql
    C
    D
  end
```
