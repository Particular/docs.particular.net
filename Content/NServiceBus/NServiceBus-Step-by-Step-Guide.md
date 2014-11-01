---
title: NServiceBus Step by Step Guide - code first
summary: Get started with NServiceBus, step by step, code-first.
tags: []
---

In this tutorial we are going to create a very simple ordering system that will send messages from a client to a server. The ordering system includes three projects: Client, Server, and Messages, to complete this tasks we will execute the following steps:

1.  [Creating the Client project](#Creating-the-Client-project)
2.  [Creating the Messages project](#Creating-the-Messages-Project)
3.  [Creating the Server project](#Creating-the-Server-Project)
4.  [Sending the order](#Sending-the-order)
5.  [Running the solution](#Running-the-solution)

The complete solution code can be found [here](https://github.com/Particular/NServiceBus.Msmq.Samples/tree/master/Documentation/001_OrderingSendOnly)

### Creating the Client project

Let's start by creating a 'Client' project that will send order requests to a NServiceBus endpoint.

Open Visual Studio as administrator, create a new 'Class Library' Project name it 'Ordering.Client', and name the solution 'Ordering'.

![](001_new_solution.png)

![](Package_manager_console.png)

We now need to add references the NServiceBus assemblies and the quickest and easiest way to do that is to use NuGet Package Manager Console.

Open the NuGet Package Manager Console: `Tools > NuGet Package Manager > Package Manager Console`.

Type the following command at the Package Manager Console:

    PM> Install-Package NServiceBus.Host

NOTE: When prompted to reload the project, click reload

The package installation process adds references to NServiceBus assemblies and creates several boiler template files in the Client project.

For example, 'EndpointConfig.cs' is used to configure the project endpoints, and by default the configuration is set to Server.

To change the configuration to 'Client', open the 'EndpointConfig.cs' file that was just created for you and replace this line:


```C#
namespace Ordering.Client
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Server
    {
    }
}
```

 with

```C#
namespace Ordering.Client
{
    public class EndpointConfig : IConfigureThisEndpoint, AsA_Client
    {
    }
}
```

 You will add more code to the 'Client' project later on but now we are going to concentrate on the area that will handle our order requests.

### Creating the Messages Project

First lets add a new Class Library project and call it 'Ordering.Messages'.


[![](Creat_Messages.png)](Creat_Messages.png)

The Messages project is the container of message definitions. This project will be shared between the client and server so both sides agree on the typed message descriptions.

Install the 'NServiceBus' NuGet package for this new project:

At the Package Manager Console, type:

    PM> Install-Package NServiceBus -ProjectName Ordering.Messages

NOTE: For version 4.x, install the NServiceBus.Interfaces package. As of version 5.x, this package has been deprecated.

Add a command with a property to hold a product name:

Delete 'Class1.cs' and add a class and name it 'PlaceOrder.cs' (or if you want you can rename the file to 'PlaceOrder.cs').

Implement the PlaceOrder command in 'PlaceOrder.cs'.

Replace the content of 'PlaceOrder.cs' with the following code:

```C#
namespace Ordering.Messages
{

    public class PlaceOrder : ICommand
    {
        public Guid Id { get; set; }

        public string Product { get; set; }
    }
}
```

### Creating the Server Project

You are now ready to create the orders processing server. Add a new class library project and name it 'Ordering.Server'.


[![](Creat_Server.png)](Creat_Server.png)

Install the 'NServiceBusHost' NuGet package for this new project:

At the Package Manager Console, type:

    PM> Install-Package NServiceBus.Host -ProjectName Ordering.Server

NOTE: When prompted to reload the project, click reload

For the server side to understand and interpret the message content, add a reference to the 'Messages' project you created earlier:

Right click References in the 'Ordering.Server' Project -\> Add Reference -\> Ordering.Messages.

Rename 'Class1.cs' to 'PlaceOrderHandler.cs' and replace the content with the following code:

```C#
using NServiceBus;
using Ordering.Messages;

namespace Ordering.Server
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public IBus Bus { get; set; }

        public void Handle(PlaceOrder message)
        {
            Console.WriteLine(@"Order for Product:{0} placed with id: {1}", message.Product, message.Id);
        }
    }
}
```


### Sending the order

We nearly done, all it is left to do is to go back to the 'Client' project add a reference to the 'Ordering.Messages' project and copy and paste the following code into the 'Class1.cs' (if you want you can rename the file to 'SendOrder.cs') file:

```C#
using NServiceBus;
using Ordering.Messages;

namespace Ordering.Client
{
    public class SendOrder : IWantToRunWhenBusStartsAndStops
    {
        public IBus Bus { get; set; }

        public void Start()
        {
            Console.WriteLine("Press 'Enter' to send a message.To exit, Ctrl + C");

            while (Console.ReadLine() != null)
            {
                var id = Guid.NewGuid();

                Bus.Send("Ordering.Server", new PlaceOrder() { Product = "New shoes", Id = id});

                Console.WriteLine("==========================================================================");
                Console.WriteLine("Send a new PlaceOrder message with id: {0}", id.ToString("N"));
            }
        }
        public void Stop()
        {
        }
    }
}
```

NOTE: The code above is version 4.x and above, the 3.x interface 'IWantToRunAtStartup' has been replaced with 'IWantToRunWhenBusStartsAndStops'

### Selecting a persistence store

At this point, if you try to compile your solution, there should be two errors in the 'EndpointConfig.cs' for both the Ordering.Client and the Order.Server projects. You will see the compiler complain about this line of code in both projects:

````C#
configuration.UsePersistence<PLEASE_SELECT_ONE>();
````

NOTE: If you are using a version of NServiceBus prior to 5.x, you will not see this error and and skip to the next section.

The quickest way to fix this for the sake of this demonstration is to replace `PLEASE_SELECT_ONE` with `InMemoryPersistence` like so:

````C#
configuration.UsePersistence<InMemoryPersistence>();
````

NServiceBus requires a persistence store for handling subscriptions, sagas, timeouts, deduplication, etc. InMemoryPersistence is fine for this simple example, but it is not intended for production use. Please read [Persistence In NServiceBus](persistence-in-nservicebus.md) for more information on how to select a persistence store and install the correct dependencies.

### Running the solution

You've completed coding the example and now it's time to run the solution. 

To see the complete system, run both the Client and the Server projects together:

To run the 'Client' and 'Server' projects together so you can see it all working, right click on the 'Ordering' solution and select 'Set StartUp Projects...'


![](002_strartup.png)

in that screen select 'Multiple startup projects' and set the 'Ordering.Client' and 'Ordering.Server' action to be 'Start'.


![](003_strartup.png)

Finally press 'F5' to run the solution.

Two console application windows should start up

![](run_1.png)

Hit enter (while the Client console is in focus) and you should see 'Order for Product: New shoes placed' in one of them.

![](run_2.png)
