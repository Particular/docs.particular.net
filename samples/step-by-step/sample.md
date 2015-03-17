---
title: Step by Step Guide
summary: Get started with NServiceBus
tags: []
redirects:
- nservicebus/nservicebus-step-by-step-guide
---

In this tutorial we are going to create a very simple ordering system that will send messages from a client to a server. The ordering system includes three projects: Client, Server, and Messages, to complete this tasks we will execute the following steps:

1.  [Creating the Client project](#Creating-the-Client-project)
2.  [Creating the Messages project](#Creating-the-Messages-Project)
3.  [Creating the Server project](#Creating-the-Server-Project)
4.  [Sending the order](#Sending-the-order)
5.  [Running the solution](#Running-the-solution)

### Creating the Client project

Let's start by creating a `Client` project that will send order requests to a NServiceBus endpoint.

Open Visual Studio as administrator, create a new 'Class Library' Project name it `Ordering.Client`, and name the solution `Ordering`.

![](001-new-solution.png)

![](package-manager-console.png)

We now need to add references the NServiceBus assemblies and the quickest and easiest way to do that is to use NuGet Package Manager Console.

Open the NuGet Package Manager Console: `Tools > NuGet Package Manager > Package Manager Console`.

Type the following command at the Package Manager Console:

    PM> Install-Package NServiceBus.Host

NOTE: When prompted to reload the project, click reload. To read more about how Nuget deals with dependencies and to ensure that the project is updated to latest version of the NServiceBus core.

The package installation process adds references to NServiceBus assemblies and creates several boiler template files in the Client project.

For example, `EndpointConfig.cs` is used to configure the project endpoints.

To change the configuration to `Client`, open the `EndpointConfig.cs` file that was just created for you and add `AsA_Client` as shown:

```C#
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
```

 You will add more code to the 'Client' project later on but now we are going to concentrate on the area that will handle our order requests.

### Creating the Messages Project

First lets add a new Class Library project and call it 'Ordering.Messages'.

[![](create-messages.png)](create-messages.png)

The Messages project is the container of message definitions. This project will be shared between the client and server so both sides agree on the typed message descriptions.

Install the `NServiceBus` NuGet package for this new project:

At the Package Manager Console, type:

    PM> Install-Package NServiceBus -ProjectName Ordering.Messages

NOTE: For version 4.x, install the `NServiceBus.Interfaces` package. As of version 5.x, `NServiceBus.Interfaces` package has been deprecated.

Add a command with a property to hold a product name:

Delete `Class1.cs` and add a class and name it `PlaceOrder.cs` (or if you want you can rename the file to `PlaceOrder.cs`).

Implement the PlaceOrder command in `PlaceOrder.cs`.

Replace the content of `PlaceOrder.cs` with the following code:

<!--import PlaceOrder-->

### Creating the Server Project

You are now ready to create the orders processing server. Add a new class library project and name it 'Ordering.Server'.

[![](create-server.png)](create-server.png)

Install the `NServiceBus.Host` NuGet package for this new project:

At the Package Manager Console, type:

    PM> Install-Package NServiceBus.Host -ProjectName Ordering.Server

NOTE: When prompted to reload the project, click reload. To read more about how Nuget deals with dependencies and to ensure that the project is updated to latest version of the NServiceBus core.

For the server side to understand and interpret the message content, add a reference to the `Messages` project you created earlier:

Right click References in the `Ordering.Server` Project -\> Add Reference -\> Ordering.Messages.

Rename `Class1.cs` to `PlaceOrderHandler.cs` and replace the content with the following code:

<!-- import PlaceOrderHandler -->

### Sending the order

We nearly done, all it is left to do is to go back to the `Client` project add a reference to the `Ordering.Messages` project and copy and paste the following code into the `Class1.cs` (if you want you can rename the file to `SendOrder.cs`) file:

<!-- import SendOrder -->

### Selecting a persistence store

At this point, if you try to compile your solution, there should be two errors in the `EndpointConfig.cs` for both the Ordering.Client`` and the `Order.Server` projects. You will see the compiler complain about this line of code in both projects:

````C#
configuration.UsePersistence<PLEASE_SELECT_ONE>();
````

NOTE: If you are using a version of NServiceBus prior to 5.x, you will not see this error and and skip to the next section.

Starting with NServiceBus version 5, it is mandatory to pick the persistence. The quickest way to fix this for the sake of this demonstration is to replace `PLEASE_SELECT_ONE` with `InMemoryPersistence` as shown below:

````C#
configuration.UsePersistence<InMemoryPersistence>();
````

NServiceBus requires a persistence store for handling subscriptions, sagas, timeouts, deduplication, etc. InMemoryPersistence is fine for this simple example, but it is not intended for production use. Please read [Persistence In NServiceBus](/nservicebus/persistence/) for more information on how to select a persistence store and install the correct dependencies.

### Running the solution

You've completed coding the example and now it's time to run the solution. 

To see the complete system, run both the Client and the Server projects together:

To run the `Client` and `Server` projects together so you can see it all working, right click on the `Ordering` solution and select 'Set StartUp Projects...'

![](002-startup.png)

in that screen select 'Multiple startup projects' and set the `Ordering.Client` and `Ordering.Server` action to be 'Start'.

![](003-startup.png)

Finally press 'F5' to run the solution.

Two console application windows should start up

![](run-1.png)

Hit enter (while the Client console is in focus) and you should see 'Order for Product: New shoes placed' in one of them.

![](run-2.png)