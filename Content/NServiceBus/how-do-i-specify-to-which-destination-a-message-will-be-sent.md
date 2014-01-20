---
title: How to Specify to Which Destination a Message Is Sent?
summary: Configure the destination for message types in &lt;UnicastBusConfig>, under &lt;MessageEndpointMappings>.
originalUrl: http://www.particular.net/articles/how-do-i-specify-to-which-destination-a-message-will-be-sent
tags:
- Message Mapping
- Message destination
- Send
---

You configure the destination for message types in <unicastbusconfig>, under <messageendpointmappings>.

Add one of the following:


```XML
<UnicastBusConfig >
  <MessageEndpointMappings>
    <!--To register all message types defined in an assembly -->
    <add Assembly="assembly" Endpoint="queue@machinename" />
      
    <!-- To register all message types defined in an assembly with a specific namespace (it does not include sub namespaces): -->
    <add Assembly="assembly" Namespace="namespace" Endpoint="queue@machinename" />
      
    <!-- To register a specific type in an assembly: -->
    <add Assembly="assembly" Type="type fullname (http://msdn.microsoft.com/en-us/library/system.type.fullname.aspx)" Endpoint="queue@machinename" />
  </MessageEndpointMappings>
</UnicastBusConfig>
```

For more information, see the [PubSub sample](https://github.com/NServiceBus/NServiceBus/tree/master/Samples/PubSub) config file.

Destinations can be QueueName@ServerName, or just QueueName if the destination is the local machine.

You can also call the following, even though it is not recommended for application-level code:

```C#
Bus.Send( string destination, params IMessage[] msgs );
```

Even if it is possible to specify a message destination in code it is highly suggested to specify message destinations at application-configuration level to maintain a high level of flexibility.

