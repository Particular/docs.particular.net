---
title: Observability
summary: Observability in distributed systems including root-cause analysis, health/performance monitoring, OpenTelemetry guidance, and Particular Platform tools.
reviewed: 2026-01-02
callsToAction: ['solution-architect']
---

Observability is the idea that one should be able to understand the system’s behavior by investigating telemetry information, without the need to change any code or configuration, or investigate any system-specific data stores which may not be accessible and/or contain sensitive information. By collecting telemetry data from systems, one is able to query and analyze that information, with the end-goal of extracting actionable insights that can help improve one's systems.

Observability can help address multiple concerns in a system:

## Root-cause analysis

Distributed systems are complex system landscapes where multiple components interact with each other through different mechanisms in different time frames (asynchronous and synchronous communication). This makes it a lot harder to pinpoint the root cause of failures, as multiple components and infrastructure may be involved. It’s quite common in message-based systems, that the cause of a failure observed in one component, is caused by another component further upstream. By collecting traces and logs from systems, one can gather insights into what’s happening in the system and make it easier to pinpoint the root cause of failures.

## Health monitoring

Is my system healthy and operable? The ability to answer this question becomes more complex in distributed systems as the system is composed of multiple components that operate autonomously. This requires more attention to be paid to the health and operability of individual components. By emitting telemetry (in the form of metrics or otherwise) from all components that make up the systems, one can understand the health of the system and gain insights into individual components that may be struggling.

## Performance monitoring

One component of performance monitoring that is harder to understand in production environments, is latency. Latency can severely affect the performance of the overall system, even when individual components have been performance-tested during development. Emitting telemetry, specifically traces, can help one gain insight into the entire request execution, across all the relevant components in the system.

## OpenTelemetry

With the increasing need for observability, multiple standards started emerging in the industry to standardize the collection of telemetry data from distributed systems, including Google’s [OpenCensus standard](https://opencensus.io/) and [OpenTracing](https://opentracing.io/). In an effort to standardize in a cross-platform, cross-runtime, cross-language manner, the [Cloud Native Computing Foundation](https://www.cncf.io/) set up the [OpenTelemetry project](https://opentelemetry.io), effectively merging the OpenCensus and OpenTracing standards into one. Its goal is to help standardize how one instruments, generates, collects, and exports telemetry data from applications, by providing multiple tools and SDK for multiple languages and platforms. Most observability vendors have adopted the OpenTelemetry standard, enabling customers to store their telemetry data in a vendor-agnostic format.

Video: [Message processing failed, but what’s the root cause?](https://www.youtube.com/watch?v=U8Aame0akl4&pp=ygUSbGFpbGEgYm91Z3JpYSBvc2xv)

## The Particular Service Platform

The Particular Service Platform offers multiple capabilities that allow teams to observe the message flows that are occurring in the system. ServicePulse provides multiple views that offer insights into message and [saga flows](/architecture/workflows.md#orchestration-implementing-orchestrated-workflows) based on audited messages. These message-based conversations are visualized using [flow diagrams](/servicepulse/message-details.md#messages-with-audited-conversation-data-flow-diagram), [sequence diagrams](/servicepulse/message-details.md#messages-with-audited-conversation-data-sequence-diagram), and [saga views](/servicepulse/message-details.md#messages-with-audited-conversation-data-saga-diagram), helping teams understand how endpoints interact and how messages move through the system. These visualizations help teams understand the components that are sending and receiving messages across the system.
