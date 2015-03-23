---
title: Specifying a message destination
summary: Configure the destination for message types in &lt;UnicastBusConfig>, under &lt;MessageEndpointMappings>.
tags:
- Message Mapping
- Message destination
- Send
redirects:
- nservicebus/how-do-i-specify-to-which-destination-a-message-will-be-sent
---

You configure the destination for message types in `<unicastbusconfig>`, under `<messageendpointmappings>`.

Add one of the following:

```XML
<configSections>
    <section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"/>
</configSections>
<UnicastBusConfig >
  <MessageEndpointMappings>
    <!--To register all message types defined in an assembly -->
    <add Assembly="assembly" Endpoint="queue@machinename" />
      
    <!-- To register all message types defined in an assembly with a specific namespace (it does not include sub namespaces): -->
    <add Assembly="assembly" Namespace="namespace" Endpoint="queue@machinename" />
      
    <!-- To register a specific type in an assembly: -->
    <add Assembly="assembly" Type="type fullname (https://msdn.microsoft.com/en-us/library/system.type.fullname.aspx)" Endpoint="queue@machinename" />
  </MessageEndpointMappings>
</UnicastBusConfig>
```

For more information, see the [PubSub sample](/samples/pubsub) config file.

Destinations can be `QueueName@ServerName`, or just `QueueName` if the destination is the local machine.

You can also call the following, even though it is not recommended for application-level code:

```C#
Bus.Send( string destination, object message );
```

Even if it is possible to specify a message destination in code it is highly suggested to specify message destinations at application-configuration level to maintain a high level of flexibility.

