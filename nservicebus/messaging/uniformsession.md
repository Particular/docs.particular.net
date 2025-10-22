---
title: Uniform Session
summary: Uniform Session introduces a uniform message session across the endpoint.
reviewed: 2025-10-22
component: UniformSession
---

`Uniform session` is an opt-in feature that provides a common interface for a message session for business services that are used both inside and outside of message handlers.

> [!IMPORTANT]
> The uniform session message operations do not support more advanced scenarios like persistence session access, subscribe/unsubscribe operations or access to the incoming message's headers.

## Prerequisites for the uniform session functionality

Install the `NServiceBus.UniformSession` NuGet package.

## Usage

To enable the uniform session functionality, enable the package via the endpoint configuration:

snippet: enable-uniformsession

When enabled, `IUniformSession` is automatically registered in the container and can safely be injected into component hierarchies that are reused in different contexts such as WebApi and the message handler, for example. The following snippet illustrates such a scenario:

snippet: uniformsession-usage

`IUniformSession` represents either an `IMessageSession` or `IMessageHandlerContext` depending on where it's used. Injected `IUniformSession` instances will automatically resolve to the correct session type based on the call hierarchy. To learn more about the session types, read the [Sending messages](/nservicebus/messaging/send-a-message.md) article.

## Safeguards

The uniform session must not be cached as the injected session's lifetime matches the current call context. To avoid potential message loss or runtime exceptions due to incorrect caching, the uniform session automatically prevents the following scenarios:

 * Cannot be cached from within the message handling pipeline and used outside the pipeline (to prevent message leakage or transaction interference).
 * Cannot be cached outside the message handling pipeline and used inside the pipeline (to prevent message duplication).
 * Cannot be cached over the lifetime of an endpoint; once an endpoint is stopped the corresponding session will be marked as unusable.

## Multi-endpoint hosting

For multiple endpoints hosted in the same process, a container per endpoint is required. If a single container is reused, the bindings of the uniform session might be overwritten, and this could lead to unpredictable behavior.
