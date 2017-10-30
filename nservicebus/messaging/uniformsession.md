---
title: Uniform Session
summary: Uniform Session introduces a uniform message session across the endpoint.
reviewed: 2017-10-27
component: UniformSession
---

NServiceBus Version 6 introduced significant design changes on how to send messages. Here's a rough outline of the changes:

- Clear separation of concerns between message operations outside the message handling pipeline (`IMessageSession`) vs. inside the message handling pipeline (`IMessageHandlerContext`). Operations inside the message handling pipeline enlist in the transaction available and participate in the batching operations of the transport. Operations outside the pipeline don't have those capabilities. The duality of the `IBus` interface was removed by introducing separate explicit interfaces for each usage scenario.
- In previous versions, the `IBus` interface was automatically registered in the IOC container. In Version 6, the new context-aware interfaces are not automatically registered in dependency injection.

For more details on the Version 6 changes, refer to the [Moving away from IBus section](/nservicebus/upgrades/5to6/moving-away-from-ibus.md) of the NServiceBus Version 5 to 6 upgrade guide.


These changes lead to better designed and safer user code because:

- Child containers no longer need to be mutated when a message arrives to rebind the current message handler context which simplifies container integration especially for immutable containers
- If a component needs state that is created by NServiceBus per message handling pipeline invocation the state has to be either
    - Explicitly carried into the components that need it and thus the component callstack will need to become asynchronous or
    - Managed in the ports and adapters of the system, i.ex. the message handler which is already asynchronous thus the component hierarchy can remain synchronous because the side effect incurring I/O operations are no longer the domain logic's concern.
- The domain logic becomes side-effect free and no longer produces messages but returns an explicit decision matrix which will be interpreted by the caller to create the necessary side effects

It is advised to embrace the design approach if possible. That being said for customers transitioning from previous versions the design decision caused some grief. This package reintroduces an opt-in approach for a uniform session approach that works seamlessly as a message session outside the pipeline and as a pipeline context inside the message handling pipeline. The message operations provided on the uniform session represent a common denominator but do not support more advanced scenarios like persistence session access.


## Prerequisites for the uniform session functionality

Install the `NServiceBus.UniformSession` NuGet package available for endpoints using NServiceBus Version 6 and later.


## Usage

`IUniformSession` is automatically registered in the container and can safely be injected into component hierarchies that are reused in different contexts such as WebApi and the message handler for example. The following snippet illustrates such a scenario:

snippet: uniformsession-usage

Due to the duality of `IUniformSession` (it can represent either `IMessageSession` or `IMessageHandlerContext`), `ReusedComponent` will behave different depending on where it is used.

- If used in the MVC controller as shown above all messages will be immediately dispatched
- If used in the handler as shown above messages are [only dispatched when the handler completed](/nservicebus/messaging/batched-dispatch.md)


## Safeguards

The uniform session must not be cached as the injected session's lifetime matches the current call context. To avoid potential message loss or runtime exceptions due to incorrect caching, the uniform session automatically prevents the following scenarios:

- Cannot be cached from within the message handling pipeline and used outside the pipeline (to prevent message leakage or transaction interference)
- Cannot be cached outside the message handling pipeline and used inside the pipeline (to prevent message loss)
- Cannot be cached over the lifetime of an endpoint, once an endpoint is stopped the corresponding session will be marked as unusable


## Multi-endpoint hosting

For multiple endpoints hosted in the same process, a container per endpoint is required. If a single container is reused the bindings of the uniform session might be overwritten, and this could lead to unpredictable behavior.
