---
title: Hosting NServiceBus in-process
summary: Configuration API to get transnational one-way messaging
tags:
- Hosting
redirects:
 - nservicebus/hosting-nservicebus-in-your-own-process
---

NServiceBus comes with its own host process but you can host it in your own process.

Requiring just a reference to the NServiceBus NuGet package the configuration API can get you up and running with transactional one-way messaging in a snap.

## Initializing and configuring the bus

Initializing the bus requires a few lines of code depending on the features that you want to have enabled.

<!-- import MinimumConfiguration -->

You can now setup the appropriate configuration by calling the methods on the configuration object. For example, to disable second level retries feature:

<!-- import SecondLevelRetriesDisable --> 

## Configuration file

A lot of settings can also be managed via the `app.config` or `web.config` file. Often you configure what features you want via code and configure these features in the configuration file. Settings like connection strings, number of threads and routing destinations are configured in the configuration file.

## Creating the bus

To start processing message just get an instance of the bus and start it like shown below.

<!-- import BusDotCreate -->

NOTE: This sample only works when running in a console due to wrapping the bus instance in a using scope.


## Singleton

The bus instance should be created only once for the duration of your application. Creating a bus instance is very expensive. The instance should be reused in your application.


## Starting the bus

After the bus has been created it is required to start the bus if you want to process incoming messages or want to send/publish messages.

## Stopping the bus

The bus is stopped when the `IBus` or `IStartableBus` instance gets disposed. This will shutdown the processing of incoming messages.

After the bus is stopped it cannot be used anymore to send messages.

## Restarting the bus

It is not possible to restart a bus instance. Restarting an endpoint is basically stopping the current endpoint by disposing it and then creating a new startable bus instance and start it.

Another option would to restart the whole application. 


## Using and managing the bus

When the bus is started you can use the `IBus` instance and call any of its methods.

You must store this instance somewhere so that your application can manage the bus or want to use it when you want to send/publishe messages not in a message handler.

There are various ways in managing and storing the `IBus` instance.

* Global static
* Service locator
* Dependency injection

Please read (ibus-instance.md) for recommendations.


## Lifetime management

When you have called `Bus.Create(..)` you get a bus instance. You must manage its lifetime. The instance implements `System.IDisposable` which means that you should call `IDisposable.Dispose()`.

If you do not call this then the .net framework will call `Dispose()` when the .net garbage collector wants to clean up.

NOTE: This does not apply when using the NServiceBus.Host


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

While you can tell NServiceBus to which address to send a message using the API: `IBus.Send(toDestination, message);` NServiceBus enables you to keep your code decoupled from where endpoints are deployed on the network through the use of routing configuration. Include this configuration section:

```XML
<section name="UnicastBusConfig" type="NServiceBus.Config.UnicastBusConfig, NServiceBus.Core"/>

<!-- And then specify the configuration data like this: -->

<UnicastBusConfig>
<MessageEndpointMappings>
    <add Messages="MessageDLL" Endpoint="DestinationQueue@TargetMachine"/>
</MessageEndpointMappings>
</UnicastBusConfig>  
```

This tells NServiceBus that all messages in the MessageDLL assembly should be routed to the queue called DestinationQueue on the machine TargetMachine. You can send messages from that assembly, like this: `IBus.Send(messageFromMessageDLL)`;
