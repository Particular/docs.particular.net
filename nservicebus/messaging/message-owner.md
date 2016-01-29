---
title: Specifying a message owner
summary: Configure the owning endpoint for message types.
tags:
- Message Mapping
- Message destination
- Mapping
redirects:
- nservicebus/how-do-i-specify-to-which-destination-a-message-will-be-sent
- nservicebus/messaging/specify-message-destination
related:
- samples/pubsub
- nservicebus/messaging/routing
---

NServiceBus had the concept of an "Owning Endpoint" for any given message type. In V6 this has been superseded by the more flexible [routing model](/nservicebus/messaging/routing.md). The legacy configuration in form of `MessageEndpointMappings` configuration section is still fully respected by the V6 endpoints in order to provide a a smoother transition experience. That legacy form might not be supported in the future releases so we encourage to use the new routing model in new V6 endpoints.

Message mapping is a configurable convention that, based on some information about a message, that message can be routed to a specific endpoint without the sending code needing to be aware of the destination. A message mapping contains the following information.


## How "Owning Endpoint" manifests


### When Sending a message

When a message is sent (and no destination is specified) then that message type be routed to the owning endpoint.


### At startup on a subscriber

When a endpoint is started the [Auto Subscribe](/nservicebus/messaging/message-owner.md) functionality will use the owning endpoint to determine which message, and on which endpoint, should be subscribed to.


## Message Mapping

The owning endpoint for any given message type can be configured using message mappings.

An endpoint may have multiple message mappings where each mapping consists of two pieces of information:


## 1. The Owning "Endpoint"

The owning endpoint (sometimes referred to as "target" or "destination" endpoint) can be of the form `QueueName@ServerName`, or just `QueueName` if the destination is the local machine.


## 2. Resolving the Messages Types to map

This allows the mapping to know which message types to include in the mapping.

There are two, mutually exclusive approaches, to message resolution:


### Resolving with the Assembly

If this is defined then all types in that assembly will included in the initial set. This effectively uses [Assembly.Load(Assembly)](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) followed by [Assembly.GetTypes()](https://msdn.microsoft.com/en-us/library/system.reflection.assembly.gettypes.aspx).

Note: This value is the [AssemblyName](https://msdn.microsoft.com/en-us/library/k8xx4k69.aspx) not the file name.


#### Filtering

The type list from `Assembly` can be further filtered via one of the following:

 * **Type**: If `Type` is defined then only that type will be included in the mapping. This effectively calls [Assembly.GetType](https://msdn.microsoft.com/en-us/library/y0cd10tb.aspx) on the Assembly resolved via `Assembly`.
 * **Namespace**: If `Namespace` is defined then only the types that have that namespace will be included in the mapping. It does not include sub namespaces.

{{Note: The xml configuration version (`app.config`) the the code based API differ slightly.

In `app.config` the attributes are `Assembly` and `Type`.

In the code API the properties are `AssemblyName` and `TypeFullName`.
}}

### Resolving with Messages

If the `Messages` represents a valid Type (i.e. [Type.GetType](https://msdn.microsoft.com/en-us/library/w3f99sx1.aspx) returns a Type) then that type will be mapped to the target endpoint.

Otherwise it will be assumed `Messages` is an assembly name and all types in that assembly will be mapped to the target endpoint. This effectively uses [Assembly.Load(Assembly)](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) followed by [Assembly.GetTypes()](https://msdn.microsoft.com/en-us/library/system.reflection.assembly.gettypes.aspx).

WARNING: Since `Messages` is ambiguous in its usage the `Assembly`, `Type` and `Namespace` attributes are the recommended approach for mapping types. The `Messages` attribute is still supported for backwards comparability.


## Some example mappings

To register all message types defined in an assembly:

 * `Assembly` = `YourMessagesAssemblyName`
 * `Endpoint` = `queue@machinename`

To register all message types defined in an assembly with a specific namespace:

 * `Assembly` = `YourMessagesAssemblyName`
 * `Namespace` = `YourMessageNamespace`
 * `Endpoint` = `queue@machinename`
 
To register a specific type in an assembly:

 * `Assembly` = `YourMessagesAssemblyName`
 * `Type` = `YourMessageFullTypeName`
 * `Endpoint` = `queue@machinename`


## Conventions

Note that any types resolved as message from the above process are then further filtered by passing them through the [Message conventions](/nservicebus/messaging/messages-events-commands.md#defining-messages-conventions).


## Configuring Endpoint Mapping

Endpoint mapping can be configured in several ways


### Using app.config

You configure mapping in your app.config by adding `<UnicastBusConfig>` and `<MessageEndpointMappings>` nodes.

snippet:endpoint-mapping-appconfig

### Using a ConfigurationSource


#### The IConfigurationSource

snippet:endpoint-mapping-configurationsource


#### Injecting the IConfigurationSource

snippet:inject-endpoint-mapping-configuration-source


### Using a configuration provider

snippet:endpoint-mapping-configurationprovider


## Bypassing the owning endpoint

You can also call the following, even though it is not recommended for application-level code:

```C#
Bus.Send(string destination, object message);
```

Even if it is possible to specify a message destination in code it is highly suggested to specify message destinations at application-configuration level to maintain a high level of flexibility.
