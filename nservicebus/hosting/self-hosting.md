---
title: Hosting NServiceBus in-process
summary: Fluent configuration API to get transnational one-way messaging
tags: []
redirects:
 - nservicebus/hosting-nservicebus-in-your-own-process
---

Lighter-weight than BizTalk and more powerful than WCF, NServiceBus comes with its own host process and allows you to host it in your own process.

Requiring just a reference to the NServiceBus NuGet package the configuration API can get you up and running with transactional one-way messaging in a snap.

## Configuring the bus

In the `ApplicationStart` method of your Global.asax file in a web application, or in the `Main` method of your `Program` file for console or Windows Forms application, include the following initialization code:

<!-- import MinimumConfiguration -->

You can now setup the appropriate configuration by calling the methods on the configuration object. For example, to disable second level retries feature:

<!-- import SecondLevelRetriesDisable --> 

## Creating an instance of the bus and starting it

To start processing message just get an instance of the bus and start it like shown below.

<!-- import BusDotCreate -->

## Configuration files

In order for the code above to work you need to tell NServiceBus where to forward messages that fail processing.

Include the following section:

```XML
<section name="MessageForwardingInCaseOfFaultConfig" type="NServiceBus.Config.MessageForwardingInCaseOfFaultConfig, NServiceBus.Core" />

<!-- Specify the configuration data, as follows: -->

<MessageForwardingInCaseOfFaultConfig ErrorQueue="error"/>
```

If an exception is thrown during the processing of a message, NServiceBus automatically retries the message (as it might have failed due to something transient like a database deadlock). MaxRetries specifies the maximum number of times this is done before the message is moved to the ErrorQueue.

## Routing configuration

While you can tell NServiceBus to which address to send a message using the API: `Bus.Send(toDestination, message);` NServiceBus enables you to keep your code decoupled from where endpoints are deployed on the network through the use of routing configuration. Include this configuration section:

```XML
<section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"/>

<!-- And then specify the configuration data like this: -->

<UnicastBusConfig>
<MessageEndpointMappings>
    <add Messages="MessageDLL" Endpoint="DestinationQueue@TargetMachine"/>
</MessageEndpointMappings>
</UnicastBusConfig>  
```

This tells NServiceBus that all messages in the MessageDLL assembly should be routed to the queue called DestinationQueue on the machine TargetMachine. You can send messages from that assembly, like this: `Bus.Send(messageFromMessageDLL)`;
