---
title: Moving away from `IBus` in NServiceBus Version 6
summary: Describes how to send messages without the IBus
---

Starting with Version 6, the `IBus` interface has been deprecated.

The `IBus` interface was one of the key interfaces when using previous versions of NServiceBus. It provided access to many of the operations like sending messages, subscribing to events, and manipulating headers. The `IBus` interface was available through dependency injection in message handlers, any custom components that were registered in the container and through other mechanisms like the saga base class.

Some scenarios involving `IBus` include: endpoint creation, sending messages during endpoint startup, sending messages from within message handlers, using the injected `IBus` in custom components also registered in the container. 

## During Endpoint Creation

In previous versions of NServiceBus to start a new instance of an endpoint, either the `Bus` static class or the `IStartableBus` interface was used. In Version 6 these two concepts have been replaced. More information can be found in the [hosting](/nservicebus/hosting/) section of the documentation. Instead of the `Bus` class, a new class called `Endpoint` helps to start the endpoint.

`BusConfiguration` has been deprecated. Use `EndpointConfiguration` instead to configure and start the endpoint. 

snippet: 5to6-endpoint-start-stop

Starting the endpoint, provides access to `IEndpointInstance` which can be used to send messages during endpoint start up instead of using the `IBus` interface.

## Sending messages outside message handlers

A common use of `IBus` is to invoke bus operations outside of message handlers, such as sending a message from an ASP.NET request or from a client application. Instead of an `IBus` the `IEndpointInstance` offers all available bus operations outside the message processing pipeline. For example:

snippet:5to6-endpoint-send-messages-outside-handlers

If the endpoint is hosted using NServiceBus.Host, use the [IWantToRunWhenEndpointStartsAndStops interface](/nservicebus/upgrades/host-6to7.md) as outlined in the NServiceBus.Host documentation.

## Sending messages inside message handlers

Instances of `IBus` that were being injected into message handler classes by the IoC container can be safely deleted. 

The message handler signature now includes an additional `IMessageHandlerContext` parameter, which provides the methods that used to be called from `IBus`. Use the `IMessageHandlerContext` to send and publish messages from within the message handler. 

snippet:5to6-bus-send-publish


## Accessing the CurrentMessageContext

Previously it was possible to access message parameters such `MessageId`, `ReplyToAddress` and the message headers via the `CurrentMessageContext` property on the `IBus` interface. These message properties are now available directly via the message handler context parameter.

snippet: 5to6-messagecontext


## Dependency Injection

In previous versions of NServiceBus the `IBus` interface was automatically registered in the IoC container. In Version 6, the new context aware interfaces, for example, the `IEndpointInstance`, `IMessageSession` and `IMessageHandlerContext`, etc., will not be automatically registered in the IoC container.  

In NServiceBus Versions 5 and below, when a custom component was registered in the container, the custom component had access to the `IBus` instance via dependency injection.

WARNING: When upgrading such custom components to Version 6, it is **not safe** to simply replace all instances of `IBus` with `IEndpointInstance`. It depends on the usage of the `IBus` inside the custom component. 

Scenario 1: If the custom component was sending messages using the injected `IBus` from outside of message handlers, for example, in an MVC Controller class, then it is safe to register the `IEndpointInstance` when self hosting. This interface can then be used to send messages. See [Injecting the Bus into ASP.NET MVC Controller](/samples/web/asp-mvc-injecting-bus/) for an example of this.

Scenario 2: If the custom component is accessed from within message handlers then the `IMessageHandlerContext` parameter should be passed to the custom component instead of the `IEndpointInstance` interface to either send or publish messages. 

Some of the dangers when using an `IEndpointInstance` interface inside a message handler to send or publish messages are:
- those messages will not participate in the same transaction scope as that of the message handler. This could result in messages dispatched or events published via the `IEndpointInstance` interface even if the message handler resulted in an exception and the operation was rolled back.
- those messages will not be part of the [batching operation](/nservicebus/messaging/batched-dispatch.md).
- those messages will not contain any important message header information that is available via the `IHandlerMessageContext` interface parameter, e.g., CorrelationId.  


## UnicastBus made internal


### Accessing the builder

When using the `IBuilder` interface outside the infrastructure of NServiceBus it was possible to use a hack by casting the `IBus` interface to `UnicastBus` and then accessing the `Builder` property like this:

    var builder = ((UnicastBus)bus).Builder

This is no longer supported. It is advised to, instead of using `IBuilder` directly, use dependency injection via the [container](/nservicebus/containers/) of choice.


### Setting the host information

Control over `HostInformation` was previously done using `UnicastBus.HostInformation`. This is now done using a [more explicit API to set the host identifier](/nservicebus/hosting/override-hostid.md#how-do-i-override-an-endpoint-host-identifier) using the endpoint configuration. 

snippet: 5to6-Specifying-HostId-Using-Api

### Accessing ReadOnlySettings

Accessing `ReadOnlySettings` using `UnicastBus.Settings` is no longer supported as these settings should only be accessed inside features, the pipeline and the start/stop infrastructure. 
