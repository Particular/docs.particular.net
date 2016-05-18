---
title: Moving away from `IBus` in NServiceBus Version 6
summary: Describes how to send messages without the IBus
---

Starting with Versions 6 and above, the `IBus` interface has been deprecated.

The `IBus` interface was one of the key interfaces when using previous versions of NServiceBus. It provided access to many of the important operations like sending messages, subscribing to events, and manipulating headers. The `IBus` interface was injected into the container using dependency injection and was accessible in all the message handlers and other injected components in the container as well. 

There are several scenarios when using IBus:

## During Endpoint Creation

In previous versions of NServiceBus to start a new instance of an endpoint, either the `Bus` static class or the `IStartableBus` interface was used. In Version 6 these two concepts have been replaced. More information can be found in the [hosting](/nservicebus/hosting/) section of the documentation.

`BusConfiguration` has been deprecated. Use `EndpointConfiguration` instead to configure and start the endpoint. 

Instead of the `Bus` class, a new class called `Endpoint` helps to start the endpoint.

snippet: 5to6-endpoint-start-stop

Starting the endpoint, provides access to `IEndpointInstance` which can be used to send messages during endpoint start up instead.

## Sending messages outside message handlers

A common use of `IBus` is to invoke bus operations outside of message handlers, such as sending a message from an ASP.NET request or from a UI input. Instead of an `IBus` the `IEndpointInstance` offers all available bus operations outside the message processing pipeline. For example:

If the endpoint is hosted using NServiceBus.Host, use the [IWantToRunWhenEndpointStartsAndStops interface](/nservicebus/upgrades/host-6to7.md) as outlined in the NServiceBus.Host documentation.

## Sending messages inside message handlers

The public `IBus` injection property in message handlers can now be safely deleted.

The message handlers now includes an additional `IMessageHandlerContext` parameter, which provides the methods that used to be called from `IBus`. Use the `IMessageHandlerContext` to send and publish messages from within the message handler. 

snippet:5to6-bus-send-publish


## Accessing the CurrentMessageContext

Previously it was possible to access message parameters such `MessageId`, `ReplyToAddress` and the message headers via the `CurrentMessageContext` property on the `IBus` interface. These message properties are now available directly via the message handler context parameter.

snippet: 5to6-messagecontext


## Dependency Injection

In previous versions of NServiceBus the `IBus` interface was automatically registered in the IoC container. In Version 6, the new context aware interfaces namely the `IEndpointInstance`, `IMessageSession` or `IMessageHandlerContext` will not be automatically registered in the IoC container. 

The `IEndpointInstance` interface or the `IMessageSession` interface when using the NServiceBus.Host can be injected into the container.

## When not to inject IEndpointInstance

While the `IEndpointInstance` can be injected in the container in order to send messages outside the scope of message handlers, for example, in a MVC Controller class, do not use the injected `IEndpointInstance` from within message handlers. It is important to use the appropriate `IMessageHandlerContext` parameter from the message handler to send or publish messages.

If the `IEndpointInstance` interface is used inside a message handler to send or publish messages, then:

- those messages will not participate in the same transaction scope as that of the message handler. This could result in messages dispatched or events published via the `IEndpointInstance` interface even if the message handler resulted in an exception and the operation was rolled back.

- those messages will be part of the [batching operation](/nservicebus/messaging/batched-dispatch.md) and therefore will not have the benefits that Version 6 provides.

- those messages will not contain any important message header information that is available via the `IHandlerMessageContext` interface parameter, e.g., CorrelationId.  


## UnicastBus made internal


### Accessing the builder

When using the `IBuilder` interface outside the infrastructure of NServiceBus it was possible to use a hack by casting the `IBus` interface to `UnicastBus` and then accessing the `Builder` property like this:

    var builder = ((UnicastBus)bus).Builder

This is no longer supported. It is advised to, instead of using `IBuilder` directly, use dependency injection via the container of choice.


### Setting the host information

Control over `HostInformation` was previously done using `UnicastBus.HostInformation`. This is now done using the more explicit API to set the host identifier, see `busConfiguration.UniquelyIdentifyRunningInstance()`.


### Accessing ReadOnlySettings

`ReadOnlySettings` have been exposed on `UnicastBus.Settings`. The settings should only be accessed inside features, the pipeline and the start/stop infrastructure. Therefore accessing the settings over the `UnicastBus` is no longer supported.
