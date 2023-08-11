---
title: Stateful Business Workflows
summary:
reviewed: 2023-07-18
---

Meaningful business processes typically involve various distributed components that need to be invoked in a coordinated manner. The Particular Platform supports both orchestrated and choreographed workflow implementations.

Note: Choreography and orchestration are are not mutually exclusive. The patterns can be combined at different levels of business workflows.

## Choreography

Choreographed workflows are implemented without a central owner of the process but rather an implict flow of events between services. Services are highly decoupled due to the use of publish/subscribe. Published messages are called `events` because they describe a completed fact to the rest of the system with subscribers being able to subscribe and react to such events within their own domain. There is no central state of the workflow.

![](/serviceinsight/images/overview-sequence-diagram.png)
The image shows a choreographed event-driven workflow across multiple endpoints in ServiceInsight.

NServiceBus provides easy-to-use publish/subscribe APIs for every supported messaging technology. NServiceBus can automatically create and manage the necessary infrastructure like topics, subscriptions, and queues.

![](nsb-publish-subscribe.png)

### Challenges

* When implementing complex workflows, the message flows through the system can quickly become difficult to understand and track. To have a full picture of the message flow, the implementation of every service needs to be known. The absence of a central processor requires more effort to detect failed or stuck business processes. The [Particular Platform Monitoring tooling](/architecture/monitoring.md) helps to understand and monitor complex choreographed workflows.
* Putting too much data on events will quickly re-introduce coupling and impact overall system performance. Read more about about properly sizing event messages in the blog post [putting events on a diet](https://particular.net/blog/putting-your-events-on-a-diet).
* Complex workflows that require aggregation of multiple events require local state. Service-internal [orchestration using NServiceBus Sagas](#orchestration) can be used to easily aggregate multiple events and process timeouts to a component of an choreographed workflow.
* Maintaining consistent state across all choreography participants in case of failures can become challenging as the coordination and flow of compensating events can quickly multiply the complexity of a choreographed workflow. Use choreography for processes that can accept eventual consistency or use [orchestrated workflows](#orchestration) to handle stronger consistency requirements.


## Orchestration

Orchestrated workflows are managed by a central process that instructs relevant services in the necessary order, manages state, and handles failures. Orchestration can be useful in complex workflows that contain multiple conditionals, time-based triggers or stronger consistency requirements. Instead of events, message-based orchestration relies on `commands` that the orchestrator sends to a receiver known to process such a command.

NServiceBus is designed to handle long-running business processes in a robust and scalable way using the [Saga feature](/nservicebus/sagas/). NServiceBus Sagas are a convenient programming model to implement orchestrated, long-running business workflows or state machines using regular C#. Sagas handle recoverability, shared state, message correlation, timeouts, and more.

![](/serviceinsight/images/overview-sagaview.png)
The image shows an orchestrated workflow using an NServiceBus Saga, visualized in ServiceInsight using the SagaAudit plugin.

[Learn more about NServiceBus Sagas](/tutorials/nservicebus-sagas/1-saga-basics/)

[See it in action with a Saga demo project](/samples/saga/simple/)

Note: NServiceBus Sagas focus on providing a convenient and efficient way to manage message-based workflows as described by the [Process Manager pattern](https://www.enterpriseintegrationpatterns.com/patterns/messaging/ProcessManager.html). The [*Saga distributed transactions* pattern](https://learn.microsoft.com/en-us/azure/architecture/reference-architectures/saga/saga) is primarily concerened with managing long-lived transactions and ensuring consistency across multiple operations, especially in the presence of failures. The NServiceBus Saga feature can be used to implement the *Saga distributed transactions* pattern however.

### Challenges:

* Orchestrators have much higher coupling due to the dependency on the components that are being orchestrated for the workflow. This makes orchestrators prune to being affected by changes. Additionally, the workflow logic might require data of involved components  that need to be accessed by the orchestrator, further increasing coupling. Orchestration can be useful to handle technical processes within a bounded context / service domain, [choreography](#choreography) can be the better approach when aiming to decouple independent domains.
* Orchestrators are more difficult to scale as the whole workflow communicationo has to go through the central orchestrator, introducing a potential bottleneck. NServiceBus Sagas implement performance best practices that will optimize the integration with any supported state storage.
