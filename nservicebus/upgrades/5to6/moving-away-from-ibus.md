---
title: Moving away from IBus in Version 6
reviewed: 2021-02-16
component: Core
summary: Describes how to send messages without the IBus interface
redirects:
 - nservicebus/upgrades/5to6-moving-away-from-ibus
isUpgradeGuide: true
upgradeGuideCoreVersions:
 - 5
 - 6
---

Starting with Version 6, the `IBus` interface has been deprecated.

The `IBus` interface was one of the key interfaces when using previous versions. It provided access to many of the operations like sending messages, subscribing to events, and manipulating headers. The `IBus` interface was available through dependency injection in message handlers, in any custom components that were registered in the container and through other mechanisms like the saga base class.

## Reasons for deprecating IBus

The `IBus` interface contained numerous methods and properties. However, not all of them are valid in all scenarios where an instance of `IBus` was exposed. For example, the methods `Reply` and `ForwardCurrentMessageTo` are always available on the `IBus` interface, but they are only relevant in the context of handling an incoming message. Using them in other scenarios throws an exception.

For users new to the API, the fact that `IBus` was available in message handlers using Dependency Injection wasn't obvious, especially when trying to send or publish messages from inside message handlers. Rather than using dependency injection, an `IMessageHandlerContext` parameter is now available in all the message handlers. This parameter exposes all of the appropriate actions to the message handler. Methods that aren't applicable are available making the API simpler.

All of the previous `Bus` methods now available via the `IMessageHandlerContext` parameter in the message handlers and the methods in the `IMessageSession` interface are fully async. However, the original method names are retained rather than adding the [async suffix](async-suffix.md) to make the upgrade easier.

## Migrating away from IBus

Some scenarios involving `IBus` include: endpoint creation, sending messages during endpoint startup, sending messages from within message handlers, using the injected `IBus` in custom components also registered in the container.

### During Endpoint Creation

In previous versions to start a new instance of an endpoint, either the `Bus` static class or the `IStartableBus` interface was used. In Version 6 these two concepts have been replaced. More information can be found in the [hosting](/nservicebus/hosting/) section of the documentation. Instead of the `Bus` class, a new class called `Endpoint` helps to start the endpoint.

`BusConfiguration` has been deprecated. Use `EndpointConfiguration` instead to configure and start the endpoint. Because `IBus` has been deprecated, the `BusConfiguration` class has been renamed to `EndpointConfiguration` to keep terminology consistent.

snippet: 5to6-endpoint-start-stop

Starting the endpoint provides access to `IMessageSession` or `IEndpointInstance` respectively which can be used to send messages during endpoint startup instead of using the `IBus` interface.

