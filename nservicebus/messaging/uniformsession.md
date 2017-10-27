---
title: Uniform Session between message session and pipeline
summary: Uniform Session allows to bridge the gap between the message session and the pipeline context and enables dependency injection.
reviewed: 2017-10-27
component: UniformSession
---

NServiceBus v6 and higher introduced a few design changes that how code that is using previous versions is structured. The [changes](/nservicebus/upgrades/5to6/moving-away-from-ibus.md) are:

- In previous versions, the `IBus` interface was automatically registered in the IOC container. In Version 6, the new context-aware interfaces, for example, `IEndpointInstance`, `IMessageSession` and `IMessageHandlerContext`, etc., are not automatically registered in dependency injection.
- In Versions 5 and below, when a custom component was registered in the container, the custom component had access to the `IBus` instance via dependency injection.
- Clear separation of concerns between message operations outside the message handling pipeline (`IEndpointInstance` and `IMessageSession`) vs. inside the message handling pipeline (`IMessageHandlerContext`). Operations inside the message handling pipeline enlist in the transaction available and participate in the batching operations of the transport. Operations outside the pipeline don't have those capabilities. The duality of the `IBus` interface was removed by introducing separate explicit interfaces for each usage scenario.
- Message operations are asynchronous by default

These changes lead to better designed and sufer user code because:

- Child containers no longer need to be mutated when a message arrives to rebind the current message handler context which simplifies container integration especially for immutable containers
- If a component needs state that is created by NServiceBus per message handling pipeline invocation the state has to be either
    - Explicitly carried into the components that need it and thus the component callstack will need to become asynchronous or
    - Managed in the ports and adapters of the system, i.ex. the message handler which is already asynchronous thus the component hierarchy can remain synchronous because the side effect incurring I/O operations are no longer the domain logic's concern.
- The domain logic becomes side-effect free and no longer produces messages but returns an explicit decision matrix which will be interpreted by the caller to create the necessary side effects

It is advised to embrace the design approach if possible. That being said for customers transitioning from previous versions the design decision caused some grief. This package reintroduces an opt-in approach for a uniform session approach that works seamlessly as a message session outside the pipeline and as a pipeline context inside the message handling pipeline. The message operations provided on the uniform session represent a common denominator but do not support more advanced scenarios like persistence session access.

## Prerequisites for the uniform session functionality

In NServiceBus Version 6 and above install the `NServiceBus.UniformSession` NuGet package.

## Usage

`IUniformSession` is automatically registered in the container and can safely be injected into component hierarchies that are reused in different contexts such as WebApi and the message handler for example. The following snippet illustrates such a scenario:

snippet: uniformsession-usage

Due to the duality of `IUniformSession` `ComponentReused` will behave different depending on where it is used.

- If used in the controller as shown above all messages will be immediately dispatched
- If used in the handler as shown above messages are [only dispatched](/nservicebus/messaging/batched-dispatch.md) when the handler completed

## Safeguards

The uniform session should not be cached and thus exceed the lifetime of the usage it was designed for. The uniform session automatically prevents the following scenarios:

- Cannot be cached from within the message handling pipeline and used outside the pipeline (to prevent message leakage or transaction interference)
- Cannot be cached outside the message handling pipeline and used inside the pipeline (to prevent message loss)
- Cannot be cached over the lifetime of an endpoint, once an endpoint is stopped the corresponding session will be marked as unusable

## Multi-endpoint hosting

For multiple endpoints hosted in the same process, a container per endpoint is required. If a single container is reused the bindings of the uniform session might be overwritten, and this could lead to unpredictable behavior.