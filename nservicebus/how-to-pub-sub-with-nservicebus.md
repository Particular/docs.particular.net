---
title: How to Publish/Subscribe to a Message
summary: Publish and subscribe to messages using NServiceBus, automatically and manually.
tags: []
---

## How to publish a message?

```C#
var message = new MyMessage
              { 
                 Prop1 = v1; 
                 Prop2 = v2; 
              };
Bus.Publish(message);
```

 OR instantiate and publish all at once:

```C#
Bus.Publish<IMyMessage>( m => { 
                                m.Prop1 = v1; 
                                m.Prop2 = v2; 
                              });
```

## How to subscribe to a message?

To manually subscribe and unsubscribe from a message:

```C#
Bus.Subscribe<MyMessage>();    
Bus.Unsubscribe<MyMessage>();
```

To subscribe to a message, you must have a `UnicastBusConfig` entry, as follows:

```XML
<UnicastBusConfig>
  <MessageEndpointMappings>
    <!-- To register all message types defined in an assembly: -->
    <add Assembly="assembly" Endpoint="queue@machinename" />
    
    <!-- To register all message types defined in an assembly with a specific namespace (it does not include sub namespaces): -->
    <add Assembly="assembly" Namespace="namespace" Endpoint="queue@machinename" />
    
    <!-- To register a specific type in an assembly: -->
    <add Assembly="assembly" Type="type fullname" Endpoint="queue@machinename" />
  </MessageEndpointMappings>
</UnicastBusConfig>
```

And a`UnicastBusConfig` config section.  

```XML
<configuration>
  <configSections>
    <section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core" />
```

When subscribing to a message, you will probably have a [message handler](how-do-i-handle-a-message.md) for it. If you do, and have the `UnicastBusConfig` section mentioned above, you do not have to write `Bus.Subscribe`, as NServiceBus invokes it automatically for you.

## Disabling auto-subscription

You can also choose to **not** have the infrastructure automatically subscribe using the fluent API

### Verison 4

<!-- import DisableAutoSubscribeV4 -->

### Version 5  

<!-- import DisableAutoSubscribeV5 -->