["Send-only" endpoints](/nservicebus/hosting/#self-hosting-send-only-hosting) were created by calling the `CreateSendOnly` method on the `Bus` class in previous versions. In version 6 there is no longer a separate method to create or start "Send-Only" endpoints. Configure the endpoint to be "Send-Only" with the `SendOnly` method on `EndpointConfiguration` and create/start it with the `Endpoint` class.

### Accessing the CurrentMessageContext

Previously it was possible to access message parameters, such as `MessageId`, `ReplyToAddress` and the message headers, via the `CurrentMessageContext` property on the `IBus` interface. These message properties are now available directly via the message handler context parameter.

snippet: 5to6-messagecontext

### Sending messages inside message handlers

Instances of `IBus` that were being injected into message handler classes by dependency injection can be safely deleted.

The message handler signature now includes an additional `IMessageHandlerContext` parameter, which provides the methods that used to be called from `IBus`. Use the `IMessageHandlerContext` to send and publish messages from within the message handler.

snippet: 5to6-bus-send-publish

### Sending messages outside message handlers

A common use of `IBus` is to invoke bus operations outside of the pipeline (e.g., in handlers, sagas and pipeline extensions), such as sending a message from an ASP.NET request or a client application. Instead of an `IBus,` the `IMessageSession` offers all available messaging operations outside the message processing pipeline. For example:

snippet: 5to6-endpoint-send-messages-outside-handlers

In this example, a message is being sent during the startup of an Endpoint. Hence the `IMessageSession` is available via the `IEndpointInstance` class from `Endpoint.Start`. Note that implicitly converting from `IEndpointInstance` to `IMessageSession` is optional but it is preferred as `IMessageSession` offers a more concise API for messaging interactions.

For other scenarios (i.e., not at startup) that need access to `IMessageSession` there are two ways to achieve this:

 1. Static property. In this scenario, during startup, the `IMessageSession` instance is assigned to a static property that is accessible from other components.
 1. Injecting via container. See "Dependency Injection" below.

If the endpoint is hosted using NServiceBus.Host, use the [`IWantToRunWhenEndpointStartsAndStops` interface](/nservicebus/upgrades/host-6to7.md).

## Dependency Injection

In previous versions, the `IBus` interface was automatically registered in the IOC container. In Version 6, the new context-aware interfaces, for example, `IEndpointInstance`, `IMessageSession` and `IMessageHandlerContext`, etc., are not automatically registered in [dependency injection](/nservicebus/dependency-injection/).

In Versions 5 and below, when a custom component was registered in the container, the custom component had access to the `IBus` instance via dependency injection.

WARNING: When upgrading such custom components to Version 6, it is **not safe** to replace all instances of `IBus` with `IMessageSession`. It depends on the usage of the `IBus` inside the custom component.

Scenario 1: If the custom component was sending messages using the injected `IBus` from outside of message handlers, for example, in an MVC Controller class, then it is safe to register the `IMessageSession` when self-hosting. This interface can then be used to send messages. See [Using NServiceBus with ASP.NET MVC](/samples/web/asp-mvc-application/) for an example how to send from an ASP.NET MVC controller.

Scenario 2: If the custom component is accessed from within message handlers then the `IMessageHandlerContext` parameter should be passed to the custom component instead of the `IMessageSession` interface to either send or publish messages.

Some of the dangers when using an `IMessageSession` interface inside a message handler to send or publish messages are:

 * Those messages do not participate in the same transaction scope as that of the message handler. This could result in messages dispatched or events published via the `IMessageSession` or `IEndpointInstance` interface even if the message handler resulted in an exception and the operation was rolled back.
 * Those messages will not be part of the [batching operation](/nservicebus/messaging/batched-dispatch.md).
 * Those messages will not contain any important message header information that is available via the `IMessageHandlerContext` interface parameter, e.g., CorrelationId.

### Accessing message handler context in the dependency hierarchy

The snippet below shows a handler with a dependency that accesses the `IBus` interface. The dependency is injected into a handler and used from within the handler.

snippet: 5to6-handler-with-dependency

Since message handler context operations are asynchronous, it is advised to refactor the dependency to no longer use the bus operations towards a design in which the dependency returns information to the caller that can be used to determine what bus operations are required. The following snippet illustrates that:

snippet: 5to6-handler-with-dependency-which-returns

By using this approach, the asynchronous APIs won't ripple through all the layers, and the dependency can remain synchronous if desired. If such a change is not feasible or desired the context has to be floated into the dependency by using method injection like shown below:

snippet: 5to6-handler-with-dependency-which-accesses-context

### Uniform session

If a step by step migration like shown above is not possible or takes a longer period, the uniform session package can be used to unify the message session with the pipeline context into a uniform session. For more information about the uniform session consult the [uniform session documentation](/nservicebus/messaging/uniformsession.md). It is advised to use this package for the transition phase until the new design approach started with NServiceBus v6 can be fully embraced.

## UnicastBus made internal

### Accessing the builder

When using the `IBuilder` interface outside the infrastructure of NServiceBus, it was possible to use a hack by casting the `IBus` interface to `UnicastBus` and then accessing the `Builder` property like this:

snippet: 5to6AccessBuilder

This is no longer supported. Instead of using `IBuilder` directly, it is advised to use [dependency injection](/nservicebus/dependency-injection/).

### Setting the host information

Control over `HostInformation` was previously done using `UnicastBus.HostInformation`. This is now done using a [more explicit API to set the host identifier](/nservicebus/hosting/override-hostid.md#overriding-the-host-identifier) using the endpoint configuration.

snippet: 5to6-Specifying-HostId-Using-Api

### Accessing ReadOnlySettings

Accessing `ReadOnlySettings` using `UnicastBus.Settings` is no longer supported as these settings should only be accessed inside features, the pipeline, and the start/stop infrastructure.
