---
title: Specifying a message destination
summary: Configure the destination for message types in via endpoint mappings.
tags:
- Message Mapping
- Message destination
- Mapping
- Send
redirects:
- nservicebus/how-do-i-specify-to-which-destination-a-message-will-be-sent
related:
- samples/pubsub
---

Message mapping is a configurable convention that, based on some information about a message, that message can be routed to a specific endpoint without the sending code needing to be aware of the destination. A message mapping contains the following information.

## The target Endpoint

The target or destination endpoint can be of the form `QueueName@ServerName`, or just `QueueName` if the destination is the local machine.

## Resolving the Messages to map 

There are two, mutually exclusive approaches, to message resolution:

### Resolving with the AssemblyName

If this is defined then all types in that assembly will included in the initial set. This effectively uses [Assembly.Load(AssemblyName)](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) followed by [Assembly.GetTypes()](https://msdn.microsoft.com/en-us/library/system.reflection.assembly.gettypes.aspx).

Note: This value is the [AssemblyName](https://msdn.microsoft.com/en-us/library/k8xx4k69.aspx) not the file name.

#### Filtering

The type list from `AssemblyName` can be further filtered via one of the following:

 * **TypeFullName**: If `TypeFullName` is defined then only that type will be included in the mapping. This effectively calls [Assembly.GetType](https://msdn.microsoft.com/en-us/library/y0cd10tb.aspx) on the Assembly resolved via `AssemblyName`. 
 * **Namespace**: If `Namespace` is defined then only the types that have that namespace will be included in the mapping. It does not include sub namespaces.

Note that the xml configuration version of the above settings are slightly different:

 * `AssemblyName` is actually `Assembly` in xml.
 * `TypeFullName` is actually `Type` in xml.

### Resolving with Messages

If the `Messages` represents a valid Type (ie [Type.GetType](https://msdn.microsoft.com/en-us/library/w3f99sx1.aspx) returns a Type) then that type will be mapped to the target endpoint.

Otherwise it will be assumed `Messages` is an assembly name and all types in that assembly will be mapped to the target endpoint.  This effectively uses [Assembly.Load(AssemblyName)](https://msdn.microsoft.com/en-us/library/ky3942xh.aspx) followed by [Assembly.GetTypes()](https://msdn.microsoft.com/en-us/library/system.reflection.assembly.gettypes.aspx).


### Some example mappings 

To register all message types defined in an assembly:

 * `AssemblyName` = `YourMessagesAssemblyName` 
 * `Endpoint` = `queue@machinename`

To register all message types defined in an assembly with a specific namespace: 

 * `AssemblyName` = `YourMessagesAssemblyName` 
 * `Namespace` = `YourMessageNamesapce`
 * `Endpoint` = `queue@machinename`
  
To register a specific type in an assembly:

 * `AssemblyName` = `YourMessagesAssemblyName` 
 * `TypeFullName` = `YourMessageFullTypeName`
 * `Endpoint` = `queue@machinename`

### Conventions

Note that any types resolved as message from the above process are then further filtered by passing them through the [Message conventions](/nservicebus/messaging/messages-events-commands.md#conventions).

## Configuring Endpoint Mapping

Endpoint mapping can be configured in several ways

### Using app.config

You configure mapping in your app.config by adding `<unicastbusconfig>` and `<messageendpointmappings>` nodes.

<!-- import endpoint-mapping-appconfig -->

### Using a ConfigurationSource

#### The IConfigurationSource

<!-- import endpoint-mapping-configurationsource -->

#### Injecting the IConfigurationSource

<!-- import inject-endpoint-mapping-configuration-source -->

### Using a configuration provider

<!-- import endpoint-mapping-configurationprovider -->

## Bypassing mappings

You can also call the following, even though it is not recommended for application-level code:

```C#
Bus.Send(string destination, object message);
```

Even if it is possible to specify a message destination in code it is highly suggested to specify message destinations at application-configuration level to maintain a high level of flexibility.