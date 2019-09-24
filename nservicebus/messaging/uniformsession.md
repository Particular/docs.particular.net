---
title: Uniform Session
summary: Uniform Session introduces a uniform message session across the endpoint.
reviewed: 2019-08-19
component: UniformSession
---

NServiceBus Version 6 introduced significant design changes on how to send messages. Here's a rough outline of the changes:

 * Clear separation of concerns between message operations outside the message handling pipeline (`IMessageSession`) vs. inside the message handling pipeline (`IMessageHandlerContext`). Operations inside the message handling pipeline enlist in the available transaction and participate in the batching operations of the transport. Operations outside the pipeline don't have those capabilities. The duality of the `IBus` interface was removed by introducing separate explicit interfaces for each usage scenario.
 * In previous versions, the `IBus` interface was automatically registered in the IOC container. In Version 6, the new context-aware interfaces are not automatically registered for dependency injection.

For more details on the Version 6 changes, refer to the [Moving away from IBus section](/nservicebus/upgrades/5to6/moving-away-from-ibus.md) of the NServiceBus Version 5 to 6 upgrade guide.

It is encouraged to embrace the design approach introduced in Version 6. Migrating to this new design in one step can be difficult though. This package reintroduces an opt-in approach for a uniform session approach that works seamlessly as a message session outside the pipeline and as a pipeline context inside the message handling pipeline. The message operations provided on the uniform session represent a common denominator but do not support more advanced scenarios like persistence session access, subscribe/unsubscribe operations or access to the incoming message's headers.


## Prerequisites for the uniform session functionality

Install the `NServiceBus.UniformSession` NuGet package available for endpoints using NServiceBus Version 6 and later.


## Usage

To enable the uniform session functionality, enable the package via the endpoint configuration:

snippet: enable-uniformsession

When enabled, `IUniformSession` is automatically registered in the container and can safely be injected into component hierarchies that are reused in different contexts such as WebApi and the message handler for example. The following snippet illustrates such a scenario:

snippet: uniformsession-usage

`IUniformSession` represents either an `IMessageSession` or `IMessageHandlerContext` depending on where it's used. Injected `IUniformSession` instances will automatically resolve to the correct session type based on the call hierarchy. To learn more about the session types, read the [Sending messages](/nservicebus/messaging/send-a-message.md) article.


## Safeguards

The uniform session must not be cached as the injected session's lifetime matches the current call context. To avoid potential message loss or runtime exceptions due to incorrect caching, the uniform session automatically prevents the following scenarios:

 * Cannot be cached from within the message handling pipeline and used outside the pipeline (to prevent message leakage or transaction interference).
 * Cannot be cached outside the message handling pipeline and used inside the pipeline (to prevent message duplication).
 * Cannot be cached over the lifetime of an endpoint, once an endpoint is stopped the corresponding session will be marked as unusable.


## Multi-endpoint hosting

For multiple endpoints hosted in the same process, a container per endpoint is required. If a single container is reused the bindings of the uniform session might be overwritten, and this could lead to unpredictable behavior.
