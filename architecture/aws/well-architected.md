## AWS Well-Architected

The [AWS Well-Architected](https://aws.amazon.com/architecture/well-architected) Framework helps cloud architects build secure, high-performing, resilient, and efficient infrastructure for a variety of applications and workloads. It’s built around six pillars: operational excellence, security, reliability, performance efficiency, cost optimization, and sustainability. The framework provides a consistent approach for customers and partners to evaluate architectures and implement scalable designs.

NServiceBus helps you achieve the six pillars of the AWS Well-architected Framework in a number of ways.

### Reliability

- NServiceBus handles unexpected failures and provides the[ recoverability features](https://docs.particular.net/nservicebus/recoverability/) required by self-healing systems.
- NServiceBus provides health metrics which can be monitored using[ ServicePulse](https://docs.particular.net/servicepulse/) and[ OpenTelemetry](https://docs.particular.net/nservicebus/operations/opentelemetry).

### Performance Efficiency

- NServiceBus endpoints can be scaled out easily using methods such as the built-in[ competing consumers mechanism](https://docs.particular.net/nservicebus/scaling#scaling-out-to-multiple-nodes-competing-consumers) and scaled up while tuning for[ concurrency](https://docs.particular.net/nservicebus/operations/tuning).
- NServiceBus is designed and tested for[ high-performance and memory efficiency](https://particular.net/blog/pipeline-and-closure-allocations).
- [Monitoring](https://docs.particular.net/monitoring/) allows observation of individual endpoint performance and identification of bottlenecks.

### Security

- NServiceBus provides data encryption in transit with[ message encryption](https://docs.particular.net/nservicebus/security/property-encryption).
- NServiceBus supports the[ least privilege](https://docs.particular.net/nservicebus/operations/installers#when-to-run-installers) approach during application deployment and runtime.

### Cost Optimization

- Costs may be optimized by[ choosing the most appropriate technology](https://docs.particular.net/architecture/aws/#technology-choices).

### Operational Excellence

- The Particular Service Platform[ creates required infrastructure components](https://docs.particular.net/nservicebus/operations/installers) using dedicated installation APIs or infrastructure scripting tools.
- ServicePulse provides[ detailed insights](https://docs.particular.net/servicepulse/) into the operational health of the system.
- NServiceBus supports[ OpenTelemetry](https://docs.particular.net/nservicebus/operations/opentelemetry) to integrate with 3rd-party monitoring and tracing tools.
- [Messaging](https://docs.particular.net/nservicebus/messaging/) allows loosely coupled architectures with autonomous and independent services.
- NServiceBus APIs are designed for[ unit testing](https://docs.particular.net/nservicebus/testing/).

### Sustainability

- By abstracting you from the underlying [technology choices of AWS](https://docs.particular.net/architecture/aws/#technology-choices), NServiceBus allows you to change to more efficient hardware and software offerings when they become available with minimal changes.

By integrating NServiceBus with AWS services according to well-architected principles, you can build a robust, scalable, and reliable distributed system that delivers optimal performance, security, and cost efficiency on the AWS cloud platform.
