---
title: AWS Well-Architected Framework
summary:
reviewed: 2024-03-14
callsToAction: ['solution-architect', 'ADSD']
---

The [AWS Well-Architected](https://aws.amazon.com/architecture/well-architected) Framework helps cloud architects build secure, high-performing, resilient, and efficient infrastructure for a variety of applications and workloads. It’s built around six pillars: operational excellence, security, reliability, performance efficiency, cost optimization, and sustainability. The framework provides a consistent approach for customers and partners to evaluate architectures and implement scalable designs.

NServiceBus helps the organization achieve the six pillars of the AWS Well-architected Framework in a number of ways.

### Reliability

- NServiceBus handles unexpected failures and provides the [recoverability features](/nservicebus/recoverability/) required by self-healing systems.
- NServiceBus provides health metrics which can be monitored using [ServicePulse](/servicepulse/) and [OpenTelemetry](/nservicebus/operations/opentelemetry.md).

### Performance Efficiency

- NServiceBus endpoints can be scaled out easily using methods such as the built-in [competing consumers mechanism](/nservicebus/scaling.md#scaling-out-to-multiple-nodes-competing-consumers) and scaled up while tuning for [concurrency](/nservicebus/operations/tuning.md).
- NServiceBus is designed and tested for [high-performance and memory efficiency](https://particular.net/blog/pipeline-and-closure-allocations).
- [Monitoring](/monitoring/) allows observation of individual endpoint performance and identification of bottlenecks.

### Security

- NServiceBus provides data encryption in transit with [message encryption](/nservicebus/security/property-encryption.md).
- NServiceBus supports the [least privilege](/nservicebus/operations/installers.md#when-to-run-installers) approach during application deployment and runtime.

### Cost Optimization

- Costs may be optimized by [choosing the most appropriate technology](/architecture/aws/#technology-choices).

### Operational Excellence

- The Particular Service Platform [creates required infrastructure components](/nservicebus/operations/installers.md) using dedicated installation APIs or infrastructure scripting tools.
- ServicePulse provides [detailed insights](/servicepulse/) into the operational health of the system.
- NServiceBus supports [OpenTelemetry](/nservicebus/operations/opentelemetry.md) to integrate with 3rd-party monitoring and tracing tools.
- [Messaging](/nservicebus/messaging/) allows loosely coupled architectures with autonomous and independent services.
- NServiceBus APIs are designed for [unit testing](/nservicebus/testing/).

### Sustainability

- By abstracting the components of the system from the underlying [technology choices of AWS](/architecture/aws/#technology-choices), NServiceBus allows one to change to more efficient hardware and software offerings when they become available with minimal changes.

By integrating NServiceBus with AWS services according to well-architected principles, one can build a robust, scalable, and reliable distributed system that delivers optimal performance, security, and cost efficiency on the AWS cloud platform.
