---
title: Hosting NServiceBus in-process
summary: Manually configuring the application for acquiring the IBus instance
tags:
- Hosting
- Initialization
- Dependencies
- Configuration
- Routing
redirects:
 - nservicebus/hosting-nservicebus-in-your-own-process
---

Lighter-weight than BizTalk and more powerful than WCF, NServiceBus comes with its own host process and allows you to host it in your own process.

The bus instance will be injected automatically into any message-handler or message-mutator class that needs it and has it defined as a public property. This is achieved by simply adding the Nuget package for NServiceBus or NserviceBus host creating transactional messaging in a snap. 

All that's left for you to do is add an IBus property to the message-handler classes you define. For Sagas even that is already defined for you.

The ***NServiceBus-Host*** for self hosted NServiceBus server applications automatically configures the bus instance for duplex (two way) messaging, while for web apps the primary ***NServiceBus*** package automatically configures the bus  instance injection for  **one-way** messaging.  


## Configuring the bus in a web app

The following instructions manually configure the bus instance injection for **one-way** messaging in regular web-apps and **duplex** messaging for SingalR web-apps, which stay alive until closed. 

In the `ApplicationStart` method of your Global.asax file in a web application, or in the `Main` method of your `Program` file for console or Windows Forms application, include the following initialization code:

<!-- import MinimumConfiguration -->

You can now setup the appropriate configuration by calling the methods on the configuration object. For example, to disable second level retries feature:

<!-- import SecondLevelRetriesDisable --> 

## Creating an instance of the bus and starting it
To start processing messages you need to instantiate the bus instance. 

The bus instance should be created only once for the duration of your application. Creating a bus instance is very expensive. The instance should be reused in your application.

### Instantiating the bus

To instantiate the bus use one of the following options:
1. Automatic Bus injuction through package
2. Create an ad-hoc bus instance in your console application
3. Create a static global instance - for web applications


#### Automatic Bus injuction through configuration
Simply use the Nuget packages - either NServiceBus, or NServiceBus-Host (which includes NServiceBus).
The bus instance will be instantiated automatically.


#### Ad-hoc instance - for Console application
IMPORTANT: This should not be used if you have your application configured for automatic IBus instantiation

* In the Main() method of your console application 'Program' class add the following: 

<!-- import BusDotCreate -->


#### Global static instance - For non NServicebus-Host applications (web apps) 
For sharing the bus instance accross the project classes without the need for injection use the following method:

Note: For a console application this is NOT recommended, since it causes tight coupling and creates testing difficulties. 

In your project add the following class: 

In the Global.asax file, or in a public class that is created when your program starts and can be accessed from all objects, add a property 

     public static IBus Bus { get; set; }


In the `ApplicationStart` method of your Global.asax file in a web application, or in the `Main` method of your `Program` file for console or Windows Forms application, include the following

    // Initializing the bus instance:
    Global.Bus = NServiceBus.Create(busConfiguration).Start(); 

### Starting the bus, restarting, stopping and disposing it

#### Starting the bus instance

After the bus has been created it is required to start the bus if you want to process incoming messages or want to send/publish messages.

Use the following code in the same place you instantiated the bus: 

    Bus.Start();


#### Stoping the bus instance and disposing of it

The bus is stopped when the `IBus` or `IStartableBus` instance gets disposed. This will shutdown the processing of incoming message. 

Most containers including Autofac (The default installed with the NServiceBus Nuget packages) automatically dispose all objects instantiated in them, when the container itself is being closed, so there is no need to write any code to stop or close the bus instance. 

If you are using any other type of injection container other than Autofac, please consult that container's documentation and check if an explicit instruction to dispose the bus instance is needed. 


After the bus is stopped it cannot be used anymore to send messages.


#### Restarting the bus instance

It is not possible to restart a bus instance. Restarting an endpoint is basically stopping the current endpoint by disposing it and then creating a new startable bus instance and starting it, or simply restaring the whole application.


## Configuration files

In order for the code above to work you need to tell NServiceBus where to forward messages that fail processing.

Include the following section in the web.config if you are developing a web application or in the App.config otherwise:

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
