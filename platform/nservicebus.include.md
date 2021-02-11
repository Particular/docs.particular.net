NServiceBus is the heart of a distributed system and the Particular Service Platform. It helps create systems that are scalable, reliable, and flexible.

At its core, NServiceBus works by routing _messages_ between _endpoints_. [Messages](/nservicebus/concepts/glossary.md#message) are plain C# classes that contain meaningful data for the business process that is being modeled.

```csharp
public class ProcessOrder
{
    public int OrderId { get; set; }
}
```

[Endpoints](/nservicebus/concepts/glossary.md#endpoint) are logical entities that send and/or receive messages.

```csharp
// Sending endpoint
await endpoint.Send(new ProcessOrder { OrderId = 15 });

// Receiving endpoint
public class ProcessOrderHandler : IHandleMessages<ProcessOrder>
{
    public async Task Handle(ProcessOrder messsage, IMessageHandlerContext context)
    {
        // Do something with ProcessOrder message
    }
}
```

Endpoints can be running in different processes, on different machines, even at different times. NServiceBus makes sure that each message reaches its intended destination and is processed.

NServiceBus accomplishes this by providing an abstraction over [existing queuing technologies](/transports/). While it's possible to work directly with queuing systems, NServiceBus provides extra features to make applications more reliable and scalable.

## Reliable

NServiceBus offers different ways of ensuring information is not lost due to failures in a system. First, NServiceBus provides native transaction support for the underlying queuing technologies that support it, as well as its own transaction guarantees through the implementation of the [Outbox pattern](/nservicebus/outbox).

In other cases, NServiceBus has [built-in recoverability](/nservicebus/recoverability) that can automatically adapt to common failures in a system. For intermittent failures, such as network outages, messages can be retried at regular intervals. For more serious errors, messages are set aside in a separate error queue so that they can be investigated at a later time without impacting the overall performance of the system.

Watch this video for a better picture of how failures can lead to lost data, and how using NServiceBus keeps data safe.

<div style="display:block;width:80%;margin:50px auto;position:relative;height:0;padding-bottom:45%"><iframe src="https://www.youtube.com/embed/_Br9Me4tqPI" allow="accelerometer; encrypted-media; gyroscope; picture-in-picture" allowfullscreen frameborder="0" style="position:absolute;top:0;left:0;width:100%;height:100%;"></iframe></div>

NOTE: To see how NServiceBus prevents loss of data, adds failure recovery, and makes systems easier to extend, try the [NServiceBus Quick Start](/tutorials/quickstart/).

## Scalable

NServiceBus is designed to handle a large number of messages. Endpoints are configured for high performance by default, handling multiple messages in parallel. Depending on the workload, the number of messages handled concurrently can be [increased to improve message throughput](/nservicebus/operations/tuning.md).

In high volume scenarios, where there are more messages being produced than a single physical endpoint can handle, the logical endpoint can be [scaled out across multiple physical instances](/nservicebus/architecture/scaling.md) running on different machines, sharing the load. 

Each endpoint tracks [key performance metrics](/monitoring/metrics/definitions.md) that can be exposed as [Windows Performance Counters](/monitoring/metrics/performance-counters.md) or [collected into a central dashboard](/monitoring/metrics/in-servicepulse.md). [The monitoring demo](/tutorials/monitoring-demo/) demonstrates how to find performance bottlenecks and identify endpoints that are ready to scale out.

## Simple and testable

NServiceBus is designed with simplicity in mind. Message handlers don't need additional code to manage logging, exception handling, serialization, transactions, or the specifics of a queueing technology. This allows message handler code to focus on business logic.

Long-running business workflows can be modeled in NServiceBus using [sagas](/nservicebus/sagas/). A saga is a C# class that handles a number of different messages over time, persisting its state between each step in the workflow. NServiceBus makes sure that each saga instance only processes a single message at a time, keeping its internal state consistent. The [NServiceBus saga tutorials](/tutorials/nservicebus-sagas/) provide more details.

Message handlers and sagas can be [tested in isolation](/nservicebus/testing/). Simulating an incoming message is as simple as creating a new C# message object and passing it to the appropriate handler or saga. The framework includes a suite of testing tools that capture the behavior of message handlers and sagas under test, allowing assertions to be made about them.

## Flexible

NServiceBus endpoints can be hosted anywhere code can be executed, such as in a Windows Service, a Docker container, or in the cloud with Azure or AWS. Endpoints can be run on [a variety of platforms](/nservicebus/upgrades/supported-platforms.md).  

NServiceBus works with many different technology stacks, offering choices for [transport](/transports/) and [persistence](/persistence/). Out of the box, defaults are provided for [serialization](/nservicebus/serialization/), [dependency injection](/nservicebus/dependency-injection/), and [logging](/nservicebus/logging/). These defaults can be overridden if a specific technology is required. 

The NServiceBus message processing and dispatching pipeline is modular and extensible. [Message mutators](/nservicebus/pipeline/message-mutators.md) inject code into the pipeline to modify messages as they are being sent or received. More complex pipeline manipulation can be done with [behaviors](/nservicebus/pipeline/manipulate-with-behaviors.md). NServiceBus extensions can be packaged up as [features](/nservicebus/pipeline/features.md), which can add behaviors to the pipeline and create tasks that get run when an endpoint starts and stops. Many of the existing capabilities of NServiceBus are implemented as behaviors and features.



## Particular Service Platform

NServiceBus is designed to work with the rest of the Particular Service Platform. All messages are instrumented with additional [headers](/nservicebus/messaging/headers.md) detailing key information about the message and how it was processed. As each message is processed it is forwarded to an [audit queue](/nservicebus/operations/auditing.md), where it is picked up by [ServiceControl](/servicecontrol/). [ServiceInsight](/serviceinsight/) connects to a ServiceControl instance to provide powerful visualizations of a running NServiceBus system, making it easy to understand message flows and timing.

When a message fails to be processed, even after a number of retry strategies have been attempted, NServiceBus will forward the message to an [error queue](/nservicebus/recoverability/configure-error-handling.md) for manual investigation. Messages sent to the error queue are instrumented with headers containing details about the failure including a full exception stack trace. ServiceControl picks up messages from the error queue and makes them available in [ServicePulse](/servicepulse/). Once the root cause of the failure has been found and corrected, all messages caused by the same problem can be retried at once. 

Additionally, each endpoint can send [heartbeat](/monitoring/heartbeats/), [health check](/monitoring/custom-checks/), and [performance metrics](/monitoring/metrics/) through the platform for visualization in ServicePulse, making it easy to see which endpoints are offline, which are ready to scale out, and which require manual intervention.

The [real-time monitoring demo](https://particular.net/real-time-monitoring) provides the ability to experience the Service Platform in action. The [Platform Sample package](/platform/platform-sample-package.md) provides the ability to demonstrate the Service Platform from within any Visual Studio solution, without the need to install anything. 
